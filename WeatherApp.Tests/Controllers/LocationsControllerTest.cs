using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using WeatherApp.Controllers;
using WeatherApp.Database.Entities;
using WeatherApp.DTOs;
using WeatherApp.Resources;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Tests.Controllers;

[TestFixture]
[TestOf(typeof(LocationsController))]
public class LocationsControllerTest
{
    private LocationsController _controller;
    private ILocationService _locationService;
    [SetUp]
    public void SetUp()
    {
        _locationService = Substitute.For<ILocationService>();
        _controller = new LocationsController(_locationService);
    }
    [Test]
    public async Task GetAllLocations_ReturnsOk()
    {
        _locationService.GetAllLocations().Returns(new Dictionary<string, List<Location>>());

        var result = await _controller.GetAllLocations() as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }
    [Test]
    public async Task GetAllLocations_ReturnsNotFound()
    {
        _locationService.GetAllLocations().ThrowsAsync(new InvalidOperationException());

        var result = await _controller.GetAllLocations() as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.TransactionFailure, result.Value);
    }

    [Test]
    public async Task GetLocationFromServer_ReturnsOk()
    {
        var serverNum = 0;
        _locationService.GetLocationFromServer(serverNum).Returns(new List<Location>());

        var result = await _controller.GetLocationFromServer(serverNum) as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }

    [Test]
    public async Task GetLocationFromServer_ReturnsBadRequest()
    {
        var serverNum = 0;
        _locationService.GetLocationFromServer(serverNum).ThrowsAsync(new ArgumentOutOfRangeException());

        var result = await _controller.GetLocationFromServer(serverNum) as BadRequestObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(400, result!.StatusCode);
        ClassicAssert.AreEqual(ErrorMessages.ServerNotFound, result.Value);
    }
    [Test]
    public async Task AddLocation_ReturnsOk()
    {
        var serverNum = 0;
        var location = new LocationDto();
        _locationService.AddLocation(serverNum, Arg.Any<LocationDto>()).Returns(Task.CompletedTask);

        var result = await _controller.AddLocation(serverNum, location) as OkResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
    }
    [Test]
    public async Task AddLocation_ReturnsBadRequest()
    {
        var serverNum = 0;
        var location = new LocationDto();
        _locationService.AddLocation(serverNum, Arg.Any<LocationDto>()).ThrowsAsync(new ArgumentOutOfRangeException());

        var result = await _controller.AddLocation(serverNum, location) as BadRequestObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(400, result!.StatusCode);
    }
}