// Tuple
// Typ Deklaration
type IntTuple = int * int
type IntTuple' = int * int * int

// Ad hoc construction
let intTuple = 1,1
let intTupleTriple = 1,1,1

// deconstruction
let t,t' = intTuple
let s,s',s'' = intTupleTriple

// Structural Equality
let intTuple' = 1,1

intTuple' = intTuple // Es sind verschiedene Instanzen, jedoch wird der Abgleich richtig ausgeführt.  

let pmTuple x = // 1. ohne den letzten 2. Typ der function aufzeigen
    match x with
    | (1,1) -> 0
    | (1,2) -> 0
    | (2,2) -> 0
    | _ -> 2 // Exhaustiveness = Ausschöpfung

// Aufrufe in interactive zeigen

