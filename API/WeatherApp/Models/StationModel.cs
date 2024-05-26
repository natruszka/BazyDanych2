namespace WeatherApp.Models;

public class StationModel
{
    public int StationId { get; set; }
    public int LocationId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;

    public StationModel()
    {
        
    }

    public StationModel(int stationId, int locationId, decimal latitude, decimal longitude, string city, string country)
    {
        StationId = stationId;
        LocationId = locationId;
        Latitude = latitude;
        Longitude = longitude;
        City = city;
        Country = country;
    }
}