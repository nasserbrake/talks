
module ``intro`` =
    // Expressions
    let x = 1 // SW: An expression is a onetime association of the name “x” with the value 1
    let y = 1.0
    let z = "string"
    let ``Can also use labels such as this one"!'$§%&`` = 1

    // "val it : whatever" // it designates that last expression that was evaluated in the REPL

    // Declaration of a function
    let f (y:int):int = y + 1

    // Functions: Annotation is often optional
    let f' x = x + 1
    let f'' x = x + 1.0
    let f''' x = x + "Mehr"

module ``Function Signature`` =
    type F<'a> = ('a -> bool) -> 'a list -> 'a list // Filter
    type F'<'a> = 'a list -> 'a // Sum
    type F''<'a,'b> = ('a -> 'b) -> 'a list -> 'b list // Map
    type F'''<'a> = 'a -> (unit -> string) // Something that takes a value, returns a function that is parameterless and returns a string

module ``Function can be HOF`` =

    // Types and signatures for Functions
    type F = int -> int  // take an int THEN return an int
    type F' = int -> int -> int // take an int THEN take an int THEN return an int

    // Function as a parameter
    let g (f:F) x = f x
    let g' (f:F') x y = f x y

    let funSign = g (fun x -> x + 2) 3
    let funSign' = g' (fun x y -> x + y) 3 4

    // Actually this definition is not needed in this case, because type inference creates a generic version of the function
    let g'' f x = f x // f x <> f x! The first one is a list of parameters.  The second is actual execution
    let funSignGeneric = g'' (fun x -> x + 2) 3

    let add2 x = x + 2 // val add2 : x:int -> int
    let evalAndAdd2 fn x = fn x + 2 // val evalAndAdd2 : fn:('a -> int) -> x:'a -> int

    let x = add2 2
    let y = evalAndAdd2 add2 2










