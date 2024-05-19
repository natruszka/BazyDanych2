using Microsoft.AspNetCore.Mvc;
using WeatherApp.Resources;
using WeatherApp.Services;

namespace WeatherApp.Controllers;

[Route("stations")]
[ApiController]
public class StationController : ControllerBase
{
    private StationService _stationService = null!;

    public StationController(StationService stationService)
    {
        _stationService = stationService;
    }

    [HttpGet("raw")]
    public async Task<IActionResult> GetAllStations()
    {
        try
        {
            return Ok(await _stationService.GetAllStations());
        }
        catch (InvalidOperationException)
        {
            return NotFound(ErrorMessages.TransactionFailure);
        }
    }

    [HttpGet("raw/{server}")]
    public async Task<IActionResult> GetServerStations(int server)
    {
        try
        {
            return Ok(await _stationService.GetAllStationsFromServer(server));
        }
        catch (ArgumentOutOfRangeException)
        {
            return NotFound(ErrorMessages.ServerNotFound);
        }
    }

    [HttpGet("view")]
    public async Task<IActionResult> GetAllStationsView()
    {
        try
        {
            return Ok(await _stationService.GetAllStationModels());
        }
        catch (InvalidOperationException)
        {
            return NotFound(ErrorMessages.TransactionFailure);
        }
    }

    [HttpGet("view/{server}")]
    public async Task<IActionResult> GetServerStationsView(int server)
    {
        try
        {
            return Ok(await _stationService.GetAllStationModelsFromServer(server));
        }
        catch (ArgumentOutOfRangeException)
        {
            return NotFound(ErrorMessages.ServerNotFound);
        }
    }

    [HttpGet("view/coords")]
    public async Task<IActionResult> GetStationsFromLocation(decimal latitude,
        decimal longitude, int radius)
    {
        try
        {
            return Ok(await _stationService.GetStationsFromLocation(latitude, longitude, radius));
        }
        catch (ArgumentException)
        {
            return BadRequest(ErrorMessages.RadiusOutOfRange);
        }
        catch (InvalidOperationException)
        {
            return NotFound(ErrorMessages.TransactionFailure);
        }
    }
}