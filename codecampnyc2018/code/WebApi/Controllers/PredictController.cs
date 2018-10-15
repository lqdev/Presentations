using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary;

namespace WebApi
{
    [Route("/api/[controller]")]
    [ApiController]
    public class Predict : ControllerBase
    {
        // POST api/predict
        [HttpPost]
        public async Task<ActionResult<string>> Post(IrisData instance)
        {
            var model = await Model.LoadZipModel("model.zip");
            var prediction = Model.MakePrediction(model,instance);
            return Ok(prediction);
        }
    }
}