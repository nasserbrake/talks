// Function
// A special function!
let (|>) x f = f x

let add1 x = x + 1
let isEven x = x % 2 = 0

let v = List.sum(List.filter isEven (List.map add1 [0..9]))

[0..9] 
|> List.map add1
|> List.filter isEven
|> List.sum
