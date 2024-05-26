namespace WeatherApp.Database.Entities;

public class WeatherData
{
    public int DataId { get; set; }
    public int ServerId { get; set; }
    public int LocationId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Temperature { get; set; }
    public decimal MinTemperature { get; set; }
    public decimal MaxTemperature { get; set; }
    public decimal Humidity { get; set; }
    public decimal WindSpeed { get; set; }
    public decimal Precipitation { get; set; }
}