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
        private IValidatorRepository _validatorRepository;

        public DiagnoseGrassRepository(PredictionEnginePool<ImageData, ImagePrediction> predictionEnginePool, IValidatorRepository validatorRepository)
        {
            _predictionEnginePool = predictionEnginePool;
            _validatorRepository = validatorRepository;
        }

        public async Task<ImagePrediction> DiagnoseGrassImage(IFormFile submittedImage)
        {
            // if image is null or not an image, return null
            if (submittedImage.Length == 0 || !_validatorRepository.IsImage(submittedImage))
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
