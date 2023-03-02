using System;
using System.IO;
using System.Drawing;
using GrassDocAPI.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GrassDocAPI.Repositories
{
    public class ValidatorRepository : IValidatorRepository
    {
        public bool IsImage(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                var dataIsImage = false;
                image.OpenReadStream().CopyTo(memoryStream);
                byte[] filebyteArr = memoryStream.ToArray();

                using (var imageReadStream = new MemoryStream(filebyteArr))
                {
                    try
                    {
                        using (var possibleImage = Image.FromStream(imageReadStream))
                        {
                        }
                        dataIsImage = true;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        dataIsImage = false;
                    }
                }

                return dataIsImage;
            }
        }
    }
}
