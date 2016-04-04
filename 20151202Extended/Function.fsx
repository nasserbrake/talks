// Aufgabe:
// - Die Zahlen von 0 -> 9
// - Inkrementiere jede Zahl um 1
// - Filtere die ungeraden Zahlen heraus
// - Ermittle die Summe der Zahlen

let add1 x = x + 1
let isEven x = x % 2 = 0 // ACHTUNG: = ist sowohl Zuweisungs- als auch Gleichheitsoperator

let f l = List.sum(List.filter isEven (List.map add1 l)) // Verschachtelung

let sum = f [0..9]

// Function Pipe to the rescue
// A special (inline) function!
let (|>) x f = f x

let f' l = 
    l
    |> List.map add1
    |> List.filter isEven
    |> List.sum

// Ich brache die ganze Liste nicht um die Berechnung durchzuführen
let fold sumSoFar i = 
    let i' = add1 i
    let even = isEven i'
    if even then
        sumSoFar + i'
    else
        sumSoFar

let f'' l = List.fold fold 0 l

let sum' = f'' [0..9]

