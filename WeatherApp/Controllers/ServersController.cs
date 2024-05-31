using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;

namespace WeatherApp.Controllers;

[ApiController]
[Route("servers")]
public class ServersController : ControllerBase
{
    private readonly ServerService _serverService;

    public ServersController(ServerService serverService)
    {
        _serverService = serverService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServers()
    {
        return Ok(await _serverService.GetAllServers());
    }
}