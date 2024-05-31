using System.Data.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.DTOs;
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
            return BadRequest(ErrorMessages.ServerNotFound);
        }
    }
    [HttpPost("{server:int}")]
    public async Task<IActionResult> AddLocation(int server, [FromBody] LocationDto location)
    {
        try
        {
            await _locationService.AddLocation(server, location);
            return Ok();
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest(ErrorMessages.ServerNotFound);
        }
        catch (DbException)
        {
            return NotFound(ErrorMessages.TransactionFailure);
        }
    }
    [HttpGet("{server:int}/ids")]
    public async Task<IActionResult> GetAllLocationIds(int server)
    {
        try
        {
            return Ok(await _locationService.GetAllLocationIds(server));
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest(ErrorMessages.ServerNotFound);
        }
    }
}