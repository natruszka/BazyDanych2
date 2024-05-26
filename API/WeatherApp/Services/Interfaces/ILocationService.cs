using WeatherApp.Database.Entities;

namespace WeatherApp.Services.Interfaces;

public interface ILocationService
{
    Task<Dictionary<string, List<Location>>> GetAllLocations();
    Task<List<Location>> GetLocationFromServer(int serverNum);
}