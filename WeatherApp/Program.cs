using WeatherApp.Database;
using WeatherApp.Services;
using WeatherApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<WeatherDbContext>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<StationService>();
builder.Services.AddScoped<WeatherService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

