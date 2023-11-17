using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using GrassDocML.Interfaces;
using GrassDocML.Models;
using GrassDocML.Configuration;

namespace GrassDocML.Repositories
{
    public class ClassificationRepository : IClassificationRepository
    {
        struct ModelSettings
        {
            public const int ImageHeight = 224;
            public const int ImageWidth = 224;
            public const float Mean = 117;
            public const float Scale = 1;
            public const bool ChannelsLast = true;
        }

        public ITransformer GenerateModel(MLContext mlContext)
        {
            // Develop the pipline
            IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: PathConfiguration.RiceFolder, inputColumnName: nameof(ImageData.ImagePath))
            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: ModelSettings.ImageWidth, imageHeight: ModelSettings.ImageHeight, inputColumnName: "input"))
            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: ModelSettings.ChannelsLast, offsetImage: ModelSettings.Mean))
            .Append(mlContext.Model.LoadTensorFlowModel(PathConfiguration.InceptionTensorFlowModel).ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label"))
            .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
            .AppendCacheCheckpoint(mlContext);

            // load training data and plug into pipeline
            IDataView trainingData = mlContext.Data.LoadFromTextFile<ImageData>(path: PathConfiguration.TrainTagsCsv, separatorChar: ',', hasHeader: false);
            ITransformer model = pipeline.Fit(trainingData);

            // load test data and validate schema
            IDataView testData = mlContext.Data.LoadFromTextFile<ImageData>(path: PathConfiguration.TestTagsCsv, separatorChar: ',', hasHeader: false);
            IDataView predictions = model.Transform(testData);

            // Create an IEnumerable for the predictions for displaying results
            IEnumerable<ImagePrediction> imagePredictionData = mlContext.Data.CreateEnumerable<ImagePrediction>(predictions, true);
            DisplayResults(imagePredictionData);

            MulticlassClassificationMetrics metrics = mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelKey", predictedLabelColumnName: "PredictedLabel");

            Console.WriteLine($"LogLoss is: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss is: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");

            return model;
        }

        private void DisplayResults(IEnumerable<ImagePrediction> imagePredictionData)
        {
            foreach (ImagePrediction prediction in imagePredictionData)
            {
                Console.WriteLine($"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
            }
        }
    }
}
