using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using MLModelLibrary;
using MLModelLibrary.Domain;

namespace MLModelAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Register MLContext
            services.AddSingleton<MLContext>();

            //Register and Initialize PredictionEngine Logic
            services.AddSingleton<PredictionEngine<IrisData, IrisPrediction>>(ctx => {
                MLContext mlContext = ctx.GetRequiredService<MLContext>();

                ITransformer trainedModel = Operations.LoadModel(mlContext, "MLModels/iris_model.zip");

                PredictionEngine<IrisData, IrisPrediction> predictionEngine = mlContext.Model.CreatePredictionEngine<IrisData, IrisPrediction>(trainedModel);

                return predictionEngine;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
