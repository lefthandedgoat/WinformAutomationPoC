open runner
    
context "poc"
once (fun _ -> ()) //open app
lastly (fun _ -> ()) //close app

"a test" &&& fun _ ->
    ()