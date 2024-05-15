namespace WeatherApp.Database.Entities;

public class WeatherEntity
{
    public int DataId { get; set; }
    public int LocationId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public decimal WindSpeed { get; set; }
    public decimal Precipitation { get; set; }
}