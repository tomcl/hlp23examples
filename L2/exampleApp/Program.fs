//-----------------------------------------------------//
//         F# Hindley Milner type system               //
//-----------------------------------------------------//

// Atomic types: unit,int,char,bool,string

// Unit
()
(    )

// string
"a" + "b"

"abc"[2]

"abc"[0..1]

"abc" |> List.ofSeq // converts string to list of char - reason this works is NOT EXAMINABLE

"abc" |> Seq.map (fun c -> printfn $"Char = {c}") // Seq.map is not examinable

// convert list of chars to string
['a';'b';'c'] |>  List.map System.Char.ToString |> String.concat ""

// how to convert char to ASCII code?
int '0'
$"hex value = 0x%x{int '0'}"
// how to convert ascii code to char?
char 0x31
typeof<char> = typeof<System.Char> // System.Char is a module as well as a type!

// bool
true
false
1 = 1


//int
1
// 64 bit int
10L

//1 + 1
//1 +1
//1 + 1L
//1. + 2
//3 + -2
//3 -2

//-------------------------------------------------------------------//
// ----------------Type aliasses and notation------------------------//
//-------------------------------------------------------------------//


type T1 = int // T1 is a type abbreviation or alias
type T2 = int
typeof<T1> = typeof<T2>

// type constructors: 
// List<'T> ; 
// 'T1 -> 'T2; **function**
// T1 * T2 (tuple)
// polymorphic type variable 'a or 'T etc
// also records, discriminated unions (WS2,3)


[1]
["abc";"cde"]
[[]]
[[[3]]]

type T3 = int list  // ML-style notation
type T4 = List<int> // .NET-style notation

typeof<T3> = typeof<T4>

type T5<'a> = List<'a->'a> // .Net-style notation
type T6<'T> = ('T -> 'T) list // ML-style notation

typeof<T5<'x>> = typeof<T6<'y>>
typeof<T5<'T>> = typeof<List<'a -> 'a>>
typeof<unit> = typeof<Unit>
typeof<int32> = typeof<int>
typeof<uint32>= typeof<int32>


[ "abc"]

// Tuples

1,2
1,"hello"
(1,3) // , => tuple. Brackets used only for grouping (but often needed!)
("yes", "no"), ()
"yes", ("no",())
("yes","no",())  // 3-tuple is not the same as 2-tuple of 2-tuple

// tuples are like records without field names.

// fst selcts component of 2-tuple
fst (1,2)

// function to select component of 3-tuple
(fun (a,_,_) -> a)("yes",1,()) // also see later


//------------------------------------------------------------------------------//
//---------------------------functions------------------------------------------//
//------------------------------------------------------------------------------//
fun n -> n + 1

let thunk() = printfn "Hello!" // use unit is parameter type if "no parameters"
thunk() // this looks like C++ but is actually different!

// () is a value - the ONLY value in the Unit type (unit is an alias for Unit).
let anotherThunk() =
    let arg = ()
    thunk arg

anotherThunk()

// function definition
let addOne n = n + 1

// equivalent function definition
let addOne' = fun n -> n + 1

addOne 10 |> printfn "%d"

// brackets can be used anywhere - for grouping - they are not needed here
addOne (10) |> printfn "%d"

// this works because () is the ONLY value of type unit
fun () -> 10

fun n -> printfn "%A" n
fun n -> $"{n}"

let addTuple(a,b) = a + b // you should NOT normally use this form of function defn
addTuple(1,2)

let addCurried a b = a + b // normal (curried) definition
addCurried 1 2

let addThree = addCurried 3
addThree 10

let test() =
    let x = 10
    let y = x
    let x = 20
    let y = y + 1
    printfn $"x={x}; y={y}"

test()

// why do all F# functions have a single argument?

// evaluation order



let addP a b = printfn $"adding %d{a}, %d{b} = {a+b}" ; a + b
let squareP a = printfn $"squaring %d{a} = {a*a}" ; a * a 

squareP (addP (addP 1 2) (addP 3 4))




let unitFun = fun (x:Unit) -> ()
let id' x = x // user defined version of standard function 'id'
id' 1
id' "abc"

let throw x = ()
let throw1 = fun x -> ()
let throw2 = fun _ -> ()
ignore

let throwTuple (_,_) = ()


let fail() = failwithf "error"
let returnTuple (x,y) = x,y
returnTuple (1,2)
fail()

1 + 1 |> throw

let stringToInt (x:string) = String.length x

//-----------------------------------------------------------------------//
// ------------------------higher order functions------------------------//
//-----------------------------------------------------------------------//

let twice f x = f (f x)
let twice' f = fun x -> f (f x)

twice 10

twice (fun x -> x + 10) 11

twice twice 1
let p = twice twice (fun n -> n+1)
let q = twice twice twice (fun n -> n+1)
let q' = twice twice twice ((*) 2) // (*) 2 alt notation for fun n -> n*2

p 0
p 1
q 0
q 1
q' 0
q' 1

let S g h x = (g x) (h x) // this is actually the S combinator (non-examinable)

// How are types inferred?
let unifyExample a b =
    ()
    //a b |> ignore
    //b + 1.0 |> ignore
    //a b + "xyz"


//-----------------------------------------------------//
//     recursion vs tail recursion                     //
//-----------------------------------------------------//

/// recursive test function
let rec testRec n =
    if n = 0 then
        0
    else
        1 + testRec (n-1)

/// recursive test function - impl with match
let rec testRec' n =
    match n with
    | 0 -> 0
    | n -> 1 + testRec (n-1)

/// recursive test function - impl with function
let rec testRec'' = function
    | 0 -> 0
    | n -> 1 + testRec (n-1)


