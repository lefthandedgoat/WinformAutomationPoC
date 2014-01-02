module guts

open System.Diagnostics
open System.Windows.Automation
open System.Linq

let mutable (app : AutomationElement) = null
let mutable (appProcess : Process) = null

let start (name : string) (pathToProcess : string) = 
    appProcess <- Process.Start(pathToProcess)
    System.Threading.Thread.Sleep(500) //cheese! todo build retryability into grabbing app on launch
    app <- AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, name))

let quit () = 
    appProcess.CloseMainWindow() |> ignore
    appProcess.Dispose()

//find stuff
let elements selector =     
    let byId id = app.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, id)).Cast<AutomationElement>() |> List.ofSeq
    let byName name = app.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name)).Cast<AutomationElement>() |> List.ofSeq
    //todo add more ways to get stuff and build retryability in
    try
        seq {
            yield (byName selector)
            yield (byId selector)            
        }
        |> Seq.filter(fun list -> not(list.IsEmpty))
        |> Seq.head
    with | ex -> []

let element selector = elements selector |> List.head

//do stuff
let click selector =
    let all = elements selector
    if List.length <| all > 1 then
        printfn "shiza"
    let elem = element selector
    (elem.GetCurrentPattern(InvokePattern.Pattern) :?> InvokePattern).Invoke()

let read selector =
    let elem = element selector
    elem.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString()

//assert stuff
let ( == ) selector value =
    //tood build retry logic etc
    let actual = read selector
    if actual <> value then raise (System.Exception(sprintf "equality check failed.  expected: %s, got: %s" value actual))