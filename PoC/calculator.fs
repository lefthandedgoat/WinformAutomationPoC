module calculator

open guts

//app info
let name = "Calculator"
let path = "Calc.exe"

//selectors of sorts?
let results = "150"

//app specific helper functions
let enter (number : string) = 
    number 
    |> List.ofSeq 
    |> List.map (fun c -> click (string(c))) 
    |> ignore