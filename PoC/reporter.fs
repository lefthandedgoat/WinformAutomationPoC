module reporter

open System

type IReporter =
   abstract member testStart : string -> unit
   abstract member pass : unit -> unit
   abstract member fail : Exception -> string -> unit
   abstract member todo : unit -> unit
   abstract member skip : unit -> unit
   abstract member testEnd : string -> unit
   abstract member describe : string -> unit
   abstract member contextStart : string -> unit
   abstract member contextEnd : string -> unit
   abstract member summary : int -> int -> int -> int -> unit
   abstract member write : string -> unit
   abstract member suggestSelectors : string -> string list -> unit
   abstract member quit : unit -> unit
   abstract member suiteBegin : unit -> unit
   abstract member suiteEnd : unit -> unit
   abstract member coverage : string -> byte [] -> unit

type ConsoleReporter() =   
    interface IReporter with               
        member this.pass () = 
            Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("Passed");
            Console.ResetColor()

        member this.fail ex id = 
            Console.ForegroundColor <- ConsoleColor.Red
            Console.WriteLine("Error: ");
            Console.ResetColor()
            Console.WriteLine(ex.Message);
            Console.WriteLine("Stack: ");
            ex.StackTrace.Split([| "\r\n"; "\n" |], StringSplitOptions.None)
            |> Array.iter (fun trace -> 
                Console.ResetColor()
                if trace.Contains(".FSharp.") then
                    Console.WriteLine(trace)
                else
                    if trace.Contains(":line") then
                        let beginning = trace.Split([| ":line" |], StringSplitOptions.None).[0]
                        let line = trace.Split([| ":line" |], StringSplitOptions.None).[1]
                        Console.Write(beginning)
                        Console.ForegroundColor <- ConsoleColor.DarkGreen
                        Console.WriteLine(":line" + line)
                    else
                        Console.ForegroundColor <- ConsoleColor.DarkGreen
                        Console.WriteLine(trace)
                Console.ResetColor())

        member this.describe d = Console.WriteLine d          
        
        member this.contextStart c = Console.WriteLine (String.Format("context: {0}", c))        
        
        member this.contextEnd c = ()
        
        member this.summary minutes seconds passed failed =
            Console.WriteLine()
            Console.WriteLine("{0} minutes {1} seconds to execute", minutes, seconds)
            if failed = 0 then
                Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("{0} passed", passed)
            Console.ResetColor()
            if failed > 0 then
                Console.ForegroundColor <- ConsoleColor.Red        
            Console.WriteLine("{0} failed", failed)    
            Console.ResetColor()        
        
        member this.write w = Console.WriteLine w        
        
        member this.suggestSelectors selector suggestions = 
            Console.ForegroundColor <- ConsoleColor.DarkYellow                    
            Console.WriteLine("Couldnt find any elements with selector '{0}', did you mean:", selector)
            suggestions |> List.iter (fun suggestion -> Console.WriteLine("\t{0}", suggestion))
            Console.ResetColor()
        member this.testStart id =
            Console.ForegroundColor <- ConsoleColor.DarkCyan
            Console.WriteLine("Test: {0}", id)
            Console.ResetColor()

        member this.testEnd id = ()
        
        member this.quit () = ()
        
        member this.suiteBegin () = ()

        member this.suiteEnd () = ()

        member this.coverage url ss = ()
        
        member this.todo () = ()

        member this.skip () = () 