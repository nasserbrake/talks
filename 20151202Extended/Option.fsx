open System
// Option wird vielfältig in funktionalen Sprachen verwendet
// Deklaration
//type Option<'a> =   // use a generic definition 
//| Some of 'a        // valid value 
//| None              // missing

type SomeString = string option

// Construction
let someString = Some "string" 
let none = None // None hat keinen Typ, anders als eine null basierend auf einen String!

// Zerlegung
// Keine, ist ein DU, daher nur pattern matching ist vorhanden

// Structural Equality
someString = Some "string"

// Pattern matching
let optionMatch s = 
    match s with 
    | Some i -> "Some" 
    | None -> "None"

let eq = optionMatch someString
let eq' = optionMatch none

// Schöne Verwendung
let parseInt intStr = 
   try
      let i = System.Int32.Parse intStr
      Some i
   with _ -> None

let showParseResult x = 
    match x with
    | Some i -> sprintf "Zahl %i" i
    | None -> sprintf "Keine Zahl"

let parseResult = parseInt "5"
let parseResult' = parseInt "Keine Zahl"

showParseResult parseResult
showParseResult parseResult'

// Option module offers a host of useful functions
// bind: (f:'T -> 'U option) -> 'T option -> 'U option
// Take a function that can map a value fo 'T into a value of U'
// Provide a value of 'T
// Expect either None of 'U option
// A possible implementation!
// let bind (f:'T -> Option<'U>) (x:Option<'T>) : Option<'U> = 
//     match x with    
//     | Some v -> f v
//     | None -> None
//     
let optionBindFun x = Some (sprintf "asdfasd %i" x)
let v = Some(1)
let bindStringOption = Option.bind optionBindFun v 

// exists (f:'T -> bool)  -> 'T option -> bool
// let exists f opt = 
//     match opt with
//     | None -> false
//     | Some x -> f x
let isValue opt value = Option.exists (fun elem -> elem = value) opt
isValue (Some(2)) 2
isValue (Some(2)) 3
isValue (None) 3

// map: (f:'T -> 'U) -> 'T option -> 'U option
// let map f opt = 
//     match opt with
//     | None -> None
//     | Some v -> Some(f v)

// filter: (f:'T -> bool) -> 'T option -> 'T option
let isLarger opt value = Option.filter (fun elem -> elem > value) opt
isLarger (Some(2)) 1
isLarger (None) 1


// These are just basic operations but they are consistent with the other operations in the other standard modules
// Say you want to parse a string as int, proof that it's even, then add ten to it

let ``add ten interleaved`` numberAsText = 
    match Int32.TryParse(numberAsText) with // TryParse return a tuple!
    | false, _ -> None 
    | _, value -> match (value % 2 = 0) with
                  | false -> None
                  | true -> value + 10 |> Some  
                  
let inline parse< ^T when ^T : (static member TryParse : string * byref< ^T > -> bool) and  ^T : (new : unit -> ^T) > valueToParse =
    let mutable output = new ^T()
    let parsed = ( ^T : (static member TryParse  : string * byref< ^T > -> bool ) (valueToParse, &output) )
    match parsed with
    | true -> output |> Some
    | _ -> None

/// Note that I don't need to include logic pertaining to the None branch
/// This is handled implicitly    
let ``add ten using module functions`` numberAsText =
    let parsed = parse<int> numberAsText
    let even = Option.filter (fun x -> x % 2 = 0) parsed
    let added = Option.bind (fun x -> x + 10 |> Some ) even
    added

// Option is great coupled with computation expressions
open System

/// Define a computation expression
type MaybeBuilder() =
    member __.Bind(option, continuation) = Option.bind continuation option
    member __.Return(value) = Some value

/// Create an instance of the builder
let maybe = MaybeBuilder()

// Now use the builder in a real function!

/// Takes in a string version of a number, tries to convert it to an int, adds 10, and returns the string representation back out
let addTen numberAsText =
    maybe {
        let! x =
            match Int32.TryParse(numberAsText) with // TryParse return a tuple!
            | false, _ -> None 
            | _, value -> Some value
        printfn "You provided me a convertable number!"
        
        let! y = 
            match (x % 2 = 0) with
            | true -> Some x
            | false -> None
        printfn "You provided me an even number, gonna add ten for you!"
        
        return x + 10
    }

/// Gets back a string option
let values = ["15"; "n/a"; "12"; ]
values |> List.map ``add ten interleaved``
values |> List.map ``add ten using module functions``
values |> List.map addTen

values |> List.map addTen |> List.filter Option.isNone |> List.length


