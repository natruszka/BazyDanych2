using WeatherApp.Database;

namespace WeatherApp.Services;

public class ServerService(WeatherDbContext dbContext)
{
    private readonly WeatherDbContext _dbContext = dbContext;

    public async Task<Dictionary<string, int>> GetAllServers()
    {
        return _dbContext.Servers().ToDictionary(x => x, x => _dbContext.Servers().IndexOf(x));
    }
}