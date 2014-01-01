module guts

open System.Diagnostics
open System.Windows.Automation
open System.Linq

let mutable (app : AutomationElement) = null
let mutable (appProcess : Process) = null

let start (name : string) (pathToProcess : string) = 
    appProcess <- Process.Start(pathToProcess)
    System.Threading.Thread.Sleep(500) //cheese!
    app <- AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, name))

let quit () = 
    appProcess.CloseMainWindow() |> ignore
    appProcess.Dispose()

let elements selector =     
    let byId id = app.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, id)).Cast<AutomationElement>() |> List.ofSeq
    let byName name = app.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name)).Cast<AutomationElement>() |> List.ofSeq

    try
        seq {
            yield (byId selector)
            yield (byName selector)
        }
        |> Seq.filter(fun list -> not(list.IsEmpty))
        |> Seq.head
    with | ex -> []

let element selector = elements selector |> List.head

let click selector =
    let elem = element selector
    (elem.GetCurrentPattern(InvokePattern.Pattern) :?> InvokePattern).Invoke()