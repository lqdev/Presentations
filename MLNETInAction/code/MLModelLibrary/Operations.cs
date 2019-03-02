using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Data.DataView;
using Microsoft.ML.Core.Data;
using MLModelLibrary.Domain;

namespace MLModelLibrary
{
    public class Operations
    {
        public static ValueTuple<IDataView,IDataView> SplitData(MLContext context, IDataView source)
        {
            return context.MulticlassClassification.TrainTestSplit(source,0.1);
        }

        public static ITransformer Train(MLContext context, IDataView trainData)
        {
            var pipeline = context.Transforms.Concatenate("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth")
                .Append(context.Transforms.Normalize("Features"))
                .Append(context.Transforms.Conversion.MapValueToKey("Label"))
                .AppendCacheCheckpoint(context)
                .Append(context.MulticlassClassification.Trainers.StochasticDualCoordinateAscent("Label", "Features"))
                .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            ITransformer trainedModel = pipeline.Fit(trainData);
            
            return trainedModel;
        }

        public static double Evaluate(MLContext context, ITransformer trainedModel, IDataView testData)
        {
            MultiClassClassifierMetrics metrics = context.MulticlassClassification.Evaluate(trainedModel.Transform(testData));
            double accuracy = metrics.AccuracyMacro;
            return accuracy;
        }

        public static string Predict(PredictionEngine<IrisData,IrisPrediction> predictionEngine, IrisData input)
        {
            IrisPrediction prediction = predictionEngine.Predict(input);
            return prediction.FlowerType;
        }

        public static void SaveModel(MLContext context, ITransformer trainedModel,string outputPath)
        {
            using (var fs = File.Create(outputPath))
            {
                context.Model.Save(trainedModel, fs);
            }
        }

        public static ITransformer LoadModel(MLContext context,string modelPath)
        {
            ITransformer model;
            using (var fs = File.OpenRead(modelPath))
            {
                model = context.Model.Load(fs);
            }
            return model;
        }
    }
}
