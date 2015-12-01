﻿(**
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

Eine Menge von Elementen 

- Müssen nicht vom gleichen Typ sein
- Die Reihenfolge ist entscheidend
- Ist ein ad-hoc Typ: keine benannter

' Auch wenn ich einen Typ definieren kann, kann ich einzelne Werte diesen Typen nicht zuordnen.  Die einzige Zuordnung ist den Bestandteilen und deren Reihenfolge

---
### Tuple: Construction

- Kann beliebig viele Elemente beinhalten                    
- Construction erolgt mittels Komma (mit und ohne Klammern)  
- Multiplikation von (mindestens) zwei Domains: Kartesiche Summe 

*)


type IntTuple = int * int // Jeder int 'mal' jeder int
let intTuple  = 1,1

type IntStringTuple = int * string // Jeder int 'mal' jeder string
let intStringTuple  = 1,"string"

type TripleIntTuple = int * int * int
let tripleIntTuple = 1,2,3

(**

---
### Tuple: Komposition

Aus Primitives/einfachen Typen lassen sich komplexere erstellen

*)

type Complex = float * float
let complex  = 1.0,1.0

type Komposition = IntStringTuple * Complex
let komposition  = intStringTuple,complex

(**

---
### Tuple: Deconstruction
Ein Tuple in seinen Bestandteilen zerlegen

' In F# ist es üblich Varianten von einem Wert mittels '' zu kennzeichnen
' Klammern sind nicht notwendig, dienen der Abgrenzung und Klarheit

*)
let complex' = 1.0,2.0
let c',c'' = complex'
let komposition' = intStringTuple,complex'
let k',k'' = komposition'

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
### Tuple: Strukturelle Gleichheit

- Typ und Reihnfolge: sind Zwei Werte vergelichbar?
- Werte: sind zwei Werte gleich?

*)
let equal = complex' = complex

(** <div style="display: none" > *)
(*** define-output:Type-Equality ***)
printf "Gleich = %b" equal
(** </div> *)

(*** include-output: Type-Equality ***)

(**
' Typ Definition ist hilfreich bei Signaturen

---
### Tuple: Pattern Matching

*)
let matchTuple c = 
    match c with
    | 0.0,0.0 -> "0.0,0.0"
    | 1.0,1.0 -> "0.0,0.0"
    | 1.0,2.0 -> "1.0,1.0"
    | _,_ -> "sonst"

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

' Fehlnde Fälle werden vom Compiler festgestellt und angezeigt

---
### Tuple: Nutzung in der .net API

TryParse Methoden die zwei Werte zurückgeben

- bool: War das Parsen erfolgreich?
- Wert: falls das Parsen erfolgreich war

' _ ist ein Platzhalter für egal welches Wert erscheint, bitte nicht evaluieren

*)

open System
let showParseResult result = 
    match result with
    | true,value -> sprintf "Value parsed is %s" (value.ToString())
    | false,_ -> "Value couldn't be parsed" 

let tryParseResult = Int32.TryParse "Keine Zahl" |> showParseResult
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

- Müssen nicht vom gleichen Typ sein
- Die Reihenfolge der Deklaration ist nicht relevant
- Ist kein ad-hoc Typ

' Wichitg: es ist auch nur ein Tuple, ein multiplication type

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
' Dies ist dann besonders hilfreich wenn Typen die gleichen Bezeichner verwenden

---
### Record: Construction
Keine halben Sachen: 

- Alle Werte müssen angegeben werden
- Kein Wert kann verändert werden nach der Construction

*)
// let complexNumber' = { ComplexNumber.Real = 1.0; } // Imaginary gebe ich später an
(**

' Funktionales Vorgehen: Wenn ich feststelle dass ich den Fall konkret habe dass ein Wert nicht immer angegeben werden kann, dann erstelle ich hierfür einen Typ

---
### Record: Deconstruction
Record in seinen Bestandteilen zerlegen

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
Da modifizieren nicht geht, bietet F# verinfachtes Klonen an

*)

