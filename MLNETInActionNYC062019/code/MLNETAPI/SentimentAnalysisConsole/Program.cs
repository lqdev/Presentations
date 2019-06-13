using System;
using Microsoft.ML;
using SentimentAnalysisLibrary.DataModels;

namespace SentimentAnalysisConsole
{
    class Program
    {
        static MLContext mlContext;

        static void Main(string[] args)
        {
            // Create MLContext
            mlContext = new MLContext();

            // Load Data
            var data = mlContext.Data.LoadFromTextFile<ModelInput>("Data/yelp_labelled.tsv",hasHeader:true);

            // Split data into training and test sets
            DataOperationsCatalog.TrainTestData dataSplit = mlContext.Data.TrainTestSplit(data);
            IDataView trainingData = dataSplit.TrainSet;
            IDataView testData = dataSplit.TestSet;

            // Define training pipeline
            IEstimator<ITransformer> trainingPipeline = GetTrainingPipeline();

            // Train model using training pipeline
            ITransformer model = TrainModel(trainingData, trainingPipeline);

            var preview = model.Transform(testData).Preview();

            // Evaluate the model
            Evaluate(testData,model);

            // Save the model
            mlContext.Model.Save(model, trainingData.Schema, "MLModel.zip");

            Console.ReadKey();
        }

        static IEstimator<ITransformer> GetTrainingPipeline()
        {
            IEstimator<ITransformer> dataPrepPipeline = mlContext.Transforms.Text.FeaturizeText("Features", "SentimentText")
                .Append(mlContext.Transforms.NormalizeMinMax("Features"));

            IEstimator<ITransformer> trainer = mlContext.BinaryClassification.Trainers.SdcaLogisticRegression();

            IEstimator<ITransformer> trainingPipeline = dataPrepPipeline.Append(trainer);
    
            return trainingPipeline;
        }

        static ITransformer TrainModel(IDataView trainingData, IEstimator<ITransformer> trainingPipeline)
        {
            return trainingPipeline.Fit(trainingData);
        }

        static void Evaluate(IDataView testData, ITransformer trainedModel)
        {
            IDataView scoredData = trainedModel.Transform(testData);
            var evaluationMetrics = mlContext.BinaryClassification.Evaluate(scoredData);

            Console.WriteLine($"Model Accuracy: {evaluationMetrics.Accuracy}");
        }
    }
}
