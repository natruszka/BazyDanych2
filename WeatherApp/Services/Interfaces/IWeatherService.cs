using WeatherApp.Database.Entities;
using WeatherApp.DTOs;

namespace WeatherApp.Services.Interfaces;

public interface IWeatherService
{
    Task AggregateData();
    Task<WeatherData?> GetCurrentWeather(int serverNum, int locationId);
    Task<List<WeatherData>> GetWeatherDataFromDay(DateOnly date);

    Task<List<WeatherData>> GetHistoricalWeather(DateOnly startDate,
        DateOnly endDate);

    Task<Dictionary<string, List<WeatherReading>>> GetAllReadings();
    Task<Dictionary<string, List<WeatherReading>>> GetReadingsFromDay(DateOnly date);

    Task<List<WeatherReading>> GetReadingsFromStationAndDay(int stationId, int serverNum,
        DateOnly date);

    Task<List<WeatherReading>> GetReadingsFromLocationAndDay(int serverNum, int locationId,
        DateOnly date);

    Task AddReading(int serverNum, WeatherReadingDto weatherReadingDto);
    Task SeedDatabaseForGivenTimeServerAndLocation(int serverNum, int locationId, DateOnly date );
}