let complexNumber'' = { complexNumber' with Imaginary = 3.0 }

(** <div style="display: none" > *)
(*** define-output:Record-Clone ***)
let { Real = real'''; Imaginary = imaginary''; } = complexNumber''
printf "real = %f | imaginary = %f" real''' imaginary'' 
(** </div> *)

(*** include-output: Record-Clone ***)

(**

'  Dies ist notwendig für den üblichen Fall dass sich nicht alle Werte geändert haben sondern nur bestimmte.  Mit der With Syntax kann ich diesen Fall gut abdecken

---
### Record: Strukturelle Gleichheit

Zwei Record Werte sind gleich wenn beide 

- Vom Gleichen Typ sind
- Alle korrespondierende Bezeichner haben die gleichen Werte

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
printf "r'' = %s |" r''
printf "r''' = %s " r'''
(** </div> *)
(*** include-output: Record-PatternMatching ***)
(*** include-output: Record-PatternMatching0 ***)

(**

***
### Discriminated Union

- Unterscheidungs-Union auf deutsch
- Besteht aus einer Anzahl von benannten Fällen
- Ein benannter Fall kann aus einer Anzahl von Werten bestehen
- Nur eines der bennanten Fällt ist gültig für einen Ausdruck
- Mit Bezeichnern oder ohne

*)

type Shape =
| Viereck of width : float * length : float
| Kreis of radius : float

(**
' Ausdrucke sind immutable, ändert sich der Wert nicht.  Bitte nicht mit einem Variant (vb.net) verwechseln
' DU ist das ganze.  Die einzelnen möglichen Werte heißen Union Case
' Single Case gibt es auch und sind sehr schön im DDD


---
### DU: Deklaration

- Empty Case, ist nur ein Bezeichner, keine Daten. 
- Komposition: Record definieren und als union case verwenden

' Empty case kann sehr hilfreich sein, z.B. bei DDD

*)

type DuBeispiel =
| Leer
| Complex of ComplexNumber
| Coordinate of GeoCoord

(**

---
### DU: Construction

Für jeden Union Case gibt es eine Constructor Funktion

*)

let viereck = Viereck(length = 1.3, width = 10.0) // ACHTUNG, Welchen Typ hat rect?
let kreis = Kreis (1.0)

(**

---
### DU: Deconstruction & Pattern Matching

- rect im Beispiel hatte den Type Shape, nicht Shape.Rectangle!
- Ich kann von außen nicht wissen welche union case ein DU-Wert darstellt
- Nur Pattern Matching erlaubt es mir dies zu erfahren
- Deconstruction *muss* für *alle* Fälle erfolgen (Exhaustivness)

' Wenn ich einen DU anspreche dann entweder um eine Transforation zu erhalten (Fläche Ermitteln) oder um 

---
### DU: Deconstruction & Pattern Matching

*)

let flaeche s = 
    match s with 
    | Viereck (w,l) -> w*l
    | Kreis(r) -> Math.PI*(r ** 2.0) 
    // einen union case auszulassen verursacht einen Compiler Fehler 

let kreisFlaeche   = flaeche (Kreis (5.0))
let viereckFlaeche = Viereck(length = 5.0, width = 5.0) |> flaeche


(** <div style="display: none" > *)
(*** define-output:DU-PatternMatching ***)
printf "Kreisfläche = %f | " kreisFlaeche
printf "Viereckfläche = %f" viereckFlaeche
(** </div> *)
(*** include-output: DU-PatternMatching ***)

(**

--- 
### DU: Nutzung

- DU für die Darstellung von Zuständen/Übergängen
- Alle Fälle abgedecken führt zu wengier Fehlerfälle
- Schnell erstellt!

---
### DU: Single Case

- Primitives haben oft besondere Bedeutung
- Höhengrad und Breitengrad sind beide floats
- Beide werte jedoch stellen einen anderen Domain dar

' In DDD (Domain Driven Design) spielen diese oft eine wichtige Rolle.
' Z.B. kann ich dadurch Primitives so definieren dass diese untereinander nicht „kompatibel“ sind, auch wenn diese vom gleichen Typ sind.

*)

// Erste Longitude ist der Name des Typs
// Zweite ist der Name des Constructors
// Müssen nicht identisch sein!
type Longitude = Longitude of float 
type Latitude = Latitude of float

let longitude = Longitude(9.993009567260742)
let latitude  = Latitude(53.553260805869805)

// Der Compiler mag das nicht, es handelt sich um zwei Typen
// let gleich = longitude = latitude 

(**

***
### Option

- Ist eine besondere Form des DU
- Zu finden in vielen funktionalen Sprachen

' In Haskel heißt dieser Typ Maybe, Just, Nothing. in Scala heißt es auch option, some, none.

*)

type Option<'a> = // DU mit generischem Parameter
| Some of 'a // Gültiger Wert
| None // ???


(**

---
### Option: Construction

*)

let s = Some "string"

(**

- Achtung: None wird nicht via Construction erstellt, sondern nur als Rückgabe Wert verwendet

---
### Option: Deconstruction und Pattern Matching

- Alle Fälle müssen behandelt werden damit ich den Wert des Option extrahieren kann

*)

let optionMatch s = 
    match s with 
    | Some wert -> sprintf "Wert ist %s" wert
    | None -> ""

let z  = optionMatch (Some("Hello"))
// Das nicht idiomatisch, nur für Demonstrationszwecke
let z' = optionMatch Option<string>.None

(**

---
### Option: Deconstruction und Pattern Matching
- Option ist meistens das Ergebnis einer Auswertung
- None stellt den Fall dar dass kein sinnvolles Ergebnis vorhanden ist

*)

let matchForOption s = 
    match s with 
    | "Jawohl" -> Some(s)
    | _ -> None

let x  = matchForOption "Jawohl"
let x' = matchForOption "Doch nicht"

(** <div style="display: none" > *)
(*** define-output:Option-Deconstruction ***)
printf "x = %A | x' = %A" x x'
(** </div> *)

(*** include-output: Option-Deconstruction ***)


(**

---
### Option: Nutzung

- Null vs Unbekannt/Fehlend
- Durch Option ist es möglich fehlende Werte explizit zu kennzeichnen
- Die Kennzeichnung erfolgt durch einen eignen Typ
- Zugriffe auf fehlende Werte sind dann compile-time und keine run-time Fehler

' Referenzen auf nicht vorhandene Objekte sind nicht die beste Artundweise 


---

### Option vs null: Type Safety

- null ist eine Reference zu einem Objekt das nicht existiert
- Das Typsystem kann uns dabei nicht helfen zu erkennen dass die Variable diesen Wert hat
- Ich kann .Length auf einen Reference null aufrufen

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
            // WIR wissen dass s2 null ist, der Compiler nicht
        }
    }

*)

(**

---
### Option vs null: Type Safety

- Das gleiche in F# verursacht einen Compile Fehler

*)

let none = Option<string>.None
// let length = none.Length // None hat keinen Length, es ist nämlich kein String!

(**

---
### Option vs nullable
- Nullable gilt nur für Werttypen, keine Referenztypen
- Option verfügt in F# über viele Hilfsfunktionen 

***
### DDD und FP
#### Was bis her geschah

- Design Prozesse gehen von einer Dreiteilung
    - Fachleute mit Fachwissen
    - Modellierer erstellen Design Dokumente in einem Zwischenformat (z.B. UML)
    - Programmierer erstellen Code anhand der Design Dokumente

---
### DDD und FP
#### Was bis her geschah

- Konsequenzen
    - Programmierer reden nicht mit Fachleute
    - Roundtrip Engineering notwendig um Code und Design Dokumente auf einem Stand zu halten
    - Code kann nicht so dargestellt werden dass Fachleute es einsehen können
    
' Ohne dass sie kotzen

    
---
### DDD und FP
#### Die Hoffnung

- Design Dokumente die verifizierbar sind
    - Verifizierbar durch einsehen
    - Verifizierbar durch eine Maschine (Compiler)

>"A good static type system is like having compile-time unit tests" (S. Wlaschin)

---
### DDD und FP
#### Die Hoffnung

- Code als Design Dokument
    - Das Code ist das Model: Keine Zwischenformate
    - Datenstrukturen und Verhalten (zum Teil) durch Datenstrukturen darstellen
    - Einbetten von Domainlogik in den Datenstrukturen

>"Making illegal states unrepresentable" (Y. Minsky)

' - Die Fachleute können ihre Dokumente schreiben in den Formaten mit denen sie vertraut sind, die DEVs können Code schreiben
' - In FP sind sowohl Datenstrukturen (Tuple, Record, DU) als Verhalten (Function) Types: Composition.
' - Domainlogik legt Regeln fest die Definition (Vorname required) und Transformation Einkaufskorb -> Bestellung festlegen. Types erlauben diese Regeln zum Teil darzustellen, der Rest muss dann mit Controlflow Construkte erstellt werden
' - MISU!!! Vielleicht hier einen Diagram malen um das Vorgehen in OO und in FP darzustellen: Offener Raum, nachträgliches Einschränken.  Geschlossener Raum, Quadrat für Quadrat gebaut, nur legale Zustände werden dargestellt

---
### DDD und F#
#### Vorteile des F# Typ System

- Typ System begünstigt Komposition<!-- .element: class="fragment grow" -->
- DU erlauben eine kompakte Darstellung von Zuständen

' - In C# oder in Java, ist die Hemmschwelle relativ hoch neue Types zu erstellen. Es gibt sogar das code smell "Primitive Obsession", kein Scherz
' - Komposition erlaubt es einfache Typen zu immer komplexeren zusammenzufassen.  Es ist erstaunlich wie viel dann auf einer Seite passt
' - Macht Nicht Programmierern weniger Angst
' - Bei Programmierung von Fachanwendungen, geht es oft darum dass ein Objekt mehrere Zustände haben kann.  Jedes Zustand hat wiederum eigne Operationen, Fähigkeiten. DU erlauben diese sehr gut darzustellen

---
### DDD und F#
#### Vorteile des F# Typ System

- Light-weight Typen: geringe Anzahl der Zeilen, Keine Sonderzeichen
- Exhaustivness führt zur Korrektheit: Fehlende Fälle werden vom Compiler erfasst


' - Die geringe Zahl von Sonderzeichen/Schlüsselwörtern macht Nicht Programmierern weniger Angst
' - Bei Programmierung von Fachanwendungen, geht es oft darum dass ein Objekt mehrere Zustände haben kann.  Jedes Zustand hat wiederum eigne Operationen, Fähigkeiten. DU erlauben diese sehr gut darzustellen

--- 
### DDD und F#

- Beispiele in den kommenden Seiten stammen aus 
    - fsharpforfunandprofit
    - Jane Street (kein F#, OCaml)

--- 
### DDD und F#
#### Intern ODER extern
*)

type Abteilung = { Name:string; Beschreibung:string; }
type Kunde = { Id:int; Name:string; }

type Person' = { Id:int; Vorname:string; Nachname:string; Abteilung: Abteilung option; Kunde: Kunde option; }

(**
' Theoretisch, hindert mich nichts daran sowohl Department und Customer leer zu lassen.  Ich muss aktiv über mein Code eingreifen um zu sichern dass mindestens eines der beiden belegt ist
' Nachnamen und Vornamen sind häufig gemeinsam anzutreffen, die Beiden werden meistens als einheit betrachtet
' Notfalls muss eine ReadOnly Property erstellen um herauszufinden dass eine Person intern oder extern ist
' Diese Property muss immer z.B. aufgerufen werden um sich zu vergewissern 

--- 
### DDD und F#
#### Komposition

- Komposition 
- DUs erlauben eine "geschlossene" Auswahl

*)

type PersonenName = { Vorname:string; Nachname:string; }
//type Person = { Id:int; Name:PersonalName }

type Person = 
    | Angestellter of Id:int * Name:PersonenName * Department: Abteilung
    | Externer of Id:int * Name:PersonenName * Customer:Kunde

(**

' Wenn ich ab jetzt nur noch mit 

---
### DDD und F#
- Model für einen Namen

' In OO wäre es möglich dass MiddleName ein null wäre. Das ist zulässig und idiomatisch.
' Aber wie wir gesehen haben, wäre es auch möglich Length gegen einen null pointer vom Type String zu verwenden
' dies kann ich verhindern in dem ich die Option nutze, hier muss der String angegeben werden (wenn auch Empty auch möglich wäre)

*)

module DDD0 = 

    type PersonenName = 
        { 
            FirstName:string; // Muss
            MiddleName:string; // Kann
            LastName:string; // Muss
        }

(**

---
### DDD und F#
#### Model für einen Namen

*)

module DDD1 = 

    type PersonalName = 
        { 
            FirstName:string; // Muss
            MiddleName:string option; // Kann
            LastName:string; // Muss
        }

(**

---
### DDD und F#
#### Model für eine Email/Telefonenummer/Kundennummer etc.

' Single Case union bedeutet dass ich einen eignen Type für Emails reserviere.  Funktionen/Datenstrukture die eine Email brauchen, können dies jetzt mittels der Typdeklaration kundtun. Ich trenne so "normale" von Emails.  

*)
    // single case union
    type Email = Email of string
    type PhoneNumber = PhoneNumber of string
    type CustomerNumber = CustomerNumber of string

(**

--- 
### DDD und F#
#### Kontakt

- Zusammenhängende Informationen als Primitives dargestellt
- Daten die fehlen darf als option dargestellt

*)

    type Contact = {
        FirstName:string
        MiddleInitial:string
        LastName:string
        EmailAdresse:string
        IsEmailVerified:bool
        EmailVerificationToken:string }

    type Contact' = {
        Name: PersonalName
        Email:Email
        IsEmailVerified:bool
        EmailVerificationToken:string
    }

(**

---
### DDD und F#
#### Model für eine verifizierte Email

- Wenn ein neuer Kunde eine Email eingibt, dann muss diese oft erst verifiziert werden
- Bis zu dieser Verifizierung, handelt es sich um eine nicht verifizierte Email
- Erst nach der Verifizierung, wird daraus eine verfizierte Email (der Beweis ist ein Token)
- Die beiden Zustände einer Email als DU modelieren

*)
    type EmailVerificationToken = EmailVerificationToken of string

    type EmailContactInfo = 
        | Unverified of Email
        | Verified of Email:Email * VerificationToken : EmailVerificationToken

(**

--- 
### DDD und F#
#### Model für einen verifizierten Kunden

*)

    type Contact'' = {
        Name: PersonalName
        EmailContactInfo:EmailContactInfo
    }






