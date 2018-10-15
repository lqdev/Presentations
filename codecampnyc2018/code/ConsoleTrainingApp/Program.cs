using System;
using ModelLibrary;

namespace ConsoleTrainingApp
{
    class Program
    {
        static void Main(string[] args)
        {

            string dataPath = "data/iris.txt";

            string modelPath = "model.zip";

            var model = Model.Train(dataPath, modelPath).Result;

            // Test data for prediction
            IrisData input = new IrisData()
            {
                SepalLength = 3.3f,
                SepalWidth = 1.6f,
                PetalLength = 0.2f,
                PetalWidth = 5.1f
            };

            string prediction = Model.MakePrediction(model, input);


            Console.WriteLine($"Predicted flower type is: {prediction}");
        }
    }
}
