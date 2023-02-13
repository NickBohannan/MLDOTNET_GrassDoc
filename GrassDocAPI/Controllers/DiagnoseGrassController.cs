using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GrassDocML;
using Microsoft.AspNetCore.Http;
using System.IO;
using GrassDocML.Models;
using Microsoft.Extensions.ML;

namespace GrassDoc_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnoseGrassController : ControllerBase
    {
        private readonly PredictionEnginePool<ImageData, ImagePrediction> _predictionEnginePool;
        private readonly ILogger<DiagnoseGrassController> _logger;

        public DiagnoseGrassController(PredictionEnginePool<ImageData, ImagePrediction> predictionEnginePool, ILogger<DiagnoseGrassController> logger)
        {
            _predictionEnginePool = predictionEnginePool;
            _logger = logger;
        }

        [HttpPost("")]
        public async Task<ActionResult<string>> PostSingleImageForDiseaseClassification(IFormFile submittedImage)
        {
            _logger.Log(LogLevel.Information, "Image submitted... attempting classification.");

            if (submittedImage.Length > 0)
            {
                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    await submittedImage.CopyToAsync(stream);
                }

                ImagePrediction prediction = _predictionEnginePool.Predict<ImageData, ImagePrediction>(modelName: "GrassClassificationModel", example: new ImageData {
                    ImagePath = filePath,
                    Label = "Submitted Image"
                });

                _logger.Log(LogLevel.Information, $"Prediction complete.");

                return Ok($"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
            }

            return BadRequest();

        }
    }
}
