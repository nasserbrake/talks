(**
- title : F# Das Typsystem
- description : Einführung in das Typsystem von F#
- author : Nasser Brake
- theme : night
- transition : none

***
## F# 
## Das Typsystem
 
#### Nasser Brake
#### http://www.nasserbrake.de
#### https://www.github.com/nasserbrake

***
### Tuple

A set of expressions

* Consisting of different types
* The order of the expressions is predetermined, identifies a tuple
* A tuple is an ad-hoc type

' Auch wenn ich einen Typ definieren kann, kann ich einzelne Werte diesen Typen nicht zuordnen.  Die einzige Zuordnung ist den Bestandteilen und deren Reihenfolge

---
### Tuple: Construction

* Construction using comma (w/o parenthesis)
* Product of two Domains: Cartesian sum

*)


type IntTuple = int * int // Every int 'times' every int
let intTuple  = 1,1

type IntStringTuple = int * string // every int 'times' every string
let intStringTuple  = 1,"string"

type TripleIntTuple = int * int * int
let tripleIntTuple = 1,2,3

(**

---
### Tuple: Composition

Create complex types from primitives/simpler types

*)

type Complex = float * float
let complex  = 1.0,1.0

type Composition = IntStringTuple * Complex
let composition  = intStringTuple,complex

(**

---
### Tuple: Deconstruction/Zerlegung
Deconstruct a tuple into its constituent parts

' In F# you can use '' to designate variants of an expression
' Paranthesis are not obligatory, can help clarify intent

*)
let complex' = 1.0,2.0
let c',c'' = complex'
let composition' = intStringTuple,complex'
let k',k'' = composition'

(** <div style="display: none" > *)
(*** define-output:complex ***)
printf "c' = %A | c'' = %A" c' c''
(*** define-output:komposition ***)
printf "k' = %A | k'' = %A" k' k''
(** </div> *)

(*** include-output: complex ***)
(*** include-output: komposition ***)

(**


---
### Tuple: Strukturelle Gleichheit/Structural equality

* Type and order: can I compare two expressions?
* Expressions: are two expressions equal?

*)
let equal = complex' = complex

(** <div style="display: none" > *)
(*** define-output:Type-Equality ***)
printf "equal = %b" equal
(** </div> *)

(*** include-output: Type-Equality ***)

(**
' Typdefinition ist hilfreich bei Signaturen

---
### Tuple: Pattern Matching/https://de.wikipedia.org/wiki/Pattern_Matching

*)
let matchTuple c = 
    match c with
    | 0.0,0.0 -> "0.0,0.0"
    | 1.0,1.0 -> "0.0,0.0"
    | 1.0,2.0 -> "1.0,1.0"
    | _,_ -> "otherwise"

let result   = matchTuple complex
let result'  = matchTuple complex'
let result'' = matchTuple (2.0,2.0) // Brauche jetzt Klammern!

(** <div style="display: none" > *)
(*** define-output:Tuple-PatternMatching ***)
printf "result = %A | " result 
printf "result' = %A | " result'
printf "result'' = %A " result''
(** </div> *)
(*** include-output: Tuple-PatternMatching ***)

(**

' The compiler produces compile-time errors for missing union-cases

---
### Tuple: Usage in the .NET API

TryParse method that returns two expressions

* bool: success/failure of parse operation
* expression: should the parse operation successed

' _ is a placeholder for whatever else, AND I promise not to touch this thing

*)

open System
let showParseResult result = 
    match result with
    | true,value -> sprintf "Value parsed is %s" (value.ToString())
    | false,_ -> "Value couldn't be parsed" 

let tryParseResult = Int32.TryParse "No number" |> showParseResult
let tryParseResult' = Int32.TryParse "1" |> showParseResult


(** <div style="display: none" > *)
(*** define-output:Tuple-TryParsePatternMatching ***)
printf "result = %A | " tryParseResult 
printf "result' = %A" tryParseResult'
(** </div> *)

(*** include-output: Tuple-TryParsePatternMatching ***)

(**



***
###  Record

Eine bennante Menge von benannten Elementen

* Die Reihenfolge der Deklaration ist nicht relevant
* Ist kein ad-hoc Typ

' Wichtig: es ist auch nur ein Tuple, ein multiplication type
' Ist wie ein POCO, mit Constructor, private backing fields, public setters, equality overloads
' POCO with lightweight syntax

---
### Record: Deklaration

*)

