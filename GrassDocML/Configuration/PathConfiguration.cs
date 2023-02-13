using System;
using System.IO;

namespace GrassDocML.Configuration
{
    static public class PathConfiguration
    {
        static public readonly string AssetsPath = Environment.GetEnvironmentVariable("GRASSDOC_ASSETS_PATH");
        static public readonly string ImagesFolder = Environment.GetEnvironmentVariable("GRASSDOC_IMAGES_FOLDER");

        static public readonly string RiceFolder = Environment.GetEnvironmentVariable("GRASSDOC_RICE_FOLDER");

        static public readonly string TrainTagsCsv = Environment.GetEnvironmentVariable("GRASSDOC_TRAIN_TAGS_CSV");
        static public readonly string TestTagsCsv = Environment.GetEnvironmentVariable("GRASSDOC_TEST_TAGS_CSV");

        static public readonly string PredictSingleImage = Environment.GetEnvironmentVariable("GRASSDOC_PREDICT_SINGLE_IMAGE");

        static public readonly string InceptionTensorFlowModel = Environment.GetEnvironmentVariable("GRASSDOC_INCEPTION_TENSORFLOW_MODEL");
        static public readonly string ExistingPlantModel = Environment.GetEnvironmentVariable("GRASSDOC_EXISTING_PLANT_MODEL");
    }
}
