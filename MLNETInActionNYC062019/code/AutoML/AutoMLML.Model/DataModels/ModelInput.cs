//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using Microsoft.ML.Data;

namespace AutoMLML.Model.DataModels
{
    public class ModelInput
    {
        [ColumnName("SentimentText"), LoadColumn(0)]
        public string SentimentText { get; set; }


        [ColumnName("Sentiment"), LoadColumn(1)]
        public bool Sentiment { get; set; }
    }
}
