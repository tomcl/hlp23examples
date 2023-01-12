()

1

1+1

"abc" + "cde"

1. + 1

n + 1

let add2 a b = a + b

let add2' (a,b) = a + b


fun n -> n + 1


let addOne n = n + 1

let addOne' = fun n -> n + 1

addOne 10 |> printfn "%d"

addOne (10) |> printfn "%d"

fun () -> 10

fun n -> printfn "%A" n
fun n -> $"{n}"






fun (x:Unit) -> ()
let throw x = ()
let throw1 = fun x -> ()
let throw2 = fun _ -> ()
ignore

let throwTuple (_,_) = ()


let fail () = failwithf "error"
let returnArg x = x
let returnTuple (x,y) = x,y
let returnTuple (x,y) = x, failwithf "error"
returnTuple (1,2)
fail()




1 + 1 |> throw

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
let rec testRec' = function
    | 0 -> 0
    | n -> 1 + testRec (n-1)

testRec 1000
testRec 1000000

/// tail-recursive test function (64 bit)
let rec testRecTail res = function
    | 0L -> res
    | n -> testRecTail (res + 1L) (n-1L)
    
testRecTail 0L 1000L
testRecTail 0 1000000000L

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




let countElements lsts =
    List.length (List.concat lsts)

let countElements' =
    fun lsts -> List.length (List.concat lsts)

let countElements'' =
    List.concat >> List.length

let countElements''': int list seq -> int =
    List.concat >> List.length

