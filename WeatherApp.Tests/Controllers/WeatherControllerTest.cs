using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using WeatherApp.Controllers;
using WeatherApp.Database.Entities;
using WeatherApp.DTOs;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Tests.Controllers;

[TestFixture]
[TestOf(typeof(WeatherController))]
public class WeatherControllerTest
{
    private WeatherController _weatherController = null!;
    private IWeatherService _weatherService = null!;
    [SetUp]
    public void Setup()
    {
        _weatherService = Substitute.For<IWeatherService>();
        _weatherController = new WeatherController(_weatherService);
    }
    [Test]
    public async Task TestAggregateWeatherData_ReturnsOkResult()
    {
        _weatherService.AggregateData().Returns(Task.CompletedTask);
        var result = await _weatherController.AggregateWeatherData() as OkResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestAggregateWeatherData_ReturnsBadRequestResult()
    {
        _weatherService.AggregateData().ThrowsAsync(new InvalidOperationException());
        var result = await _weatherController.AggregateWeatherData() as BadRequestObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 400);
    }
    [Test]
    public async Task TestGet_ReturnsOkResult()
    {
        _weatherService.GetCurrentWeather(1, 1).Returns(new WeatherData());
        var result = await _weatherController.Get(1, 1) as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestGet_ReturnsNotFoundResult()
    {
        _weatherService.GetCurrentWeather(1, 1).ThrowsAsync(new Exception());
        var result = await _weatherController.Get(1, 1) as NotFoundObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 404);
    }
    [Test]
    public async Task TestGetWeatherFromDay_ReturnsOkResult()
    {
        _weatherService.GetWeatherDataFromDay(DateOnly.FromDateTime(DateTime.Now.Date)).Returns(new List<WeatherData>());
        var result = await _weatherController.GetWeatherFromDay(DateTime.Now) as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestGetWeatherFromDay_ReturnsNotFoundResult()
    {
        _weatherService.GetWeatherDataFromDay(DateOnly.FromDateTime(DateTime.Now.Date)).ThrowsAsync(new Exception());
        var result = await _weatherController.GetWeatherFromDay(DateTime.Now) as NotFoundObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 404);
    }
    [Test]
    public async Task TestGetHistoricalWeather_ReturnsOkResult()
    {
        _weatherService.GetHistoricalWeather(DateOnly.FromDateTime(DateTime.Now.Date), DateOnly.FromDateTime(DateTime.Now.Date)).Returns(new List<WeatherData>());
        var result = await _weatherController.GetHistoricalWeather(DateTime.Now, DateTime.Now) as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestGetHistoricalWeather_ReturnsNotFoundResult()
    {
        _weatherService.GetHistoricalWeather(DateOnly.FromDateTime(DateTime.Now.Date), DateOnly.FromDateTime(DateTime.Now.Date)).ThrowsAsync(new Exception());
        var result = await _weatherController.GetHistoricalWeather(DateTime.Now, DateTime.Now) as NotFoundObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 404);
    }
    [Test]
    public async Task TestGetAllReadings_ReturnsOkResult()
    {
        _weatherService.GetAllReadings().Returns(new Dictionary<string, List<WeatherReading>>());
        var result = await _weatherController.GetAllReadings() as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestGetAllReadings_ReturnsNotFoundResult()
    {
        _weatherService.GetAllReadings().ThrowsAsync(new InvalidOperationException());
        var result = await _weatherController.GetAllReadings() as NotFoundObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 404);
    }
    [Test]
    public async Task TestGetReadingsFromDay_ReturnsOkResult()
    {
        _weatherService.GetReadingsFromDay(DateOnly.FromDateTime(DateTime.Now.Date)).Returns(new Dictionary<string, List<WeatherReading>>());
        var result = await _weatherController.GetReadingsFromDay(DateTime.Now) as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestGetReadingsFromDay_ReturnsNotFoundResult()
    {
        _weatherService.GetReadingsFromDay(DateOnly.FromDateTime(DateTime.Now.Date)).ThrowsAsync(new InvalidOperationException());
        var result = await _weatherController.GetReadingsFromDay(DateTime.Now) as NotFoundObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 404);
    }
    [Test]
    public async Task TestGetReadingsFromStationAndDay_ReturnsOkResult()
    {
        _weatherService.GetReadingsFromStationAndDay(1, 1, DateOnly.FromDateTime(DateTime.Now.Date)).Returns(new List<WeatherReading>());
        var result = await _weatherController.GetReadingsFromStationAndDay(DateTime.Now,1, 1) as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestGetReadingsFromStationAndDay_ReturnsBadRequestResult()
    {
        _weatherService.GetReadingsFromStationAndDay(1, 1, DateOnly.FromDateTime(DateTime.Now.Date)).ThrowsAsync(new ArgumentOutOfRangeException());
        var result = await _weatherController.GetReadingsFromStationAndDay(DateTime.Now, 1, 1) as BadRequestObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 400);
    }
    [Test]
    public async Task TestGetReadingsFromLocationAndDay_ReturnsOkResult()
    {
        _weatherService.GetReadingsFromLocationAndDay(1, 1, DateOnly.FromDateTime(DateTime.Now.Date)).Returns(new List<WeatherReading>());
        var result = await _weatherController.GetReadingsFromLocationAndDay(DateTime.Now, 1, 1) as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestGetReadingsFromLocationAndDay_ReturnsBadRequestResult()
    {
        _weatherService.GetReadingsFromLocationAndDay(1, 1, DateOnly.FromDateTime(DateTime.Now.Date)).ThrowsAsync(new ArgumentOutOfRangeException());
        var result = await _weatherController.GetReadingsFromLocationAndDay(DateTime.Now, 1, 1) as BadRequestObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 400);
    }
    [Test]
    public async Task TestAddReading_ReturnsOkResult()
    {
        _weatherService.AddReading(1, new WeatherReadingDto()).Returns(Task.CompletedTask);
        var result = await _weatherController.AddReading(1, new WeatherReadingDto()) as OkResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestAddReading_ReturnsBadRequestResult()
    {
        _weatherService.AddReading(1,Arg.Any<WeatherReadingDto>()).ThrowsAsync(new ArgumentOutOfRangeException());
        var result = await _weatherController.AddReading(1, new WeatherReadingDto()) as BadRequestObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 400);
    }
    [Test]
    public async Task TestSeedDatabaseForGivenTimeServerAndLocation_ReturnsOkResult()
    {
        _weatherService.SeedDatabaseForGivenTimeServerAndLocation(1, 1, DateOnly.FromDateTime(DateTime.Now.Date)).Returns(Task.CompletedTask);
        var result = await _weatherController.SeedDatabaseForGivenTimeServerAndLocation(1, 1, DateTime.Now) as OkResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 200);
    }
    [Test]
    public async Task TestSeedDatabaseForGivenTimeServerAndLocation_ReturnsBadRequestResult()
    {
        _weatherService.SeedDatabaseForGivenTimeServerAndLocation(1, 1, DateOnly.FromDateTime(DateTime.Now.Date)).ThrowsAsync(new ArgumentOutOfRangeException());
        var result = await _weatherController.SeedDatabaseForGivenTimeServerAndLocation(1, 1, DateTime.Now) as BadRequestObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(result!.StatusCode, 400);
    }
    
}