export interface ReadingModel {
  readingId: number,
  stationId: number,
  timestamp: string,
  temperature: number,
  humidity: number,
  windspeed: number,
  precipitation: number
}
