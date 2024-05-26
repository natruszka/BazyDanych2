using System.Data.SqlClient;
using System.Transactions;
using WeatherApp.Database;
using WeatherApp.Database.Entities;

namespace WeatherApp.Services;

public class WeatherService
{
    private readonly WeatherDbContext _dbContext;
    private SqlConnection _connection;
    private List<string> _servers;

    public WeatherService(WeatherDbContext dbContext)
    {
        _dbContext = dbContext;
        _connection = _dbContext.Get();
        _servers = _dbContext.Servers();
        _servers = _dbContext.Servers();
    }

    public WeatherData GetCurrentWeather()
    {
        throw new NotImplementedException();
    }

    public List<WeatherData> GetWeatherDataFromDay()
    {
        throw new NotImplementedException();
    }

    public List<WeatherData> GetHistoricalWeather(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<Dictionary<string, List<WeatherReading>>> GetAllReadings()
    {
        var result = new Dictionary<string, List<WeatherReading>>();

        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var server in _servers)
            {
                var readings = new List<WeatherReading>();
                SqlCommand command = new($"Select * from {server}.[WeatherDatabase].[dbo].[weather_readings]" +
                                         $" ORDER BY timestamp DESC",
                    _connection);
                await using var reader = await command.ExecuteReaderAsync();
                readings.AddRange(ReadWeatherReadingRange(reader));
                result.Add(server, readings);
            }

            scope.Complete();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Dictionary<string, List<WeatherReading>>> GetReadingsFromDay(DateTime date)
    {
        var result = new Dictionary<string, List<WeatherReading>>();

        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var server in _servers)
            {
                var readings = new List<WeatherReading>();
                SqlCommand command = new($"Select * from {server}.[WeatherDatabase].[dbo].[weather_readings]" +
                                         $" WHERE CAST(timestamp AS date) = '{date.Date:yyyy/MM/dd}'",
                    _connection);
                await using var reader = await command.ExecuteReaderAsync();
                readings.AddRange(ReadWeatherReadingRange(reader));
                result.Add(server, readings);
            }

            scope.Complete();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<WeatherReading>> GetReadingsFromStationAndDay(int stationId, int serverNum,
        DateTimeOffset date)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        try
        {
            SqlCommand command = new($"Select * from {server}.[WeatherDatabase].[dbo].[weather_readings]" +
                                     $" WHERE CAST(timestamp AS date) = '{date.Date:yyyy/MM/dd}' AND station_id = {stationId}",
                _connection);
            await using var reader = await command.ExecuteReaderAsync();
            return ReadWeatherReadingRange(reader);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void SeedDatabaseForGivenTimeAndLocation()
    {
        throw new NotImplementedException();
    }

    private List<WeatherReading> ReadWeatherReadingRange(SqlDataReader reader)
    {
        var listWeatherReading = new List<WeatherReading>();
        while (reader.Read())
        {
            listWeatherReading.Add(new WeatherReading
            {
                ReadingId = reader.GetInt32(0),
                StationId = reader.GetInt32(1),
                Timestamp = reader.GetDateTime(2),
                Temperature = reader.GetDecimal(3),
                Humidity = reader.GetDecimal(4),
                WindSpeed = reader.GetDecimal(5),
                Precipitation = reader.GetDecimal(6)
            });
        }

        return listWeatherReading;
    }
}