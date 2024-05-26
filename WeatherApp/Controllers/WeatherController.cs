using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.DTOs;
using WeatherApp.Resources;
using WeatherApp.Services;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Controllers;

[Route("weather")]
[ApiController]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    private readonly IWeatherService _weatherService = weatherService;

    [HttpPost("aggregate")]
    public async Task<IActionResult> AggregateWeatherData()
    {
        try
        {
            await _weatherService.AggregateData();
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return BadRequest(ErrorMessages.TransactionFailure);
        }
    }

    [HttpGet("{serverNum:int}/{locationId:int}")]
    public async Task<IActionResult> Get(int serverNum, int locationId)
    {
        try
        {
            return Ok(await _weatherService.GetCurrentWeather(serverNum, locationId));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("day")]
    public async Task<IActionResult> GetWeatherFromDay(DateTime date)
    {
        try
        {
            return Ok(await _weatherService.GetWeatherDataFromDay(DateOnly.FromDateTime(date.Date)));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("historical")]
    public async Task<IActionResult> GetHistoricalWeather(DateTime startDate, DateTime endDate)
    {
        try
        {
            return Ok(await _weatherService.GetHistoricalWeather(DateOnly.FromDateTime(startDate.Date),
                DateOnly.FromDateTime(endDate.Date)));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("readings")]
    public async Task<IActionResult> GetAllReadings()
    {
        try
        {
            return Ok(await _weatherService.GetAllReadings());
        }
        catch (InvalidOperationException)
        {
            return NotFound(ErrorMessages.TransactionFailure);
        }
    }

    [HttpGet("readings/day")]
    public async Task<IActionResult> GetReadingsFromDay(DateTime date)
    {
        try
        {
            return Ok(await _weatherService.GetReadingsFromDay(DateOnly.FromDateTime(date.Date)));
        }
        catch (InvalidOperationException)
        {
            return NotFound(ErrorMessages.TransactionFailure);
        }
    }

    [HttpGet("readings/day/{serverNum}/station/{stationId}")]
    public async Task<IActionResult> GetReadingsFromStationAndDay(DateTime date, int serverNum, int stationId)
    {
        try
        {
            return Ok(await _weatherService.GetReadingsFromStationAndDay(stationId, serverNum,
                DateOnly.FromDateTime(date.Date)));
        }
        catch(ArgumentOutOfRangeException)
        {
            return BadRequest(ErrorMessages.ServerNotFound);
        }
    }

    [HttpGet("readings/day/{serverNum}/location/{locationId}")]
    public async Task<IActionResult> GetReadingsFromLocationAndDay(DateTime date, int serverNum, int locationId)
    {
        try
        {
            return Ok(await _weatherService.GetReadingsFromLocationAndDay(serverNum, locationId,
                DateOnly.FromDateTime(date.Date)));
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest(ErrorMessages.ServerNotFound);
        }
    }

    [HttpPost("readings/{serverNum:int}")]
    public async Task<IActionResult> AddReading(int serverNum, [FromBody] WeatherReadingDto weatherReadingDto)
    {
        try
        {
            await _weatherService.AddReading(serverNum, weatherReadingDto);
            return Ok();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
        catch (DbException)
        {
            return BadRequest(ErrorMessages.TransactionFailure);
        }
    }

    [HttpPost("seed/{serverNum:int}/{locationId:int}")]
    public async Task<IActionResult> SeedDatabaseForGivenTimeServerAndLocation(int serverNum, int locationId,
        DateTime date)
    {
        try
        {
            await _weatherService.SeedDatabaseForGivenTimeServerAndLocation(serverNum, locationId,
                DateOnly.FromDateTime(date.Date));
            return Ok();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
        catch (DbException)
        {
            return BadRequest(ErrorMessages.TransactionFailure);
        }
    }
}