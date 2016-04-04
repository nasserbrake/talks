// Record
type ComplexNumber = { real:float; imaginary:float; }
type GeoCoord = { lat:float; long:float; }

type TransformGeoCoord = { coordinates:GeoCoord; transform: GeoCoord -> GeoCoord; } // sieht nicht nur aus wie eine lambda aus!

// construction
let n = { real = 1.0; imaginary = 1.0;} // Ich kann die records nur vollständig angeben wegen der Immutability, "mache ich später zu Ende" geht nicht!
let c = { lat = 1.0; long = 1.0; }
let t = { coordinates = c; transform = fun c -> c } 

// Deconstruction
let { real=real'; imaginary=imaginary'; } = n

// Structural Euqality
{ real = 1.0; imaginary = 1.0;} = { real = 1.0; imaginary = 1.0;}
{ real = 1.0; imaginary = 1.0;} = { real = 1.0; imaginary = 1.1;}

// Copy, sehr hilfreich, macht immutability 
let n' = { n with real = 2.0; }

// Structural Equality
n' = n

// Pattern matching 
let pmRecord r = 
    match r with
    | { real = 1.0 } -> 1.0
    | { imaginary = 1.0 } -> 1.0
    | _ -> 2.0





