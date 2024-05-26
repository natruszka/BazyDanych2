using System.Data.SqlClient;
using System.Transactions;
using WeatherApp.Database;
using WeatherApp.Database.Entities;
using WeatherApp.DTOs;
using WeatherApp.Resources;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public class WeatherService : IWeatherService
{
    private readonly WeatherDbContext _dbContext;
    private readonly SqlConnection _connection;
    private readonly List<string> _servers;
    private readonly IStationService _stationService;

    public WeatherService(WeatherDbContext dbContext)
    {
        _dbContext = dbContext;
        _connection = _dbContext.Get();
        _servers = _dbContext.Servers();
        _stationService = new StationService(_dbContext);
        AddUpsertProcedure();
    }

    private void AddUpsertProcedure()
    {
        try
        {
            SqlCommand command = new SqlCommand(SqlScripts.CheckIfUpsertProcedureExists, _connection);
            var result = command.ExecuteScalar();
            if (result != null && (int)result == 1)
                return;
            command = new SqlCommand(SqlScripts.UpsertProcedureScript, _connection);
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //swallow exception
        }
    }

    public async Task AggregateData()
    {
        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            var readings = await GetAllReadings();
            foreach (var reading in readings)
            {
                var newWeatherData = reading.Value
                    .GroupBy(r => new
                    {
                        r.StationId,
                        r.Timestamp.Year,
                        r.Timestamp.Month,
                        r.Timestamp.Day,
                        r.Timestamp.Hour
                    })
                    .Select(async g => new WeatherDataDto
                    {
                        ServerId = _servers.IndexOf(reading.Key),
                        LocationId = await GetLocationIdFromStation(_servers.IndexOf(reading.Key), g.Key.StationId),
                        Timestamp = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0),
                        Temperature = g.Average(r => r.Temperature),
                        MinTemperature = g.Min(r => r.Temperature),
                        MaxTemperature = g.Max(r => r.Temperature),
                        Humidity = g.Average(r => r.Humidity),
                        WindSpeed = g.Average(r => r.WindSpeed),
                        Precipitation = g.Average(r => r.Precipitation)
                    })
                    .ToList();
                await UpsertWeatherData(newWeatherData.Select(t => t.Result));
            }
            scope.Complete();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task ReAggregateData(int serverNum, int locationId, DateTime date)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var readings = await GetReadingsFromLocationAndDay(serverNum, locationId, DateOnly.FromDateTime(date));
        var newWeatherData = readings
            .GroupBy(r => new
            {
                r.StationId,
                r.Timestamp.Year,
                r.Timestamp.Month,
                r.Timestamp.Day,
                r.Timestamp.Hour
            })
            .Select(g => new WeatherDataDto
            {
                ServerId = serverNum,
                LocationId = locationId,
                Timestamp = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0),
                Temperature = g.Average(r => r.Temperature),
                MinTemperature = g.Min(r => r.Temperature),
                MaxTemperature = g.Max(r => r.Temperature),
                Humidity = g.Average(r => r.Humidity),
                WindSpeed = g.Average(r => r.WindSpeed),
                Precipitation = g.Average(r => r.Precipitation)
            })
            .ToList();
        await UpsertWeatherData(newWeatherData);
    }

    private async Task UpsertWeatherData(IEnumerable<WeatherDataDto> data)
    {
        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var weatherDataDto in data)
            {
                SqlCommand command = new(
                    $"EXEC dbo.UpsertWeatherData @Server_Id = {weatherDataDto.ServerId}, @Location_Id = {weatherDataDto.LocationId}, @Timestamp = '{weatherDataDto.Timestamp:yyyy/MM/dd HH:mm:ss}'," +
                    $" @Temperature = {weatherDataDto.Temperature}, @Min_Temperature = {weatherDataDto.MinTemperature}, @Max_Temperature = {weatherDataDto.MaxTemperature}," +
                    $" @Humidity = {weatherDataDto.Humidity}, @WindSpeed = {weatherDataDto.WindSpeed}, @Precipitation = {weatherDataDto.Precipitation}",
                    _connection);
                await command.ExecuteNonQueryAsync();
            }

            scope.Complete();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<WeatherData?> GetCurrentWeather(int serverNum, int locationId)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        try
        {
            SqlCommand command = new(
                $"Select * from [WeatherDatabase].[dbo].[weather_data]" +
                $" WHERE location_id = {locationId} " +
                $"AND server_id = {serverNum} " +
                $"AND CAST(timestamp AS date) = CAST(GETDATE() AS date) " +
                $"AND DATEPART(HOUR, timestamp) = DATEPART(HOUR, GETDATE()) " +
                $" ORDER BY timestamp DESC",
                _connection);
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new WeatherData
                {
                    DataId = reader.GetInt32(reader.GetOrdinal("data_id")),
                    ServerId = reader.GetInt32(reader.GetOrdinal("server_id")),
                    LocationId = reader.GetInt32(reader.GetOrdinal("location_id")),
                    Timestamp = reader.GetDateTime(reader.GetOrdinal("timestamp")),
                    Temperature = reader.GetDecimal(reader.GetOrdinal("temperature")),
                    MinTemperature = reader.GetDecimal(reader.GetOrdinal("min_temperature")),
                    MaxTemperature = reader.GetDecimal(reader.GetOrdinal("max_temperature")),
                    Humidity = reader.GetDecimal(reader.GetOrdinal("humidity")),
                    WindSpeed = reader.GetDecimal(reader.GetOrdinal("windspeed")),
                    Precipitation = reader.GetDecimal(reader.GetOrdinal("precipitation"))
                };
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<WeatherData>> GetWeatherDataFromDay(DateOnly date)
    {
        try
        {
            SqlCommand command = new($"Select * from [WeatherDatabase].[dbo].[weather_data]" +
                                     $" WHERE CAST(timestamp AS date) = '{date:yyyy/MM/dd}' ORDER BY timestamp DESC",
                _connection);
            await using var reader = await command.ExecuteReaderAsync();
            return ReadWeatherData(reader);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<WeatherData>> GetHistoricalWeather(DateOnly startDate,
        DateOnly endDate)
    {
        try
        {
            var readings = new List<WeatherReading>();
            SqlCommand command = new($"Select * from [WeatherDatabase].[dbo].[weather_data]" +
                                     $"WHERE CAST(timestamp AS date) BETWEEN '{startDate:yyyy/MM/dd}' AND '{endDate:yyyy/MM/dd}' ORDER BY timestamp DESC",
                _connection);
            await using var reader = await command.ExecuteReaderAsync();
            return ReadWeatherData(reader);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Dictionary<string, List<WeatherReading>>> GetAllReadings()
    {
        var result = new Dictionary<string, List<WeatherReading>>();
        try
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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

    public async Task<Dictionary<string, List<WeatherReading>>> GetReadingsFromDay(DateOnly date)
    {
        var result = new Dictionary<string, List<WeatherReading>>();

        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var server in _servers)
            {
                var readings = new List<WeatherReading>();
                SqlCommand command = new($"Select * from {server}.[WeatherDatabase].[dbo].[weather_readings]" +
                                         $" WHERE CAST(timestamp AS date) = '{date:yyyy/MM/dd}'",
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
        DateOnly date)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        try
        {
            SqlCommand command = new($"Select * from {server}.[WeatherDatabase].[dbo].[weather_readings]" +
                                     $" WHERE CAST(timestamp AS date) = '{date:yyyy/MM/dd}' AND station_id = {stationId}",
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

    public async Task<List<WeatherReading>> GetReadingsFromLocationAndDay(int serverNum, int locationId,
        DateOnly date)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        try
        {
            SqlCommand command = new(
                $"Select wr.reading_id, wr.station_id, wr.timestamp, wr.temperature, wr.humidity, wr.windspeed, wr.precipitation" +
                $" from {server}.[WeatherDatabase].[dbo].[weather_readings] wr join dbo.stations s on wr.station_id = s.station_id " +
                $" WHERE CAST(timestamp AS date) = '{date:yyyy/MM/dd}' AND location_id = {locationId}",
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

    public async Task AddReading(int serverNum, WeatherReadingDto weatherReadingDto)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException(ErrorMessages.ServerNotFound);
        var server = _servers[serverNum];
        var locationId = await GetLocationIdFromStation(serverNum, weatherReadingDto.StationId);
        if (locationId == -1)
        {
            throw new ArgumentOutOfRangeException("Location does not exist");
        }

        try
        {
            SqlCommand command = new($"INSERT INTO {server}.[WeatherDatabase].[dbo].[weather_readings]" +
                                     $" (station_id, timestamp, temperature, humidity, windspeed, precipitation)" +
                                     $" VALUES ({weatherReadingDto.StationId}, '{weatherReadingDto.Timestamp:yyyy/MM/dd HH:mm:ss}'," +
                                     $" {weatherReadingDto.Temperature}, {weatherReadingDto.Humidity}, {weatherReadingDto.WindSpeed}, {weatherReadingDto.Precipitation})",
                _connection);
            await command.ExecuteNonQueryAsync();
            await ReAggregateData(serverNum, locationId, weatherReadingDto.Timestamp);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<int> GetLocationIdFromStation(int serverNum, int stationId)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        try
        {
            SqlCommand command = new(
                $"Select location_id" +
                $" from {server}.[WeatherDatabase].[dbo].[stations] " +
                $" WHERE station_id = {stationId}",
                _connection);
            await using var reader = await command.ExecuteReaderAsync();
            var location = reader.Read() ? reader.GetInt32(0) : -1;
            return location;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SeedDatabaseForGivenTimeServerAndLocation(int serverNum, int locationId, DateOnly date)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        try
        {
            var stations = await _stationService.GetAllStationsFromServer(serverNum);
            stations = stations.Where(x => x.LocationId == locationId).ToList();
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                foreach (var station in stations)
                {
                    var weatherReadingDto = new WeatherReadingDto
                    {
                        StationId = station.StationId,
                        Timestamp = date.ToDateTime(
                            new TimeOnly(random.Next(0, 24), random.Next(0, 60), random.Next(0, 60))),
                        Temperature = random.Next(-20, 40),
                        Humidity = random.Next(0, 100),
                        WindSpeed = random.Next(0, 100),
                        Precipitation = random.Next(0, 100)
                    };
                    await AddReading(serverNum, weatherReadingDto);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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

    private List<WeatherData> ReadWeatherData(SqlDataReader reader)
    {
        var listWeatherData = new List<WeatherData>();
        while (reader.Read())
        {
            listWeatherData.Add(new WeatherData
            {
                DataId = reader.GetInt32(0),
                ServerId = reader.GetInt32(1),
                LocationId = reader.GetInt32(2),
                Timestamp = reader.GetDateTime(3),
                Temperature = reader.GetDecimal(4),
                MinTemperature = reader.GetDecimal(5),
                MaxTemperature = reader.GetDecimal(6),
                Humidity = reader.GetDecimal(7),
                WindSpeed = reader.GetDecimal(8),
                Precipitation = reader.GetDecimal(9)
            });
        }

        return listWeatherData;
    }
}