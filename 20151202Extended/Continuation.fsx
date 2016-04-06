// Aus https://fsharpforfunandprofit.com/posts/computation-expressions-intro/ entnommen
// Wie immer, kann ich die Seite https://fsharpforfunandprofit.com nur empfehlen!

// Scott Wlaschin gibt 4 Grunde an warum man computation expression verwenden kann
// - perform some side-effect between each step (logging example).
// - handle errors elegantly so that we could focus on the happy path (safe division example).
// - return early with the first success (multiple dictionary lookup).
// - hide the use of callbacks and avoid the "pyramid of doom" (async).


module ``Of Async Fame`` = 
    open System.Net
    let req1 = HttpWebRequest.Create("http://tryfsharp.org")
    let req2 = HttpWebRequest.Create("http://google.com")
    let req3 = HttpWebRequest.Create("http://bing.com")

    req1.BeginGetResponse((fun r1 -> 
        use resp1 = req1.EndGetResponse(r1)
        printfn "Downloaded %O" resp1.ResponseUri

        req2.BeginGetResponse((fun r2 -> 
            use resp2 = req2.EndGetResponse(r2)
            printfn "Downloaded %O" resp2.ResponseUri

            req3.BeginGetResponse((fun r3 -> 
                use resp3 = req3.EndGetResponse(r3)
                printfn "Downloaded %O" resp3.ResponseUri

                ),null) |> ignore
            ),null) |> ignore
        ),null) |> ignore    

    // ODER
    async{
        use! resp1 = req1.AsyncGetResponse()
        printfn "Downloaded %O" resp1.ResponseUri
        use! resp2 = req2.AsyncGetResponse()
        printfn "Downloaded %O" resp2.ResponseUri
        use! resp3 = req3.AsyncGetResponse()
        printfn "Downloaded %O" resp3.ResponseUri

    } |> Async.RunSynchronously

module ``Logging ohne computation expressions`` =
    let log x = printfn "Ausdruck ist %A" x

    let loggerWorkFlow = 
        let x = 42
        log x
        let y = 43
        log y
        let z = 44
        log z
        z

module ``Logging mit computation expressions`` =
    type LoggingBuilder() = 
        let log x = printfn "Ausdruck ist %A" x
        member __.Bind(x,f) = 
            log x
            f x
        member __.Return(x) =
            x

    let logger = new LoggingBuilder()

    let loggerWorkFlow = 
        logger
            {
                let! x = 42
                let! y = 43
                let! z = 44
                return z
            }


module ``Verschachtelung ohne Vielleicht`` = 
    let divideBy bottom top = 
        if bottom = 0
        then None
        else Some(top/bottom)

    // Eine solche Operation soll eine Option zurückgeben für den Fall dass "Nichts" zurückgegeben werden kann
    let divideByWorkFlow init x y z = 
        let a = init |> divideBy x

        match a with
        | None -> None
        | Some a' -> 
            let b = a' |> divideBy y
            match b with
            | None -> None
            | Some b' ->
                let c = b' |> divideBy z
                match c with
                | None -> None
                | Some c' -> Some c'      
    divideByWorkFlow 12 3 2 1
    divideByWorkFlow 12 2 0 1
    
module ``Vielleicht ohne Verschachtelung`` = 
    type MaybeBuilder() =
        member __.Bind(x, f) = 
            match x with
            | None -> None
            | Some a -> f a

        member __.Return(x) = Some x

    let maybe = new MaybeBuilder()

    let divideBy bottom top = 
        if bottom = 0
        then None
        else Some(top/bottom)

    let divideByWorkFlow init x y z = 
        maybe 
            {
            let! a = init |> divideBy x
            let! b = a |> divideBy y
            let! c = b |> divideBy z
            return c
            }

module ``Vielleicht mit logging`` = 
    type MaybeLoggingBuilder() =
        let log x = printfn "Ausdruck ist %A" x
        member __.Bind(option, continuation) = 
            log option
            Option.bind continuation option
        member __.Return(value) = Some value

    let maybeLogger = new MaybeLoggingBuilder()

    let divideBy bottom top = 
        if bottom = 0
        then None
        else Some(top/bottom)

    let divideByWorkFlow init x y z = 
        maybeLogger 
            {
            let! a = init |> divideBy x
            let! b = a |> divideBy y
            let! c = b |> divideBy z
            return c
            }

    divideByWorkFlow 12 3 2 1

// Es gibt mehr, viel mehr dazu, siehe dafür https://msdn.microsoft.com/de-de/library/dd233182.aspx#Anchor_1


// https://fsharpforfunandprofit.com/posts/computation-expressions-wrapper-types/
module ``Maybe`` = 

    type Result<'a> = 
        | Success of 'a
        | Error of string

    type CustomerId =  CustomerId of string
    type OrderId =  OrderId of int
    type ProductId =  ProductId of string

    // Eine Sequenz von Operationen die jeweils mit Erfolg/Fehler enden kann
    // Output der ersten Funktion ist der Input der zweiten (beim Happy Path!)
    let getCustomerId name = 
        if(name = "")
        then Error "getCustomerIdFromString fehlgeschlagen"
        else Success (CustomerId name)

    // Achtung: Zerlegung des single case union in der Deklaration des Parameters!
    let getLastOrderForCustomer (CustomerId custId) = 
        if(custId = "")
        then Error "getLastOrderForCustomer"
        else Success (OrderId 123)

    let getLastProductForOrder (OrderId orderId) =
        if (orderId  = 0) 
        then Error "getLastProductForOrder failed"
        else Success (ProductId "Product456")

    // Nur im Falle dass eine ProductId ermittelt werden könnte, wird ein Wert zurückgegeben das nicht Error ist!
    let product name = 
        let customerIdResult = getCustomerId name
        match customerIdResult with 
        | Error e -> Error e
        | Success custId ->
            let lastOrderIdResult = getLastOrderForCustomer custId 
            match lastOrderIdResult with 
            | Error e -> Error e
            | Success orderId ->
                let lastProductIdResult = getLastProductForOrder orderId 
                match lastProductIdResult with 
                | Error e -> Error e
                | Success productId ->
                    printfn "Product is %A" productId
                    lastProductIdResult

    // Geht besser mit Builder!
    type DbResultBuilder() = 
        member __.Bind(m, f) = 
            match m with
            | Error e -> Error e
            | Success a -> f a
        member __.Return(x) = Success x

    let dbresult = new DbResultBuilder()

    let product' name = 
        dbresult {
            let! customerId = getCustomerId name
            let! lastOrderId = getLastOrderForCustomer customerId
            let! lastProductId = getLastProductForOrder lastOrderId
            return lastProductId
        }

// Weitere Variante: Error als DU definieren und dann per pattern matching reagieren