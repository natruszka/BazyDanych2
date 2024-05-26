using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DTOs;

public class LocationDto
{
    [Range(-90, 90)]
    [Required]
    public decimal Latitude { get; set; }
    [Range(-180, 180)]
    [Required]
    public decimal Longitude { get; set; }
    [Required]
    public string City { get; set; } = null!;
    [Required]
    public string Country { get; set; } = null!;
    [Required]
    public decimal Elevation { get; set; }
}