using System;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using MLModelLibrary;
using MLModelLibrary.Domain;

namespace MLModelTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Define data path
            var dataPath = "Data/iris.csv";

            //Create Context
            var mlContext = new MLContext();

            //Load Data
            var data = mlContext.Data.ReadFromTextFile<IrisData>(dataPath,hasHeader:true,separatorChar:',');

            //Split data into train and test set
            var (trainData,testData) = Operations.SplitData(mlContext, data);

            //Preview Tranining Data
            var trainDataPreview = trainData.Preview();

            //Train model
            ITransformer trainedModel = Operations.Train(mlContext, trainData);

            //Apply trained model to test data
            IDataView transformedData = trainedModel.Transform(testData);

            //Preview transformed test data
            var transformedDataPreview = transformedData.Preview();

            //Evaluate model using test data
            double rSquared = Operations.Evaluate(mlContext, trainedModel, testData);

            Console.WriteLine("RSquared Metric:\t{0:P4}", rSquared);

            IrisData testInput = new IrisData
            {
                SepalLength = 3.3f,
                SepalWidth = 1.6f,
                PetalLength = 0.2f,
                PetalWidth = 5.1f
            };

            PredictionEngine<IrisData, IrisPrediction> predictionEngine = mlContext.Model.CreatePredictionEngine<IrisData, IrisPrediction>(trainedModel);

            //Make prediction on unseen instance of data using trained model
            string prediction = Operations.Predict(predictionEngine, testInput);

            Console.WriteLine("The prediction is {0}", prediction);

            //Save Model
            Operations.SaveModel(mlContext, trainedModel, "iris_model.zip");

            Console.ReadKey();

            
        }
    }
}
