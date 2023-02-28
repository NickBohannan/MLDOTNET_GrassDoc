using System;
using System.IO;
using System.Threading.Tasks;
using GrassDocML.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ML;
using GrassDocAPI.Interfaces;

namespace GrassDocAPI.Repositories
{
    public class DiagnoseGrassRepository : IDiagnoseGrassRepository
    {
        private readonly PredictionEnginePool<ImageData, ImagePrediction> _predictionEnginePool;

        public DiagnoseGrassRepository(PredictionEnginePool<ImageData, ImagePrediction> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        public async Task<ImagePrediction> DiagnoseGrassImage(IFormFile submittedImage)
        {
            if (submittedImage.Length == 0)
            {
                return null;
            }

            var filePath = Path.GetTempFileName();

            using (var stream = File.Create(filePath))
            {
                await submittedImage.CopyToAsync(stream);
            }

            ImagePrediction prediction = _predictionEnginePool.Predict<ImageData, ImagePrediction>(modelName: "GrassClassificationModel", example: new ImageData
            {
                ImagePath = filePath,
                Label = "Submitted Image"
            });

            return prediction;
        }
    }
}
