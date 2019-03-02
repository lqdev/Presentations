using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using MLModelLibrary;
using MLModelLibrary.Domain;

namespace MLModelServerless
{
    public static class Predict
    {
        [FunctionName("Predict")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            MLContext mlContext = new MLContext();

            ITransformer trainedModel = Operations.LoadModel(mlContext, "iris_model.zip");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            IrisData data = JsonConvert.DeserializeObject<IrisData>(requestBody);

            PredictionEngine<IrisData, IrisPrediction> predictionEngine = mlContext.Model.CreatePredictionEngine<IrisData, IrisPrediction>(trainedModel);

            string prediction = Operations.Predict(predictionEngine, data);

            return (ActionResult)new OkObjectResult(prediction);   
        }
    }
}
