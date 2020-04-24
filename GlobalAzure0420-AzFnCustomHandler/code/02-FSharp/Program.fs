open System
open Saturn
open Giraffe

// Define PORT
let PORT = Environment.GetEnvironmentVariable("FUNCTIONS_HTTPWORKER_PORT")
let URL = sprintf "http://0.0.0.0:%s" PORT

// Define route handlers
let getValues = {|value=[|1;2;3|]|}

let apiRouter = router {
    get "/values" (getValues |> json)
}

let app = application {
    use_router apiRouter
    url URL
}

run app