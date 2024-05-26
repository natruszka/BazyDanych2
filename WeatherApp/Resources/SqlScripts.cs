namespace WeatherApp.Resources;

public static class SqlScripts
{
    public static readonly string CheckIfUpsertProcedureExists = @"
        IF OBJECT_ID('dbo.UpsertWeatherData', 'P') IS NOT NULL
        BEGIN
            SELECT 1;
        END
        ELSE
        BEGIN
            SELECT 0;
        END;";
    public static readonly string UpsertProcedureScript = @"
        CREATE PROCEDURE dbo.UpsertWeatherData
            @Server_Id INT,
            @Location_Id INT,
            @Timestamp DATETIME,
            @Temperature DECIMAL(18, 0),
            @Min_Temperature DECIMAL(18, 0),
            @Max_Temperature DECIMAL(18, 0),
            @Humidity DECIMAL(18, 0),
            @WindSpeed DECIMAL(18, 0),
            @Precipitation DECIMAL(18, 0)
        AS
        BEGIN
            IF EXISTS (SELECT 1 FROM Weather_Data WHERE Server_Id = @Server_Id AND Location_Id = @Location_Id AND Timestamp = @Timestamp)
            BEGIN
                UPDATE Weather_Data
                SET Temperature = @Temperature,
                    Min_Temperature = @Min_Temperature,
                    Max_Temperature = @Max_Temperature,
                    Humidity = @Humidity,
                    WindSpeed = @WindSpeed,
                    Precipitation = @Precipitation
                WHERE Server_Id = @Server_Id AND Location_Id = @Location_Id AND Timestamp = @Timestamp;
            END
            ELSE
            BEGIN
                INSERT INTO Weather_Data (Server_Id, Location_Id, Timestamp, Temperature, Min_Temperature, Max_Temperature, Humidity, WindSpeed, Precipitation)
                VALUES (@Server_Id, @Location_Id, @Timestamp, @Temperature, @Min_Temperature, @Max_Temperature, @Humidity, @WindSpeed, @Precipitation);
            END
        END;
        ";
}