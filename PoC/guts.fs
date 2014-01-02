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
let rec elementsWithin selector context =
    let byId (id : string) (context : AutomationElement) = 
        if id.StartsWith("#") then
            let id = id.Replace("#", "")
            context.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, id)).Cast<AutomationElement>() |> List.ofSeq
        else
            []
    let byName name (context : AutomationElement) = context.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name)).Cast<AutomationElement>() |> List.ofSeq
    let byMenu (selector : string) (context : AutomationElement) =
        if selector.StartsWith(">") then
            let expand (elem : AutomationElement) = (elem.GetCurrentPattern(ExpandCollapsePattern.Pattern) :?> ExpandCollapsePattern).Expand()
            let parts = selector.Split('>') |> List.ofArray |> List.filter (fun part -> part <> "")
            let rec toLastMenuItem (parts : string list) (context : AutomationElement) =
                match parts with
                | [] -> []
                | part :: [] -> elementsWithin part context
                | part :: _ -> 
                    let elem = ((elementsWithin part context) |> List.head)
                    expand elem
                    toLastMenuItem parts.Tail elem
            toLastMenuItem parts context
        else
            []

    //todo add more ways to get stuff and build retryability in
    try
        seq {
            yield (byName selector context)
            yield (byId selector context)
            yield (byMenu selector context)
        }
        |> Seq.filter(fun list -> not(list.IsEmpty))
        |> Seq.head
    with | ex -> []

let elementWithin selector context = elementsWithin selector context |> List.head
let elements selector = elementsWithin selector app
let element selector = elementWithin selector app

//do stuff
let click selector =    
    elements selector
    |> List.iter (fun elem ->
        try (elem.GetCurrentPattern(InvokePattern.Pattern) :?> InvokePattern).Invoke() 
        with | _ -> ()) //try to click everything. 1 + 2 + 3 would fail otherwise because there are more than 3 elements with 3 as the name property (I think because of in the results window)
    
let read selector =
    let elem = element selector
    elem.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString()

//assert stuff
let ( == ) selector value =
    //tood build retry logic etc
    let actual = read selector
    if actual <> value then raise (System.Exception(sprintf "equality check failed.  expected: %s, got: %s" value actual))