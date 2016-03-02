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