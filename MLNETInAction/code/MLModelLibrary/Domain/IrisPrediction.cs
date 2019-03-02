using Microsoft.ML.Data;

namespace MLModelLibrary.Domain
{
    public class IrisPrediction
    {
        [ColumnName("PredictedLabel")]
        public string FlowerType { get; set; }
    }
}
