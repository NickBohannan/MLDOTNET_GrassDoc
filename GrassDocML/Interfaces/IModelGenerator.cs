using System;
using Microsoft.ML;

namespace GrassDocML.Interfaces
{
    public interface IModelGenerator
    {
        struct ModelSettings { }
        ITransformer GenerateModel(MLContext mlContext);   
    }
}
