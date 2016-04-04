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

// Ich brauche die ganze Liste nicht um die Berechnung durchzuführen, ich kann einen akkumulator schreiben: 
let acc sumSoFar i = 
    let i' = add1 i
    let even = isEven i'
    if even then
        sumSoFar + i'
    else
        sumSoFar

// Fold faltet die Liste zu einem einzigen Wert, die Zwischenergebnise werden nicht behalten
// Die laufende Summe werden weitergereicht und nicht gespeichert
type fold<'a,'b> = ('a -> 'b -> 'a) -> 'a -> 'b list -> 'a
// Das erste Parameter ist ein Akkumulator: Bisheriger Wert, aktueller Wert, neuer Wert
// Das zweite Paramter ist der Initialwert
// Das dritte Paramter ist die Eingabe Liste
// und zu guter Letzt, das Ergebnis: Ein Wert

let mitFold l = List.fold acc 0 l

let sum' = mitFold [0..9]

// Eine weitere Module Funktion ist scan (https://cockneycoder.wordpress.com/2016/02/16/working-with-running-totals-in-f/)
// Scan Erzeugt eine Liste der laufenden Summe aus einer Liste
type scan<'a,'b> = ('a -> 'b -> 'a) -> 'a -> 'b list -> 'a list
// Das erste Parameter ist ein Akkumulator: Bisheriger Wert, aktueller Wert, neuer Wert
// Das zweite Paramter ist der Initialwert
// Das dritte Paramter ist die Eingabe Liste
// und zu guter Letzt, das Ergebnis: Eine Liste aus Werten

let mitScan l = List.scan acc 0 l

let sum'' = mitScan [0..9]




