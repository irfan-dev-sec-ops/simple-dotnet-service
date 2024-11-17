using Microsoft.AspNetCore.Mvc;
using SimpleDotNet9WebApi.Interfaces;
using SimpleDotNet9WebApi.Models;

namespace SimpleDotNet9WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExternalApiController(IExternalApiClient externalApiClient) : ControllerBase
{
    [HttpGet("data/{id}")]
    public async Task<IActionResult> GetExternalData([FromRoute] string id)
    {
        var (statusCode, content) = await externalApiClient.GetExternalDataAsync(id);
        return StatusCode(statusCode, content);
    }

    [HttpPost("data")]
    public async Task<IActionResult> GetExternalDataPost([FromBody] Student student)
    {
        var (statusCode, content) = await externalApiClient.GetExternalDataPostAsync(student);
        return StatusCode(statusCode, content);
    }
}