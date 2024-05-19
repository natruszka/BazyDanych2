using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;

namespace WeatherApp.Controllers;
[Route("weather")]
[ApiController]
public class WeatherController(WeatherService weatherService) : ControllerBase
{
    private readonly WeatherService _weatherService = weatherService;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
    [HttpGet("readings")]
    public async Task<IActionResult> GetAllReadings()
    {
        return Ok(await _weatherService.GetAllReadings());
    }
    [HttpGet("readings/day")]
    public async Task<IActionResult> GetReadingsFromDay(DateTime date)
    {
        return Ok(await _weatherService.GetReadingsFromDay(date));
    }
    [HttpGet("readings/day/{serverNum}/{stationId}")]
    public async Task<IActionResult> GetReadingsFromDay(DateTime date, int serverNum, int stationId)
    {
        return Ok(await _weatherService.GetReadingsFromStationAndDay(stationId, serverNum,date));
    }
}