using System.Data.SqlClient;

namespace WeatherApp.Database;

public class WeatherDbContext
{
    private SqlConnection _connection { get; }
    private List<String> _servers;

    public WeatherDbContext(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("WeatherDatabase"));
        _servers = configuration.GetSection("servers").Get < List<string>>() ?? throw new InvalidOperationException();
        _connection.Open();
    }

    public SqlConnection Get()
    {
        return _connection;
    }

    public List<string> Servers()
    {
        return _servers;
    }

    ~WeatherDbContext()
    {
        _connection.Close();
    }
}