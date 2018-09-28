
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ModelLibrary;

namespace Predict
{
    public static class Predict
    {
        [FunctionName("Predict")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            [Blob("models/model.zip", FileAccess.Read, Connection = "AzureWebJobsStorage")] Stream serializedModel,
            ILogger log)
        {
            // Workaround for Azure Functions Host
            if (typeof(Microsoft.ML.Runtime.Data.LoadTransform) == null ||
                typeof(Microsoft.ML.Runtime.Learners.LinearClassificationTrainer) == null ||
                typeof(Microsoft.ML.Runtime.Internal.CpuMath.SseUtils) == null ||
                typeof(Microsoft.ML.Runtime.FastTree.FastTree) == null)
            {
                log.LogError("Error loading ML.NET");
                return new StatusCodeResult(500);
            }

            //Read incoming request body
            string requestBody = new StreamReader(req.Body).ReadToEnd();

            log.LogInformation(requestBody);

            //Bind request body to IrisData object
            IrisData data = JsonConvert.DeserializeObject<IrisData>(requestBody);

            //Load prediction model
            var model = await Model.LoadSerializeModel(serializedModel);
            // var model = PredictionModel.ReadAsync<IrisData, IrisPrediction>(serializedModel).Result;

            //Make prediction
            string prediction = Model.MakePrediction(model, data);

            //Return prediction
            return (IActionResult)new OkObjectResult(prediction);
        }
    }
}
