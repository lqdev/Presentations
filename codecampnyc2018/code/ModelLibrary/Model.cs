using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;
using Microsoft.ML.Legacy.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class Model
    {
        public static async Task<PredictionModel<IrisData, IrisPrediction>> Train(string dataPath, string modelPath)
        {
            //Initialize Learning Pipeline
            LearningPipeline pipeline = new LearningPipeline();

            // Load Data
            pipeline.Add(new TextLoader(dataPath).CreateFrom<IrisData>(separator: ','));

            // Transform Data
            // Assign numeric values to text in the "Label" column, because
            // only numbers can be processed during model training
            pipeline.Add(new Dictionarizer("Label"));

            // Vectorize Features
            pipeline.Add(new ColumnConcatenator("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"));

            // Add Learner
            pipeline.Add(new StochasticDualCoordinateAscentClassifier());

            // Convert Label back to text
            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });

            // Train Model
            var model = pipeline.Train<IrisData, IrisPrediction>();

            // Persist Model
            await model.WriteAsync(modelPath);

            return model;
        }

        public static async Task<PredictionModel<IrisData, IrisPrediction>> LoadZipModel(string modelPath)
        {
            var model = await PredictionModel.ReadAsync<IrisData, IrisPrediction>(modelPath);
            return model;
        }

        public static async Task<PredictionModel<IrisData, IrisPrediction>> LoadSerializeModel(Stream serializedModel)
        {
            var model = await PredictionModel.ReadAsync<IrisData, IrisPrediction>(serializedModel);
            return model;
        }

        public static string MakePrediction(PredictionModel<IrisData, IrisPrediction> model, IrisData input)
        {
            var prediction = model.Predict(input);
            return prediction.PredictedLabels;
        }
    }
}
