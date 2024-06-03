CREATE DATABASE [WeatherDatabase]
GO

USE [WeatherDatabase]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[locations](
	[location_id] [int] IDENTITY(1,1) NOT NULL,
	[latitude] [decimal](8, 6) NOT NULL,
	[longitude] [decimal](9, 6) NOT NULL,
	[city] [nvarchar](50) NOT NULL,
	[country] [nvarchar](50) NOT NULL,
	[elevation] [int] NOT NULL,
 CONSTRAINT [PK_locations] PRIMARY KEY CLUSTERED 
(
	[location_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[stations](
	[station_id] [int] IDENTITY(1,1) NOT NULL,
	[location_id] [int] NOT NULL,
	[latitude] [decimal](8, 6) NOT NULL,
	[longitude] [decimal](9, 6) NOT NULL,
 CONSTRAINT [PK_stations] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[stations]  WITH CHECK ADD  CONSTRAINT [FK_stations_locations] FOREIGN KEY([location_id])
REFERENCES [dbo].[locations] ([location_id])
GO

ALTER TABLE [dbo].[stations] CHECK CONSTRAINT [FK_stations_locations]
GO

CREATE TABLE [dbo].[weather_readings](
	[reading_id] [int] IDENTITY(1,1) NOT NULL,
	[station_id] [int] NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[temperature] [decimal](18, 0) NOT NULL,
	[humidity] [decimal](18, 0) NOT NULL,
	[windspeed] [decimal](18, 0) NOT NULL,
	[precipitation] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_weather_readings] PRIMARY KEY CLUSTERED 
(
	[reading_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[weather_readings]  WITH CHECK ADD  CONSTRAINT [FK_weather_readings_stations] FOREIGN KEY([station_id])
REFERENCES [dbo].[stations] ([station_id])
GO

ALTER TABLE [dbo].[weather_readings] CHECK CONSTRAINT [FK_weather_readings_stations]
GO