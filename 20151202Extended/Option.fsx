open System
// Option wird vielfältig in funktionalen Sprachen verwendet
// Deklaration
type Option<'a> =   // use a generic definition 
| Some of 'a        // valid value 
| None              // missing

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
