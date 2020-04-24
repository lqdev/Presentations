open Saturn
open Giraffe


// Define model input and output schema
module Schema = 

    open Microsoft.ML.Data

    [<CLIMutable>]
    type ModelInput = {
        [<LoadColumn(0)>] Comment: string
        [<LoadColumn(1); ColumnName("Label")>] Sentiment: bool
    }

    [<CLIMutable>]
    type ModelOutput = {
        [<ColumnName("PredictedLabel")>] PredictedSentiment: bool
        Score: float32
    }

module Config = 

    open System
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.ML

    // Define port and URL to listen on
    let PORT = Environment.GetEnvironmentVariable("FUNCTIONS_HTTPWORKER_PORT")
    let URL = sprintf "http://0.0.0.0:%s" PORT


    // Configure services
    let MODEL_URI = Environment.GetEnvironmentVariable("MODEL_URI")
    let servicesConfig (services:IServiceCollection) = 
        services
            .AddPredictionEnginePool<Schema.ModelInput, Schema.ModelOutput>()
            .FromUri(MODEL_URI)
        |> ignore

        services

module Handlers = 

    open Microsoft.Extensions.ML
    open FSharp.Control.Tasks.ContextInsensitive
    open Schema

    // Define function to make prediction
    let postPrediction (predEngine:PredictionEnginePool<ModelInput,ModelOutput>) (input: ModelInput) = 

        input
        |> predEngine.Predict
        |> (fun sentiment -> 
            match sentiment.PredictedSentiment with
            | false -> "Negative"
            | true -> "Positive"
        )

    // Define HttpHandler
    let postPredictionHandler : HttpHandler = 
        handleContext (
            fun ctx ->
                task {
                    let predEnginePool = ctx.GetService<PredictionEnginePool<ModelInput,ModelOutput>>()
                    let! observation = ctx.BindJsonAsync<Schema.ModelInput>()
                    let prediction = observation |> postPrediction predEnginePool
                    return! ctx.WriteTextAsync prediction
                }
        )

let apiRouter = router {
    post "/analyze-sentiment" Handlers.postPredictionHandler
}

let app = application {
    service_config Config.servicesConfig
    use_router apiRouter
    url Config.URL
}

run app