using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DTOs;

public class StationDto
{
    [Required]
    public int LocationId { get; set; }
    [Required]
    [Range(-90, 90)]
    public decimal Latitude { get; set; }
    [Required]
    [Range(-180, 180)]
    public decimal Longitude { get; set; }
}