// Function that takes in 
let greet (name:string option) = 
    match name with 
    | Some x -> printfn "Hello %s" x
    | None -> printfn "Hello World"

// Print out Hello Luis
greet (Some "Luis")

// Print out Hello World
greet None