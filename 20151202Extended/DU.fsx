// DU
// Deklaration
type Shape =
| Rectangle of width : float * length : float // Rectangle ist hier sowohl der Name als auch der Constructor, habe also in diesem Fall drei Konstruktore
| Circle of radius : float
| Prism of width : float * float * height : float // base ist nicht bennant worden! Brauche ich auch nicht

// Construction Achte auf das typ der Ausdrücke: immer nur Shape
let rect = Rectangle(length = 1.3, width = 10.0)
let circ = Circle (1.0)
let prism = Prism(5., 2.0, height = 3.0)

// Structural equality
rect = circ
Circle (1.0) = Circle (1.0) 
Circle (1.0) = Circle (1.1) 


// Pattern matching
let pmDU du = 
    match du with
    | Rectangle(w,l) -> w + l
    | Circle(r) -> r
    | Prism(w,b,h) -> w + b + h

// ACHTUNG: Das ist auch die einzige Möglichkeit einen DU zu zerlegen
// rect, circ, und prism sind alle vom Typ Shape.  Es kann jede union case sein. Daher muss ich alle Fälle berücksichtigen

// Eine weitere Möglichkeit, aber dann muss ich aller benennen!
type Rectangle = {width: float; length: float;}
type Circle = {radius: float; }
type Prism = {width: float; ``base``:float; height: float; } // base ist ein Schlüsselwort

// Deklaration
type Shape' =
| Rectangle of Rectangle
| Circle of Circle
| Prism of Prism

