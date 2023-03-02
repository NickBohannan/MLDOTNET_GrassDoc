using System;
using System.Threading.Tasks;
using GrassDocML.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ML;

namespace GrassDocAPI.Interfaces
{
    public interface IDiagnoseGrassRepository
    {
        public Task<ImagePrediction> DiagnoseGrassImage(IFormFile submittedImage);
    }
}
