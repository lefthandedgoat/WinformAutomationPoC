module calculator

open guts

//app info
let name = "Calculator"
let path = "Calc.exe"

//selectors of sorts?
let results = "#150" //made up psuedo selector for Id

//app specific helper functions
let enter (number : string) = 
    number 
    |> List.ofSeq 
    |> List.iter (fun c -> click (string(c))) 

let clear _ = click "Clear"

let private enterThen numbers action =
    numbers
    |> List.iter (fun number -> 
                    enter (number.ToString())
                    click action)

let add numbers = enterThen numbers "Add"
    
let multiply numbers = enterThen numbers "Multiply"