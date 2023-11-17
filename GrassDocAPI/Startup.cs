using GrassDocAPI.Repositories;
using GrassDocAPI.Interfaces;
using GrassDocML.Configuration;
using GrassDocML.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;
using System.IO;

namespace GrassDocAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddLogging();
            services.AddSingleton<IDiagnoseGrassRepository, DiagnoseGrassRepository>();
            services.AddSingleton<IValidatorRepository, ValidatorRepository>();
            services.AddPredictionEnginePool<ImageData, ImagePrediction>()
                .FromFile(modelName: "GrassClassificationModel", filePath: new DirectoryInfo(Directory.GetCurrentDirectory()).Parent + "/GrassDocML/GeneratedMLModels/model.zip", watchForChanges: true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
