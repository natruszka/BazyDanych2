using WeatherApp.Database.Entities;
using WeatherApp.DTOs;
using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces;

public interface IStationService
{
    Task<Dictionary<string, List<Station>>> GetAllStations();
    Task<List<Station>> GetAllStationsFromServer(int serverNum);
    Task<Dictionary<string, List<StationModel>>> GetAllStationModels();
    Task<List<StationModel>> GetAllStationModelsFromServer(int serverNum);

    Task<Dictionary<string, List<StationModel>>> GetStationsFromLocation(decimal latitude,
        decimal longitude, int radius);

    Task AddStation(int server, StationDto station);
}