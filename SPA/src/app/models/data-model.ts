export interface DataModel {
  dataId: number,
  serverId: number,
  locationId: number,
  timestamp: string,
  temperature: number,
  minTemperature: number,
  maxTemperature: number,
  humidity: number,
  windSpeed: number,
  precipitation: number
}
