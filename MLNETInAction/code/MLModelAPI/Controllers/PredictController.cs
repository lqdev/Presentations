using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using MLModelLibrary;
using MLModelLibrary.Domain;

namespace MLModelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictController : ControllerBase
    {
        private readonly PredictionEngine<IrisData, IrisPrediction> _predictionEngine;

        public PredictController(PredictionEngine<IrisData,IrisPrediction> predictionEngine)
        {
            _predictionEngine = predictionEngine;
        }

        [HttpPost]
        public ActionResult Post([FromBody] IrisData input)
        {
            string prediction = Operations.Predict(_predictionEngine, input);
            return Ok(prediction);
        }
    }
}