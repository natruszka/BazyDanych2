using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DTOs;

public class WeatherReadingDto
{
    [Required]
    public int StationId { get; set; }
    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    public decimal Temperature { get; set; }
    [Required]
    [Range(0,100)]
    public decimal Humidity { get; set; }
    [Required]
    public decimal WindSpeed { get; set; }
    [Required]
    [Range(0,100)]
    public decimal Precipitation { get; set; }
}