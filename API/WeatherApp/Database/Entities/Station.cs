namespace WeatherApp.Database.Entities;

public class Station
{
    public int StationId { get; set; }
    public int LocationId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public Station()
    {
            
    }

    public Station(int stationId, int locationId, decimal latitude, decimal longitude)
    {
        StationId = stationId;
        LocationId = locationId;
        Latitude = latitude;
        Longitude = longitude;
    }
}