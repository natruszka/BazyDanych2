using WeatherApp.Database.Entities;
using WeatherApp.DTOs;

namespace WeatherApp.Services.Interfaces;

public interface ILocationService
{
    Task<Dictionary<string, List<Location>>> GetAllLocations();
    Task<List<Location>> GetLocationFromServer(int serverNum);
    Task AddLocation(int serverNum, LocationDto location);
    Task<List<int>> GetAllLocationIds(int serverNum);
}