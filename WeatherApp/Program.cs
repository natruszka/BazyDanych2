using WeatherApp.Database;
using WeatherApp.Services;
using WeatherApp.Services.Interfaces;

var corsPolicy = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<WeatherDbContext>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ServerService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IWeatherService,WeatherService>();
builder.Services.AddDateOnlyTimeOnlyStringConverters();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();