type ComplexNumber = { Real: float; Imaginary: float; }
type GeoCoord = { Lat: float; Long: float; }

(**
' Semikolon ist hier der Trenner

---
### Record: Construction
*)

let complexNumber = { Real = 1.0; Imaginary = 1.0; }
let hamburg       = { Lat = 53.553260805869805; Long = 9.993009567260742; }

(**
' Construction ist ähnlich wie die Deklaration, nur werden : durch = ersetzt
' F# ist in der Lage anhand der Namen der Member den Typ zu erkennen -> Typinferenz

---
### Record: Construction
Typ kann bei der Construction qualifiziert werden
*)

let complexNumber' = { ComplexNumber.Real = 2.0; Imaginary = 2.0; }
let hamburg'       = { GeoCoord.Lat = 53.553260805869805; Long = 9.993009567260742; }

(**
' Dies ist dann besonders hilfreich, wenn Typen die gleichen Bezeichner verwenden

---
### Record: Construction
Keine halben Sachen: 

* Alle Werte müssen angegeben werden
* Kein Wert kann verändert werden nach der Construction

*)
// let complexNumber' = { ComplexNumber.Real = 1.0; } // Imaginary gebe ich später an
(**

' Funktionales Vorgehen: Wenn ich feststelle, dass ich den Fall konkret habe, dass ein Wert nicht immer angegeben werden kann, dann erstelle ich hierfür einen Typ

---
### Record: Deconstruction
Record in seine Bestandteile zerlegen

*)

