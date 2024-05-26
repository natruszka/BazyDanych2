using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.DTOs;
using WeatherApp.Resources;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Controllers;

[Route("stations")]
[ApiController]
public class StationController : ControllerBase
{
    private IStationService _stationService = null!;

    public StationController(IStationService stationService)
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
            return BadRequest(ErrorMessages.ServerNotFound);
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
            return BadRequest(ErrorMessages.ServerNotFound);
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
    [HttpPost("{server:int}")]
    public async Task<IActionResult> AddStation(int server, [FromBody] StationDto station)
    {
        try
        {
            await _stationService.AddStation(server, station);
            return Ok();
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest(ErrorMessages.ServerNotFound);
        }
        catch (DbException)
        {
            return NotFound(ErrorMessages.LocationNotFound);
        }
    }
}