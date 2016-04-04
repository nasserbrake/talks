// Tuple
// Type declaration
type IntTuple = int * int
type IntTuple' = int * int * int

// Tuple type declaration is not nominal.  Any int * int is equal to any other int * int

// Ad hoc construction
let intTuple = 1,1
let intTupleTriple = 1,1,1

// deconstruction
let t,t' = intTuple
let s,s',s'' = intTupleTriple

// Structural Equality
let intTuple' = 1,1

intTuple' = intTuple // Different instances/expressions, yet the comparison is structural and not reference based

// 1. ohne den letzten Pattern läuft es nicht, anzeigen
// 2. Typ der function aufzeigen
let pmTuple x = 
    match x with
    | (1,1) -> 0
    | (1,2) -> 0
    | (2,2) -> 0
    | _ -> 2 // Exhaustiveness = Ausschöpfung



