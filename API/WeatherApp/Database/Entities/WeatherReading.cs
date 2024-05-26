namespace WeatherApp.Database.Entities;

public class WeatherReading
{
    public int ReadingId { get; set; }
    public int StationId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public decimal WindSpeed { get; set; }
    public decimal Precipitation { get; set; }
}