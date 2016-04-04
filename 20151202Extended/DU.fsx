// DU
// Deklaration
type Shape =
| Circle of radius : float // Circle ist hier sowohl der Name als auch der Constructor, habe also in diesem Fall drei Konstruktore
| Rectangle of width : float * length: float
// side left without a label!
| Square of side : float 

// Construction Achte auf das typ der Ausdrücke: immer nur Shape
let rect = Rectangle(length = 1.3, width = 10.0)
let circ = Circle (1.0)
let square = Square(5.0)

// Structural equality
rect = circ
Circle (1.0) = Circle (1.0) 
Circle (1.0) = Circle (1.1) 


// Pattern matching muss ausschöpfend (exhaustive) sein
let pmDU du = 
    match du with
    | Rectangle(w,l) -> w + l
    | Circle(r) -> r
    | Square(s) -> 4.0 * s

// Pattern matching with escape ist immer exhaustive!
let pmDU' du = 
    match du with
    | Rectangle(w,l) -> w + l
    | _ -> 0.0 // this operation should return 0.0 for all values other than a Rectangle

// ACHTUNG: Achtung: this is the only way to peek into a DU
// rect, circ, und square are of type Shape.  Es kann jede union case sein. Daher muss ich alle Fälle berücksichtigen

// Eine weitere Möglichkeit, aber dann muss ich aller benennen!
type Rectangle = {width: float; length: float;}
type Circle = {radius: float; }
type Prism = {width: float; ``base``:float; height: float; } // base ist ein Schlüsselwort

// Deklaration
type Shape' =
| Rectangle of Rectangle
| Circle of Circle
| Prism of Prism

