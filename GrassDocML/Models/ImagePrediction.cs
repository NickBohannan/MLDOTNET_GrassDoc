using System;

namespace GrassDocML.Models
{
    public class ImagePrediction : ImageData
    {
        public float[] Score;
        public string PredictedLabelValue;
    }
}
