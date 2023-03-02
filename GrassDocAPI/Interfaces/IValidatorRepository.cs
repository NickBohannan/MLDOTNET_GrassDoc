using System;
using Microsoft.AspNetCore.Http;

namespace GrassDocAPI.Interfaces
{
    public interface IValidatorRepository
    {
        public bool IsImage(IFormFile image);
    }
}
