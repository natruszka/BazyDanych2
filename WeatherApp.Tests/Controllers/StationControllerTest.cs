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
using WeatherApp.Models;
using WeatherApp.Resources;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Tests.Controllers;

[TestFixture]
[TestOf(typeof(StationController))]
public class StationControllerTest
{
    private StationController _stationController;
    private IStationService _stationService;
    [SetUp]
    public void SetUp()
    {
        _stationService = Substitute.For<IStationService>();
        _stationController = new StationController(_stationService);
    }
    [Test]
    public async Task GetAllStations_ReturnsOk()
    {
        _stationService.GetAllStations().Returns(new Dictionary<string, List<Station>>());

        var result = await _stationController.GetAllStations() as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }

    [Test]
    public async Task GetAllStations_ReturnsNotFound()
    {
        _stationService.GetAllStations().ThrowsAsync(new InvalidOperationException());

        var result = await _stationController.GetAllStations() as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.TransactionFailure, result.Value);
    }
    [Test]
    public async Task GetServerStations_ReturnsOk()
    {
        var serverNum = 0;
        _stationService.GetAllStationsFromServer(serverNum).Returns(new List<Station>());

        var result = await _stationController.GetServerStations(serverNum) as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }
    [Test]
    public async Task GetServerStations_ReturnsNotFound()
    {
        var serverNum = 0;
        _stationService.GetAllStationsFromServer(serverNum).ThrowsAsync(new ArgumentOutOfRangeException());

        var result = await _stationController.GetServerStations(serverNum) as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.ServerNotFound, result.Value);

    }
    [Test]
    public async Task GetAllStationsView_ReturnsOk()
    {
        _stationService.GetAllStationModels().Returns(new Dictionary<string, List<StationModel>>());

        var result = await _stationController.GetAllStationsView() as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }
    [Test]
    public async Task GetAllStationsView_ReturnsNotFound()
    {
        _stationService.GetAllStationModels().ThrowsAsync(new InvalidOperationException());

        var result = await _stationController.GetAllStationsView() as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.TransactionFailure, result.Value);
    }
    [Test]
    public async Task GetServerStationsView_ReturnsOk()
    {
        var serverNum = 0;
        _stationService.GetAllStationModelsFromServer(serverNum).Returns(new List<StationModel>());

        var result = await _stationController.GetServerStationsView(serverNum) as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }
    [Test]
    public async Task GetServerStationsView_ReturnsNotFound()
    {
        var serverNum = 0;
        _stationService.GetAllStationModelsFromServer(serverNum).ThrowsAsync(new ArgumentOutOfRangeException());

        var result = await _stationController.GetServerStationsView(serverNum) as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.ServerNotFound, result.Value);

    }
    [Test]
    public async Task GetStationsFromLocation_ReturnsOk()
    {
        decimal latitude = 0;
        decimal longitude = 0;
        int radius = 10;
        _stationService.GetStationsFromLocation(latitude, longitude, radius).Returns(new Dictionary<string, List<StationModel>>());

        var result = await _stationController.GetStationsFromLocation(latitude, longitude, radius) as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }
    [Test]
    public async Task GetStationsFromLocation_ReturnsBadRequest()
    {
        decimal latitude = 0;
        decimal longitude = 0;
        int radius = -10;
        _stationService.GetStationsFromLocation(latitude, longitude, radius).ThrowsAsync(new ArgumentException());

        var result = await _stationController.GetStationsFromLocation(latitude, longitude, radius) as BadRequestObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(400, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.RadiusOutOfRange, result.Value);
    }
    [Test]
    public async Task GetStationsFromLocation_ReturnsNotFound()
    {
        decimal latitude = 0;
        decimal longitude = 0;
        int radius = 10;
        _stationService.GetStationsFromLocation(latitude, longitude, radius).ThrowsAsync(new InvalidOperationException());

        var result = await _stationController.GetStationsFromLocation(latitude, longitude, radius) as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.TransactionFailure, result.Value);
    }
}