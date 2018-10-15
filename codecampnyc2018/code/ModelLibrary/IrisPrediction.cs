using Microsoft.ML.Runtime.Api;

namespace ModelLibrary
{
    public class IrisPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabels;
    }
}