using Microsoft.AspNetCore.Mvc;
using SimpleDotNet9WebApi.Services;
using System.Threading.Tasks;
using SimpleDotNet9WebApi.Interfaces;

namespace SimpleDotNet9WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalApiController(IExternalApiClient externalApiClient) : ControllerBase
    {
        [HttpGet("data")]
        public async Task<IActionResult> GetExternalData()
        {
            var data = await externalApiClient.GetExternalDataAsync("/get/1");
            return Ok(data);
        }
    }
}