using Microsoft.ML.Data;

namespace SentimentAnalysisLibrary.DataModels
{
    public class ModelInput
    {
        [LoadColumn(0)]
        public string SentimentText { get; set; }


        [ColumnName("Label"), LoadColumn(1)]
        public bool Sentiment { get; set; }
    }
}
