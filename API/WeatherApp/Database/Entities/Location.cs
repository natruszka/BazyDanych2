namespace WeatherApp.Database.Entities;

public class Location
{
    public int LocationId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public decimal Elevation { get; set; }

    public Location()
    {
        
    }

    public Location(int locationId, decimal latitude, decimal longitude, string city, string country, decimal elevation)
    {
        LocationId = locationId;
        Latitude = latitude;
        Longitude = longitude;
        City = city;
        Country = country;
        Elevation = elevation;
    }
}