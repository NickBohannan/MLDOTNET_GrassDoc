using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrassDocAPI.Interfaces;
using GrassDocML.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrassDocAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnoseGrassController : ControllerBase
    {
        private readonly ILogger<DiagnoseGrassController> _logger;
        private IDiagnoseGrassRepository _diagnoseGrassRepository;

        public DiagnoseGrassController(ILogger<DiagnoseGrassController> logger, IDiagnoseGrassRepository diagnoseGrassRepository)
        {
            _logger = logger;
            _diagnoseGrassRepository = diagnoseGrassRepository;
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostSingleImageForDiseaseClassification(IFormFile submittedImage)
        {
            _logger.Log(LogLevel.Information, "Image submitted... attempting classification.");
            ImagePrediction prediction = await _diagnoseGrassRepository.DiagnoseGrassImage(submittedImage);

            if (prediction != null)
            {
                _logger.Log(LogLevel.Information, $"Prediction complete.");
                return Ok($"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
            }

            return BadRequest("Prediction unable to be generated due to image issue.");
        }
    }
}
