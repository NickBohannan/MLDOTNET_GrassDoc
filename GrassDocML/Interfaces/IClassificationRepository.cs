using System;
using Microsoft.ML;

namespace GrassDocML.Interfaces
{
    public interface IClassificationRepository
    {
        struct ModelSettings { }
        ITransformer GenerateModel(MLContext mlContext);   
    }
}
