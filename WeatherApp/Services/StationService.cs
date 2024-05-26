using System.Data.SqlClient;
using System.Transactions;
using WeatherApp.Database;
using WeatherApp.Database.Entities;
using WeatherApp.DTOs;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public class StationService : IStationService
{
    private readonly WeatherDbContext _dbContext;
    private SqlConnection _connection;
    private List<string> _servers;

    public StationService(WeatherDbContext dbContext)
    {
        _dbContext = dbContext;
        _connection = _dbContext.Get();
        _servers = _dbContext.Servers();
    }

    public async Task<Dictionary<string, List<Station>>> GetAllStations()
    {
        var result = new Dictionary<string, List<Station>>();
        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var server in _servers)
            {
                var stations = new List<Station>();
                SqlCommand command = new($"SELECT * FROM {server}.[WeatherDatabase].[dbo].[stations]", _connection);
                await using var reader = await command.ExecuteReaderAsync();
                stations.AddRange(ReadStationRange(reader));
                result.Add(server, stations);
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

    public async Task<List<Station>> GetAllStationsFromServer(int serverNum)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        try
        {
            SqlCommand command = new($"SELECT * FROM {server}.[WeatherDatabase].[dbo].[stations]", _connection);
            await using var reader = await command.ExecuteReaderAsync();
            return ReadStationRange(reader);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Dictionary<string, List<StationModel>>> GetAllStationModels()
    {
        var result = new Dictionary<string, List<StationModel>>();
        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var server in _servers)
            {
                var stations = new List<StationModel>();
                SqlCommand command = new(
                    "SELECT s.station_id, s.location_id, s.latitude, s.longitude, l.city, l.country " +
                    $"FROM {server}.[WeatherDatabase].[dbo].[stations] s " +
                    $"JOIN {server}.[WeatherDatabase].[dbo].[locations] l ON s.location_id = l.location_id",
                    _connection);
                await using var reader = await command.ExecuteReaderAsync();
                stations.AddRange(ReadStationModelRange(reader));
                result.Add(server, stations);
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

    public async Task<List<StationModel>> GetAllStationModelsFromServer(int serverNum)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        try
        {
            SqlCommand command = new("SELECT s.station_id, s.location_id, s.latitude, s.longitude, l.city, l.country " +
                                     $"FROM {server}.[WeatherDatabase].[dbo].[stations] s " +
                                     $"JOIN {server}.[WeatherDatabase].[dbo].[locations] l ON s.location_id = l.location_id",
                _connection);
            await using var reader = await command.ExecuteReaderAsync();
            return ReadStationModelRange(reader);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Dictionary<string, List<StationModel>>> GetStationsFromLocation(decimal latitude,
        decimal longitude, int radius)
    {
        if (radius < 0)
            throw new ArgumentException();
        var minLatitude = latitude - radius;
        var maxLatitude = latitude + radius;
        var minLongitude = longitude - radius;
        var maxLongitude = longitude + radius;
        var result = new Dictionary<string, List<StationModel>>();
        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var server in _servers)
            {
                var stations = new List<StationModel>();
                SqlCommand command = new(
                    "SELECT s.station_id, s.location_id, s.latitude, s.longitude, l.city, l.country " +
                    $"FROM {server}.[WeatherDatabase].[dbo].[stations] s " +
                    $"JOIN {server}.[WeatherDatabase].[dbo].[locations] l ON s.location_id = l.location_id " +
                    $"WHERE s.latitude BETWEEN {minLatitude} AND {maxLatitude} AND s.longitude BETWEEN {minLongitude} AND {maxLongitude};",
                    _connection);
                await using var reader = await command.ExecuteReaderAsync();
                stations.AddRange(ReadStationModelRange(reader));
                result.Add(server, stations);
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

    public async Task AddStation(int serverNum, StationDto station)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        SqlCommand command = new(
            $"INSERT INTO {server}.[WeatherDatabase].[dbo].[stations] (location_id, latitude, longitude) " +
            "VALUES (@location_id, @latitude, @longitude);",
            _connection);
        command.Parameters.AddWithValue("@location_id", station.LocationId);
        command.Parameters.AddWithValue("@latitude", station.Latitude);
        command.Parameters.AddWithValue("@longitude", station.Longitude);
        await command.ExecuteNonQueryAsync();
    }

    private List<Station> ReadStationRange(SqlDataReader reader)
    {
        var listStation = new List<Station>();
        while (reader.Read())
        {
            listStation.Add(new Station(
                Convert.ToInt32(reader["station_id"]),
                Convert.ToInt32(reader["location_id"]),
                Convert.ToDecimal(reader["latitude"]),
                Convert.ToDecimal(reader["longitude"])));
        }

        return listStation;
    }

    private List<StationModel> ReadStationModelRange(SqlDataReader reader)
    {
        var listStationModel = new List<StationModel>();
        while (reader.Read())
        {
            listStationModel.Add(new StationModel(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetDecimal(2),
                reader.GetDecimal(3),
                reader.GetString(4),
                reader.GetString(5)));
        }

        return listStationModel;
    }
}