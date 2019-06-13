using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.ML;
using SentimentAnalysisFunctionsApp;
using SentimentAnalysisLibrary.DataModels;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SentimentAnalysisFunctionsApp
{
    class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>()
                .FromFile("MLModels/MLModel.zip");
        }
    }
}
