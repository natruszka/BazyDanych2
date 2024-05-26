using System.Data.SqlClient;
using System.Transactions;
using WeatherApp.Database;
using WeatherApp.Database.Entities;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public class LocationService : ILocationService
{
    private readonly WeatherDbContext _dbContext;
    private SqlConnection _connection;
    private List<string> _servers;

    public LocationService(WeatherDbContext dbContext)
    {
        _dbContext = dbContext;
        _connection = _dbContext.Get();
        _servers = _dbContext.Servers();
    }

    public async Task<Dictionary<string, List<Location>>> GetAllLocations()
    {
        var result = new Dictionary<string, List<Location>>();
        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            foreach (var server in _servers)
            {
                var locations = new List<Location>();
                SqlCommand command = new($"Select * from {server}.[WeatherDatabase].[dbo].[locations]",
                    _connection);
                await using var reader = await command.ExecuteReaderAsync();
                locations.AddRange(ReadLocationRange(reader));
                result.Add(server, locations);
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

    public async Task<List<Location>> GetLocationFromServer(int serverNum)
    {
        if (serverNum < 0 || serverNum >= _servers.Count)
            throw new ArgumentOutOfRangeException();
        var server = _servers[serverNum];
        SqlCommand command = new($"Select * from {server}.[WeatherDatabase].[dbo].[locations]", _connection);
        try
        {
            await using var reader = await command.ExecuteReaderAsync();
            return ReadLocationRange(reader);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private List<Location> ReadLocationRange(SqlDataReader reader)
    {
        var listLocations = new List<Location>();
        while (reader.Read())
        {
            listLocations.Add(new Location(
                Convert.ToInt32(reader["location_id"]),
                Convert.ToDecimal(reader["latitude"]),
                Convert.ToDecimal(reader["longitude"]),
                reader["city"].ToString()!,
                reader["country"].ToString()!,
                Convert.ToDecimal(reader["elevation"])));
        }

        return listLocations;
    }
}