// Aus https://fsharpforfunandprofit.com/posts/computation-expressions-intro/ entnommen

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
        member __.Bind(x, f) = 
            match x with
            | None -> 
                log "None"
                None
            | Some a -> 
                log a
                f a

        member __.Return(x) = Some x

    let maybe = new MaybeLoggingBuilder()

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