using System.Data.SqlClient;

namespace WeatherApp.Database;

public class WeatherDbContext
{
    private SqlConnection _connection { get; }
    private List<String> _servers = new List<string> { "[LAPTOP-TKOQ32I7\\SQLEXPRESS]", "[LAPTOP-TKOQ32I7\\MSSQLSERVER1]", "[LAPTOP-TKOQ32I7\\MSSQLSERVER2]" };

    public WeatherDbContext()
    {
        _connection = new SqlConnection(@"
            Data Source=LAPTOP-TKOQ32I7\SQLEXPRESS;
            Database=WeatherDatabase;
            Trusted_Connection=True;
            MultipleActiveResultSets=True");
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