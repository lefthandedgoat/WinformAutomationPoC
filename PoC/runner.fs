module runner

open reporter
open System

let rec private last = function
    | hd :: [] -> hd
    | hd :: tl -> last tl
    | _ -> failwith "Empty list."

type Test (description: string, func : (unit -> unit), number : int) =
    member x.Description = description
    member x.Func = func
    member x.Number = number

type suite () = class
    let mutable context : string = null
    let mutable once = fun () -> ()
    let mutable before = fun () -> ()
    let mutable after = fun () -> ()
    let mutable lastly = fun () -> () 
    let mutable tests : Test list = []

    member x.Context
        with get() = context
        and set(value) = context <- value
    member x.Once
        with get() = once
        and set(value) = once <- value
    member x.Before
        with get() = before
        and set(value) = before <- value
    member x.After
        with get() = after
        and set(value) = after <- value
    member x.Lastly
        with get() = lastly
        and set(value) = lastly <- value
    member x.Tests
        with get() = tests
        and set(value) = tests <- value
end

let failFast = ref false
let mutable reporter : IReporter = new ConsoleReporter() :> IReporter
let mutable suites = [new suite()]
let mutable todo = fun () -> ()
let mutable skipped = fun () -> ()

let once f = (last suites).Once <- f
let before f = (last suites).Before <- f
let after f = (last suites).After <- f
let lastly f = (last suites).Lastly <- f
let context c = 
    if (last suites).Context = null then 
        (last suites).Context <- c
    else 
        let s = new suite()
        s.Context <- c
        suites <- suites @ [s]

let ( &&& ) description f = (last suites).Tests <- (last suites).Tests @ [Test(description, f, (last suites).Tests.Length + 1)]
let ( &&! ) description f = (last suites).Tests <- (last suites).Tests @ [Test(description, skipped, (last suites).Tests.Length + 1)]
//todo add back wip mode

let mutable passedCount = 0
let mutable failedCount = 0
let mutable contextFailed = false
let mutable failedContexts : string list = []
let mutable failed = false
let mutable (failureMessage : string) = null

let pass () =    
    passedCount <- passedCount + 1
    reporter.pass ()

let fail (ex : Exception) id =
    if failFast = ref true then failed <- true        
    failedCount <- failedCount + 1
    contextFailed <- true
    let p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
    let f = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
    reporter.fail ex id

let run () =
    reporter.suiteBegin()
    let stopWatch = new Diagnostics.Stopwatch()
    stopWatch.Start()      
    
    let runtest (suite : suite) (test : Test) =
        if failed = false then
            let desc = if test.Description = null then (String.Format("Test #{0}", test.Number)) else test.Description
            try 
                reporter.testStart desc  
                if System.Object.ReferenceEquals(test.Func, todo) then 
                    reporter.todo ()
                else if System.Object.ReferenceEquals(test.Func, skipped) then 
                    reporter.skip ()
                else
                    suite.Before ()
                    test.Func ()
                    suite.After ()
                    pass()
            with
                | ex when failureMessage <> null && failureMessage = ex.Message -> pass()
                | ex -> fail ex desc
            reporter.testEnd desc
        
        failureMessage <- null            

    
    suites
    |> List.iter (fun s ->
        if failed = false then
            contextFailed <- false
            if s.Context <> null then reporter.contextStart s.Context
            s.Once ()            
            s.Tests |> List.iter (fun t -> runtest s t)
            s.Lastly ()        
            if contextFailed = true then failedContexts <- failedContexts @ [s.Context]
            if s.Context <> null then reporter.contextEnd s.Context
    )
    
    stopWatch.Stop()    
    reporter.summary stopWatch.Elapsed.Minutes stopWatch.Elapsed.Seconds passedCount failedCount 
    reporter.suiteEnd()