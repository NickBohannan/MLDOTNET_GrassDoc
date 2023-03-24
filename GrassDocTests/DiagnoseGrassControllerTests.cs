using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassDocAPI.Controllers;
using GrassDocAPI.Interfaces;
using GrassDocML.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GrassDocTests
{
    public class DiagnoseGrassControllerTests
    {
        private readonly Mock<ILogger<DiagnoseGrassController>> _loggerMock;
        private readonly Mock<IDiagnoseGrassRepository> _diagnoseGrassRepositoryMock;

        public DiagnoseGrassControllerTests()
        {
            _loggerMock = new Mock<ILogger<DiagnoseGrassController>>();
            _diagnoseGrassRepositoryMock = new Mock<IDiagnoseGrassRepository>();
        }

        [Fact]
        public async Task PostSingleImageForDiseaseClassification_WhenImageIsNull_ReturnsBadRequest()
        {
            // Arrange
            var diagnoseGrassController = new DiagnoseGrassController(_loggerMock.Object, _diagnoseGrassRepositoryMock.Object);

            // Act
            var actionResult = await diagnoseGrassController.PostSingleImageForDiseaseClassification(null);

            // Assert
            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(result?.Value, "Prediction unable to be generated due to image issue.");
            Assert.IsType<ActionResult<string>>(actionResult);
        }

        [Fact]
        public async Task PostSingleImageForDiseaseClassification_WhenImageIsNotNull_ReturnsOk()
        {
            // Arrange
            var diagnoseGrassController = new DiagnoseGrassController(_loggerMock.Object, _diagnoseGrassRepositoryMock.Object);
            var mockFormFile = new Mock<IFormFile>();
            var mockImagePrediction = new Mock<ImagePrediction>();
            mockImagePrediction.SetupAllProperties();
            mockImagePrediction.Object.ImagePath = "test.jpg";
            mockImagePrediction.Object.PredictedLabelValue = "test";
            mockImagePrediction.Object.Score = new float[] { 0.5f };
            _diagnoseGrassRepositoryMock.Setup(x => x.DiagnoseGrassImage(It.IsAny<IFormFile>())).ReturnsAsync(mockImagePrediction.Object);

            // Act
            var actionResult = await diagnoseGrassController.PostSingleImageForDiseaseClassification(mockFormFile.Object);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(result?.Value?.ToString(), $"Image: {mockImagePrediction.Object.ImagePath} predicted as: {mockImagePrediction.Object.PredictedLabelValue} with score: {mockImagePrediction.Object.Score.Max()}");
            Assert.IsType<ActionResult<string>>(actionResult);
        }
    }
}
