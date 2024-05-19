using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Resources;
using WeatherApp.Services;

namespace WeatherApp.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private LocationService _locationService = null!;

    public LocationsController(LocationService locationService)
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