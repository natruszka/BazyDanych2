using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Resources;
using WeatherApp.Services;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private ILocationService _locationService = null!;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLocations()
    {
        try
        {
            return Ok(await _locationService.GetAllLocations());
        }
        catch (InvalidOperationException)
        {
            return NotFound(ErrorMessages.TransactionFailure);
        }
    }

    [HttpGet("{server:int}")]
    public async Task<IActionResult> GetLocationFromServer(int server)
    {
        try
        {
            return Ok(await _locationService.GetLocationFromServer(server));
        }
        catch (ArgumentOutOfRangeException)
        {
            return NotFound(ErrorMessages.ServerNotFound);
        }
    }
}