//testRec' = testRec''

testRec 1000
testRec 1000000

/// tail-recursive test function (64 bit)
let rec testRecTailInner res = function
    | 0L -> res
    | n -> testRecTailInner (res + 1L) (n-1L)


let testRecTail n = testRecTailInner 0 n
    
testRecTail 1000L
testRecTail 1000000000L

let pAdd a b = printfn $"Adding: {a} + {b} = {a+b}"; a+b

/// recursive test function - impl with function
let rec testRecP n =
    printfn $"Calling testRecTail with n={n}"
    match n with
    | 0 -> 0
    | n -> pAdd 1 (testRecP (n-1))
    |> (fun x -> printfn $"Return {x}"; x)

/// tail-recursive test function (64 bit)
let rec testRecTailP res n =
    printfn $"Calling testRecTail with n={n}"
    match n with
    | 0 -> res
    | n -> testRecTailP (pAdd res 1) (n-1)
    |> (fun x -> printfn $"Return {x}"; x)


//-----------------------------------------------------//
//           Recursion vs List functions               //
//-----------------------------------------------------//


let els = ["a"; "b"; "c"; "d"]

let reflect x = 
    List.findIndex ((=) x) els
    |> (fun index -> els[els.Length - index - 1])

let reflect' x =
    List.zip els (List.rev els)
    |> List.find (fun (el,elRev) -> el = x)
    |> snd

let lookupLst = List.zip els (List.rev els)

let reflect'' x =
    lookupLst
    |> List.find (fun (el,elRev) -> el = x)
    |> snd

reflect "b"

reflect' "c"

reflect'' "a"

let rec swapEls1 elA elB lst =
    match lst with
    | [] -> []
    | el :: lst' when el = elA -> elB :: swapEls1 elA elB lst'
    | el :: lst' when el = elB -> elA :: swapEls1 elA elB lst'
    | el :: lst' -> el :: swapEls1 elA elB lst'

let rec swapEls2 elA elB lst =
    lst
    |> List.map ( fun el ->
        match el with
        | _ when el = elA -> elB
        | _ when el = elB -> elA
        | _ -> el)

let rec swapElsByIndexL indexA indexB lst =
    lst
    |> List.updateAt indexA lst[indexB]
    |> List.updateAt indexB lst[indexA]

let rec swapElsByIndexA indexA indexB arr =
    arr
    |> Array.updateAt indexA arr[indexB]
    |> Array.updateAt indexB arr[indexA]

let swapElsByIndex indexA indexB lst =
    lst
    |> List.mapi (fun i el ->
        match i with
        | _ when i = indexA -> lst[indexB]
        | _ when i = indexB -> lst[indexA]
        | _ -> el)


let rec merge lst1 lst2 = 
    match lst1,lst2 with
    | el1::lst1', el2::lst2' when el1 < el2 -> merge lst2 lst1
    | el1::lst1', _ -> el1 :: merge lst1' lst2
    | [], lst | lst, [] -> lst

let rec mergeSort lst = 
    match List.splitInto 2 lst with
    | [a;b] -> merge (mergeSort a) (mergeSort b)
    | _ -> lst

mergeSort [0;2;7;4;8;3;11;1]


let els' = [(fun (a,_,_) -> a); (fun (_,a,_) -> a); (fun (_,_,a) -> a)]



//-----------------------------------------------------//
//                    Value Restriction                //
//-----------------------------------------------------//

/// polymorphic typed value
let v0: 'a option list = [None]

/// function returning polymorphic type value
let v0' = id v0

let v0: int option list = id v0



/// syntatic function
let vf1 x = (List.map (fun x -> x,x)) x

//vf1 [1;2]

//vf1 ["a"; "b"]


/// syntactic value (actual function)
let v1 = List.map (fun x -> x,x)

//v1 [1;2]

//v1 ["a"; "b"]

/// polymorphic type variable specialised to int
let v2 = List.map (fun (x:int) -> x,x)

let vf3 = [id]
let vf3r = List.rev (vf3: ('a -> 'a) list)


/// actual value containing polymorphic functions
let els'' = [(fun (a,_,_) -> a); (fun (_,a,_) -> a); (fun (_,_,a) -> a)]

let makesEls' (f1: 'a*'a*'a->'a) = [f1;f1;f1]

makesEls' (fun (a,_,_) -> a)

let xx = els''[0](0,0,0)

/// list of polymorhic functions as argument to function
let y = id els'' 

/// list with type defined
let elsRev = id (els'': (int * int * int -> int) list)


// References on Value Restriction
// (1) F# language reference - does not explain it, but tells you what to do
// https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/generics/automatic-generalization
//
// (2) Best SHORT blog description of why it is needed (from https://stackoverflow.com/users/180286/fyodor-soikin)
// https://stackoverflow.com/questions/61243289/understanding-f-value-restriction
//
// (3) Useful in that if you don't want to understand what is going on it gives you pragmatic solutions
// https://stackoverflow.com/questions/1131456/understanding-f-value-restriction-errors
//
// (4) This is the F# world explainer - I don't myself much like it
// https://web.archive.org/web/20160313200648/http://blogs.msdn.com/b/mulambda/archive/2010/05/01/value-restriction-in-f.aspx
//
// (5) This is an old, very good, explanation of why value restriction is needed in ML (F# precursor) although the
// syntax of ML is slightly different from F# all the exam0les should be pretty obvious. I find this the best detailed
// explanation. (F# inherits value restriction from ML).
// http://mlton.org/ValueRestriction#:~:text=The%20value%20restriction%20prevents%20a,hence%20would%20break%20type%20safety.