let { Real = real;  Imaginary = imaginary; } = complexNumber // Alle Member
let { Real = real'; } = complexNumber' // Einzelne
let real'' = complexNumber'.Real // Point Style ist auch möglich für einen Wert

(** <div style="display: none" > *)
(*** define-output:Record-Deconstruction ***)
printf "real = %f | imaginary = %f | real' = %f | real'' = %f" real imaginary real' real''
(** </div> *)

(*** include-output: Record-Deconstruction ***)

(**

---
### Record: Clone With
Da Modifizieren nicht geht, bietet F# vereinfachtes Klonen an

*)

let complexNumber'' = { complexNumber' with Imaginary = 3.0 }

(** <div style="display: none" > *)
(*** define-output:Record-Clone ***)
let { Real = real'''; Imaginary = imaginary''; } = complexNumber''
printf "real = %f | imaginary = %f" real''' imaginary'' 
(** </div> *)

(*** include-output: Record-Clone ***)

(**

'  Dies ist notwendig für den üblichen Fall, dass sich nicht alle Werte geändert haben, sondern nur bestimmte.  Mit der With Syntax kann ich diesen Fall gut abdecken

---
### Record: Structural equality

Two records are equal if

* both are from the same type
* all corresponding values are equal

' ACHTUNG: Wenn zwei unterschiedliche Record Typen (unterschiedlilch bennante) die gleichen Bezeichner haben, sind beide trotzdem nicht über strukturelle Gleichheit vergleichbar.  In anderen funktionalen Sprachen wird die Gleicheit anders gehandhabt

---
### Record: Pattern Matching

*)

let matchRecord c = 
    match c with 
    | { Real = 1.0; Imaginary = 1.0; } -> "Real = 1.0 & Imaginary = 1.0"
    | { Real = 1.0 } -> "Real = 1.0"
    | { Imaginary = 2.0 } -> "Imaginary = 2.0"
    | _ -> "sonst"

let r    = matchRecord { Real = 1.0; Imaginary = 1.0; }
let r'   = matchRecord { Real = 1.0; Imaginary = 2.0; }
let r''  = matchRecord { Real = 2.0; Imaginary = 2.0; }
let r''' = matchRecord { Real = 3.0; Imaginary = 3.0; }

(** <div style="display: none" > *)
(*** define-output:Record-PatternMatching ***)
printf "r = %s | " r
printf "r' = %s" r'
(*** define-output:Record-PatternMatching0 ***)
printf "r'' = %s | " r''
printf "r''' = %s " r'''
(** </div> *)
(*** include-output: Record-PatternMatching ***)
(*** include-output: Record-PatternMatching0 ***)

(**

***
### Discriminated Union

* A number of named union-cases
* A union-case can be empty
* A union-case can consist of a number of values
* Values can be labeled
* A union case is not a type in its own right
* A union-case is best viewed as a constructor case 

*)

type Shape =
| Circle of radius : float
| Rectangle of width : float * length: float
// side left without a label!
| Square of side : float 


(**

' * Looks like an enum but it isn't
' * It would be possible  to define an enum using DU. Interoperability with C# would be brocken.  
' * DU is THE type, example.
' * Single Case Union is also possible is very useful in DDD
' * Closed set: keine Erweiterung
' * Seperation of Data and Behavior: Behavior is not scatterd across classes


---
### DU: Declaration

* Empty Case, consists of a Label, no Data. 
* Composition: Define a record and use as a union-case

*)

type DuExample =
| Empty
| Complex of ComplexNumber
| Coordinate of GeoCoord

(**

---
### DU: Construction

* Use the constructor functions to construct a DU value

*)

// ACHTUNG, can you guess the type of rectangle?
let rectangle = Rectangle(width = 1.3, length = 10.0) 
let circle = Circle (1.0)
let square = Square(3.0)

(**

--- 
### DU: Structural equality

* Union case *and* all constituent values must be equal

*)

let circleEq = Circle (1.0) = Circle (1.0) 
let circleEq' = Circle (1.0) = Circle (1.1) 

(** <div style="display: none" > *)
(*** define-output:DU-Structural equality ***)
printf "circleEq = %A | " circleEq
printf "circleEq' = %A " circleEq'
(** </div> *)
(*** include-output: DU-Structural equality ***)

(**

---
### DU: Deconstruction & Pattern Matching

* rectangle in the example is of type shape, not Shape.Rectangle!
* Looking at a DU value you cannot determine which union case it represents
* Pattern Matching allows "peeking" into the value 
* Deconstruction *must* happen for *all* cases (Exhaustivness)

' Every operation I design to work with type shape, must be designed to "survive" all union cases

---
### DU: Deconstruction & Pattern Matching

*)

let area s = 
    match s with 
    | Rectangle (w,l) -> w*l
    | Circle(r) -> Math.PI*(r ** 2.0) 
    // Leaving out a union-case leads to a compilation error
    | Square(s) -> (s ** 2.0)
let circleArea   = area (Circle (5.0))
let rectangleArea = Rectangle(length = 5.0, width = 5.0) |> area


(** <div style="display: none" > *)
(*** define-output:DU-PatternMatching ***)
printf "Area of circle = %f | " circleArea
printf "Area of   = %f" rectangleArea  
(** </div> *)
(*** include-output: DU-PatternMatching ***)

(**

--- 
### DU: Use

* DUs can be used to model states/transitions
* Exhaustivness leads to less errors: no case is left out

---
### DU: Single Case

* Primitives often possess a special meaning in a business system
* In a system of coordinates, both latitude and longitude are floats
* Each, however, represents a distinct set of values
* Designating single case unions renders operations involving both illegal

' In DDD (Domain Driven Design) spielen diese oft eine wichtige Rolle.
' Z.B. kann ich dadurch Primitives so definieren, dass diese untereinander nicht „kompatibel“ sind, auch wenn diese vom gleichen Typ sind.

*)

// First longitude is the name of the type!
// Second is the name of the constructor

type Longitude = Longitude of float 
type Latitude = LatitudeConstructorFunction of float

let longitude = Longitude(9.993009567260742)
let latitude = LatitudeConstructorFunction(53.553260805869805)

// Der Compiler mag das nicht, es handelt sich um zwei Typen
// let gleich = longitude = latitude 

(**

***
### Option

* Special form of DU
* Found in many functional languages

' In Haskel heißt dieser Typ Maybe, Just, Nothing. in Scala heißt es auch option, some, none.
' Used extensively!

*)

type Option<'a> = // DU with a generic paramter
| Some of 'a // Valid value
| None // ???


(**

---
### Option: Construction

*)

let s = Some "string"
let none = None


(**

---
### Option: Deconstruction and Pattern Matching

* Must handle all cases in order to extract the value of the DU

*)

let optionMatch s = 
    match s with 
    | Some wert -> sprintf "Value: %s" wert
    | None -> ""

let z  = optionMatch (Some("Hello"))
let z' = optionMatch None

(** <div style="display: none" > *)
(*** define-output:Option-PatternMatching ***)
printf "z = %s | " z
printf "z' = %s" z'
(** </div> *)
(*** include-output:Option-PatternMatching ***)

(**

---
### Option: Deconstruction and Pattern Matching
* Option is often the result of an evaluation
* None represents the case that no valid value was constructed

*)

let matchForOption s = 
    match s with 
    | "Sure" -> Some(s)
    | _ -> None

let x  = matchForOption "Sure"
let x' = matchForOption "Not"

(** <div style="display: none" > *)
(*** define-output:Option-Deconstruction ***)
printf "x = %A | x' = %A" x x'
(** </div> *)

(*** include-output: Option-Deconstruction ***)


(**

---
### Option: Use

* Null vs unknown/missing
* Option allows for an explicit designation of missing values
* Accessing missing values becomes a *compile-time* and not a *run-time* error 

' Referenzen auf nicht vorhandene Objekte sind nicht die beste Art und Weise 
' Code Analysis wird verwendet um Warnungen zu geben in C# 7.0.  Keine typ Unterstützung möglich wegen backward compatibility
' C# OO <-> Imperative <-> Fun

---

### Option vs null: Type Safety

* null is a reference to an object that doesn't exist
* The type system is unable to verify if a value equals null
* I can call .Length on a variable that has value null

' Danke Scott Wlaschin!

*)

(**

    [lang=cs]
    using System;
    class Program
    {
        static void Main()
        {
            string s2 = null;
            var len2 = s2.Length; 
            // We know that s2 equals null.  Not so the compiler
        }
    }

*)

(**

---
### Option vs null: Type Safety

* Attempting a similar call on Option causes a compile error

*)

let none' = Option<string>.None
// let length = none.Length // None hat keinen Length, es ist nämlich kein String!

(**

---
### Option vs nullable
* Nullable is only valid for value types, not for reference types
* Option has great support in F# through the runtime

***
### DDD und FP
#### The story hitherto

* Processes of design often assume a threefold division
    * Business people with domain knowledge
    * Business analysts issue design-documents using intermediate formats (i.e. UML)
    * Programmers write software based on design-documents

---
### DDD und FP
#### The story hitherto

* Consequences
    * Programmers don't communicate with those who possess domain-knowledge (not directly)
    * Roundtrip Engineering keeps code and design-documents in sync
    * Business people have no direct interaction with code
    
' Ohne dass sie kotzen

    
---
### DDD und FP
#### The hope

* Verifeiable design-documents
    * You can reason about
    * Compiler verification

>"A good static type system is like having compile-time unit tests" (S. Wlaschin)

---
### DDD und FP
#### The hope: Domain knowledge as a data structure

* Code as design-document
    * Code is the model: no intermediate formats
    * Datenstrukturen are modeled as code
    * Behavior is partially modeled through the type system

>"Making illegal states unrepresentable" (Y. Minsky)

' * Die Fachleute können ihre Dokumente schreiben in den Formaten, mit denen sie vertraut sind, die DEVs können Code schreiben
' * In FP sind sowohl Datenstrukturen (Tuple, Record, DU) als Verhalten (Function) Types: Composition.
' * Domainlogik legt Regeln fest die Definition (Vorname required) und Transformation Einkaufskorb -> Bestellung festlegen. Types erlauben diese Regeln zum Teil darzustellen, der Rest muss dann mit Controlflow Construkte erstellt werden
' * MISU!!! Vielleicht hier einen Diagram malen um das Vorgehen in OO und in FP darzustellen: Offener Raum, nachträgliches Einschränken.  Geschlossener Raum, Quadrat für Quadrat gebaut, nur legale Zustände werden dargestellt

---
### DDD und F#
#### Advantages of the F# type system

* Type system embraces composition
* DU allow a comprehensive representation of states

' * In C# oder in Java ist die Hemmschwelle relativ hoch neue Types zu erstellen. Es gibt sogar das code smell "Primitive Obsession", kein Scherz
' * Komposition erlaubt es einfache Typen zu immer komplexeren zusammenzufassen.  Es ist erstaunlich wie viel dann auf eine Seite passt
' * Macht Nicht-Programmierern weniger Angst
' * Bei Programmierung von Fachanwendungen geht es oft darum, dass ein Objekt mehrere Zustände haben kann. Jeder Zustand hat wiederum eigene Operationen, Fähigkeiten. DU erlauben diese sehr gut darzustellen

---
### DDD und F#
#### Advantages of the F# type system

* Light-weight types: less LOCs, less symbols to consider
* Exhaustivness can lead to correctness


' * Die geringe Zahl von Sonderzeichen/Schlüsselwörtern macht Nicht-Programmierern weniger Angst
' * PUNKT 2: Fehlende Fälle werden vom Compiler erfasst
' * Bei Programmierung von Fachanwendungen, geht es oft darum, dass ein Objekt mehrere Zustände haben kann. Jeder Zustand hat wiederum eigene Operationen, Fähigkeiten. DU erlauben diese sehr gut darzustellen

--- 
### DDD und F#

* Examples from 
    * fsharpforfunandprofit
    * Jane Street (kein F#, OCaml)

---
### DDD und F#
#### More states

*)

type ConnectionState = Connecting | Connected | Disconnected
 
type ConnectionInfo = { 
    State: ConnectionState; 
    Server: System.Net.IPAddress; 
    LastPingTime: DateTime option; 
    LastPingId: int option; 
    SessionId: string option; 
    WhenInitiated: DateTime option; 
    WhenDisconnected: DateTime option; 
} 

(**

' An Enum representing states, mixed with a single type that contains a mixture of legal and illegal data

---
### DDD und F#
#### More states

*)

type Connecting = { WhenInitiated: DateTime; }

type Connected = { LastPing : DateTime * int; SessionId: string; }

type Disconnected = { WhenDisconnected: DateTime; }

type ConnectionInfo' =
    | Connecting of Connecting
    | Connected of Connected
    | Disconnected of Disconnected

(**

--- 
### DDD und F#
#### Own Employee OR employee of a customer
*)

type Department = { Name:string; Description:string; }

type Customer = { Id:int; Name:string; }

type Person = { Id:int; 
                Name:string; 
                Surname:string; 
                Department: Department option; 
                Customer: Customer option; }

(**
' Theoretisch, hindert mich nichts daran sowohl Department und Customer leer zu lassen. 
' Construktoren und Scope sind die Optionen die ich habe.  Aber es ist nicht erkennbar
' Ich muss aktiv über mein Code eingreifen um zu sichern dass mindestens eines der beiden belegt ist
' Nachnamen und Vornamen sind häufig gemeinsam anzutreffen, die Beiden werden meistens als einheit betrachtet
' Notfalls muss eine ReadOnly Property erstellen um herauszufinden dass eine Person intern oder extern ist
' Diese Property muss immer z.B. aufgerufen werden um sich zu vergewissern 

--- 
### DDD und F#
#### Composition

- Composition 
- DUs create closed sets

*)

type PersonenName = { Titel: string option; Vorname:string; Nachname:string; }

type Employee = { Id:int; Name:PersonenName; Department: Department;}

type EmployeeOfCustomer = { Id:int; Name:PersonenName; Customer:Customer;}

type Person' = 
    | Angestellter of Employee
    | EmployeeOfCustomer of EmployeeOfCustomer

(**

' - ACHTUNG: string kann nicht mit nullable kombiniert werden, da diese nur Werttypen unterstützt
' - Ein ORM hätte an der Stelle den Wert null gewählt als mapping
' - string option hingegen erlaubt den Verzicht auf null, bei Einhaltung der 
' Bedenken: Ich habe weiterhin nur einen Typ für Person, keine zwei.  Nur bei jedem Zugriff auf Person, muss ich jetzt beide Fälle explizit berücksichtigen
' Operationen die nur vom Angestellten/Externen ausgeführt werden müssen, können jetzt auf Typ Ebene beschränkt werden
' Datenhaltung: Command Query Seperation, dass eine Tabelle sich 1:1 zu einem Objekt mappen soll ist problematisch. Also besser ist es, wenn ich mich damit abfinde, dass eine Tabelle unter Umständen auf mehreren Wegen abgefragt/geändert werden kann
' Zugriffe können weniger als eine Tabelle holen
' Updates nur bestimmte Werte updaten
' http://gorodinski.com/blog/2013/01/21/inverting-object-orientation-with-fsharp-discriminated-unions-and-pattern-matching/

---
### DDD und F#
#### Models for email, phone, customer number, etc.  

' Use single case union to reserve a type for emails
' Functions and data structures that require email can now state this in a declarative manner
' It's possible to *hide* type constructors behind a module to insure compliance
' Emails are *NOT* strings!

*)

// single case union
type Email = Email of string

let emailConstructor s = 
    if System.Text.RegularExpressions.Regex.IsMatch(s,@"^\S+@\S+\.\S+$") 
        then Some (Email s)
        else None

let v = emailConstructor "n@b.de"
let v' = emailConstructor "vvv"

(** <div style="display: none" > *)
(*** define-output:SCU-Email ***)
printf "n@b.de = %A | vvv = %A" v v'
(** </div> *)

(*** include-output:SCU-Email ***)

(**

--- 
### DDD und F#
#### States
* Type system helps
    * bring related data in structures
    * represent states

*)

module DDD0 = 
    open System

    type PersonalName = { Title: string option; Surname:string; Name:string; }

    type Contact = { Name:PersonalName; Email:Email; EmailVerified:bool; }

(**

' Kontakt hat eine Email Adresse angegeben, diese muss verifiziert werden.  Erst danach kann der Kontakt bestimmte Tätigkeiten ausüben
' Beachte: EmailVerifiziert kann gesetzt werden, ODER ich muss in meinem Code dies kontrollieren

--- 
### DDD und F#
#### States

*)

    type EmailVerificationToken = EmailVerificationToken of string

    type VerifyEmail = Email -> EmailVerificationToken -> bool

    let ``Funktion gilt nur für verifizierte`` kontakt = 
        if kontakt.EmailVerified then
            "???"
        else
            "Contact has been verified"


(**

' Zeigen. Type Inference funktioniert wirklich: Kontakt wird erkannt, kein Rückgabetyp da dieser auch vom Compiler erkannt wurde (kontakt:Kontakt -> string)
' VerifyEmail gibt mir einen bool zurück, ich muss jetzt darauf handeln und die Behandlung der Verifizierung (das Bool) übernehmen, neben der tatsächlichen email Adresse
' Solches code muss jetzt immer verwendet werden um zu prüfen ob die Email eines Kontakts verifiziert ist

---
### DDD und F#
#### Model für eine verifizierte Email

' Wenn ein neuer Kunde eine Email eingibt, dann muss diese oft erst verifiziert werden
' Bis zu dieser Verifizierung handelt es sich um eine nicht verifizierte Email
' Erst nach der Verifizierung wird daraus eine verfizierte Email (der Beweis ist ein Token)
' Die beiden Zustände einer Email als DU modellieren

*)
    type VerifiedEmail = { Email:Email; Verifikation: EmailVerificationToken; }

    type VerifyEmail' = Email -> EmailVerificationToken -> VerifiedEmail option

(**

' Der neue Typ soll nur dann erstellt werden wenn die Verifikation erfolgt ist.  
' Der neue Typ soll nur dann erstellt werden wenn die Verifikation erfolgt ist.  Die Funktion VerifyEmail' könnte ich z.B. mittels F# Scope Regeln so erstellen dass diese als einziges Gateway vorhanden ist um an das Typ VerifizierteEmail zu erzeugen

--- 
### DDD und F#
#### Model für einen verifizierten Kunden

*)

    type UnverifiedContact = { Name: PersonalName; Email:Email; }

    type VerifiedContact = { Name: PersonalName; Email:VerifiedEmail; }

    type Kontakt' = 
        | UnverifiedContact of UnverifiedContact
        | VerifiedContact of VerifiedContact

    let ``Funktion only applies to verified contacts`` verifiedContact = 
        // I can access Email without further ado
        verifiedContact.Email.Email

(**

---
### DDD und F#
#### Model for a customer with an adress


' Das Pattern matching schränkt die Auswahl auf konkrete Typen und nicht mehr auf das Abfragen von einzelnen Werten
' In OO wäre hier jetzt eine Implementierung des Visitor Pattern notwendig
' http://www.dofactory.com/net/visitor-design-pattern
' Je mehr Fälle man hat, desto komplizierter wird das Ganze

*)

    type Strasse = Strasse of string
    type PLZ = PLZ of string
    type Land = Land of string

    type PostalAdress = { Strasse:Strasse; PLZ:PLZ; Land:Land; }

    type VerifiedAdress = 
        | VerifizierteEmail of VerifiedEmail
        | PostAnschrift of PostalAdress

    type Kontakt'' = 
        | UnverifizierterKontakt of UnverifiedContact
        | VerifizierterKontakt of VerifiedAdress

(**

' Das gleiche Spiel mit der Verifikation kann ich jetzt auch mit der postalen Anschrift machen

***
### Functions

F# functions resemble mathematical functions

* *Map* values between a *domain* and a *range*
* Applied to a value (domain) producing a result (range)
* Have no side effects -> reducing "incidental complexity"

' Functions are really lookups
' https://youtu.be/iSmkqocn0oQ?t=9s What are side effects?
' Vieles hier habe ich der Diskussion auf http://fsharpforfunandprofit.com/posts/defining-functions/ entnommen
' 

---
### Functions as values 

* Declared using keyword let
* They are independent of their names
* Don't try to compare them though!
* camelCase naming is enforced

*)

let add1 x = 
    let sum = x + 1
    sum

// I can assign a function to a value
let plus1 = add1 

(**

' http://stackoverflow.com/questions/8225433/checking-function-equality-in-a-f-unit-test
' let declares the function (it's just a value)
' name of the function is in camelCase
' Function parameters (input) are listed after the name of the function
' Body of the function is indented (non tab, 4 whitespace)
' Last item is the return value, no return is needed

---
### Functions as values

And they have signatures

val add1 : x:int -> int

' First the parameters are listed, seperated by ->
' Last element is the return type
' Signatures allow us to make educated gusses about a function's nature
' Siehe Module Function Signature

---
### Functions are lambdas

*)

let add = 
    let f = fun x y -> x + y
    f
let add' x y = x + y

(**

' Both implementations have the same signature.  Both boil down to the same implementation
' Again, Body is indented
' Last value is the return value, no return keyword here

---
### Function Annotation

Type annotation is only needed when type inference has failed

' Type inference (Hindley-Milner Type Inference) is an algorithm that computes the "most general" type for a given expression
' http://stackoverflow.com/questions/12532552/what-part-of-milner-hindley-do-you-not-understand

*)

let f' x = x + 1
let f'' x = x + 1.0
let f''' x = x + "More"

(**

---
### Functions as parameters

*)

(** <div style="display: none"> *)
module FunAsParams = 
(** </div> *)
    let evalWith5AndAdd2 f = f 5 + 2
    let add2 x = x + 2
    let eval = evalWith5AndAdd2 add2

    let evalWith5AndAdd2' f x = f x + 2
    let eval' = evalWith5AndAdd2' add2 5

    let add5 x = x + 5
    let eval'' = evalWith5AndAdd2' add5 5

(** <div style="display: none" > *)
(*** define-output:Fun-Param ***)
    printf "eval = %A | eval' = %A | eval'' = %A" eval eval' eval''
(** </div> *)

(*** include-output:Fun-Param ***)

(** 

' You can use the sigantures to define a type that you can use

---
### Functions as output

*)

(** <div style="display: none"> *)
open System
(** </div> *)


let adderGenerator x = fun y -> x + y
let add3 = adderGenerator 3
let add4 = adderGenerator 4

let eval = add3 5
let eval' = add4 5

(** <div style="display: none" > *)
(*** define-output:Fun-Output ***)
printf "eval = %A | eval' = %A" eval eval' 
(** </div> *)
(*** include-output:Fun-Output ***)
    
(** 

' Using the lambda in adderGenerator shows that an intermediate functino is returned
' It's also possible to write let adderGenerator = fun x -> (fun y -> x + y)
' When this function is called with a single value, the second part is returned (fun y -> x + y) with x being fixed

---
### Functions and annotation

It's still possible to add annotations

*)

let apply f x :int = f x // type of result is defined
let applied = apply add3 5

(** <div style="display: none" > *)
(*** define-output:Fun-Annotation ***)
printf "eval = %A | eval' = %A" eval eval' 
(** </div> *)

(*** include-output:Fun-Annotation ***)

(**
' In this case the signature is val apply : f:('a -> int) -> x:'a -> int and the function is generic!

---
### Function with multiple parameters?

* Mathematical functions have a single input and single output
* Programmers want functions that can take multiple inputs
* F# achieves this through applying "currying"

---
### The other Curry

![From https://wiki.haskell.org](images/HaskellBCurry.jpg)
<div class="align-center">Haskell Brooks Curry (Image: https://wiki.haskell.org)</div>

---
### Functions without parameters

*)

let printToConsole = printf "Hello World"
let printToConsole' = fun () -> printf "Hello World"

(**

' Achtung: The first expression is evaluated immediately, it's of type unit (F#'s notion of void, but is a real type!)
' The second expression returns an actual function that I can call in the repl

---
### Partial application

* Apply some of the paramters
* Returns a function with applied paramters baked in

*)

(** <div style="display: none"> *)
module PartialApplication = 
(** </div> *)
    let add x y = x + y // val add : int -> int -> int
    let add1 = add 1    // val add1 : int -> int
    let add2 = add 2
    let add3 = add 3

(**
 
---
### Partial application

A function of *n* arguments = <br/>
A function of *1* argument that returns a function of *n-1* arguments   

---
### Partial application

<div class="align-left">
F# design rule:<br/> the most varying value is the last parameter <br/>
val filter : ('a -> bool) -> 'a list -> 'a list <br/>
<div/>



Bake the filter in, apply the function to many lists

---
### Partial application

Another example

*)

    let shift (dx, dy) (px, py) = (px + dx, py + dy)
    let shiftRight = shift (1, 0)
    let shiftUp = shift (0, 1)
    let shiftLeft = shift (-1, 0)
    let shiftDown = shift (0, -1) 

(**

' Example from Expert F# 4.0

---
### Functions and pipes

* Typical scenario: Transform a value in steps
* Output of first transformation is input to the second
* List of ints -> addOne -> filter by even -> sum

*)

let isEven x = x % 2 = 0
let addFilterSum l = List.sum(List.filter isEven (List.map add1 l))

(**

' reading inside out

---
### Functions and pipes

* Pipes help make code readable

*)

let addFilterSum' l = 
    l
    |> List.map add1        // Partial application of List.map
    |> List.filter isEven   // Pipe operator provides the last paramter
    |> List.sum

// Original implementation
// List.sum(List.filter isEven (List.map add1 l))

(**

' And you can enjoy looking at the implementation too! let inline (|>) x f = f x
' Notice that this is not a fluent interface: method instances are not needed. No Design decisions needed.
' http://alexey.raga.name/posts/2015/08/04/fluent-interfaces/ provides a C# take on the concept
' You chain results and inputs with appropriate functions

---
### Functions and composition

* Compose simple functions to complex ones
* Partial application means: calling with n-1 parameters yields a function
* F# has an operator for this

*)

let (>>) f g x = g (f (x) )
let times n x = n * x
let multiply2 = times 2
let add1Multiply2 = add1 >> multiply2

(**

---
### Functions and composition

* Achtung: Composition operator has lower precedence

*)

let add4Multiply4 = add 4 >> times 4

(**

' add 4/times 4 is evaluated prior to the >> operator
' You can pipe values, you cannot compose them using >>

---
### HOF and interfaces

* Strategy Pattern rethought

*)

module ``Strategy`` = 

    let minus x y = 
        x - y
    let plus x y = 
        x + y

    let calculateClient' f x y = 
        f x y

    let calculateClient (f:int -> int -> int) x y = 
        f x y

    let z = calculateClient minus 10 9
    let z' = calculateClient plus 10 9


(**
 
' https://en.wikipedia.org/wiki/Strategy_pattern#C.23  
' No need to define an interface since HOF is its own signature
' As long as the signature fits, I can call the function
' No need to define a class/client, a function would do
' I can call the client with different functions, no need for an immutable backing field
' actually you don't need the annotation
    
*)


    



















