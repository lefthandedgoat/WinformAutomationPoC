open runner
open guts
open calculator

context "PoC"
once (fun _ -> start calculator.name calculator.path)
before clear

"type a random number" &&& fun _ ->    
    let random = System.Random().Next(100, 10000).ToString()    
    enter random
    results == random

"clear works" &&& fun _ ->    
    enter "1234"
    results == "1234"
    clear ()
    results == "0"

"adding 1 and 2 is 3" &&& fun _ ->
    add [1; 2]
    results == "3"

"adding 1 and 2 and 10 and 40 is 53" &&& fun _ ->
    add [1; 2; 10; 40]
    results == "53"

run ()

System.Console.ReadKey() |> ignore

quit ()