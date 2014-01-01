open runner
open guts
open calculator

context "PoC"
once (fun _ -> start calculator.name calculator.path)

"type a random number" &&& fun _ ->    
    let random = System.Random().Next(100, 10000).ToString()    
    enter random

run ()

System.Console.ReadKey() |> ignore

quit ()