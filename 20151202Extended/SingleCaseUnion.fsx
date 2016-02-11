﻿// Besonderer Fall: single case
// Deklaration eines type alias
type CustomerId = int
let cid = 5

// Verwendung bei der Annotation
let cIdf (id:CustomerId):string = "Ist ja Kunde"

type OrderId' = int
let oid = 5

// Ich kann die OrderId an eine Funktion übergeben die einen CustomerId erwartet
cIdf oid

// Deklaration eines Single Case Union
type CustomerIdTypeName = CustomerIdConstructorFunction of int // 
type OrderId = OrderId of int
// Construction
let cid' = CustomerIdConstructorFunction 5
let oid' = OrderId 5

// Deconstruction
let (CustomerIdConstructorFunction cid'') = cid'

// das geht jetzt nicht mehr!
// cIdf oid'

// Structural equality
let cid'' = CustomerIdConstructorFunction 5
cid'' = cid'