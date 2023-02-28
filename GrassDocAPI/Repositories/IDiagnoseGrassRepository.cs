using System;
using System.Threading.Tasks;
using GrassDocML.Models;
using Microsoft.AspNetCore.Http;

namespace GrassDocAPI.Repositories
{
    public interface IDiagnoseGrassRepository
    {
        public Task<ImagePrediction> DiagnoseGrassImage(IFormFile submittedImage);
    }
}
