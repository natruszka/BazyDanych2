import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../environments/environment";
import {LocationModel} from "../models/location-model";
import {LocationFormModel} from "../models/location-form-model";
import {StationModel} from "../models/station-model";
import {StationViewModel} from "../models/station-view-model";

@Injectable({
  providedIn: 'root'
})
export class StationsService {

  constructor(private http: HttpClient) { }
  getStationsFromApi(): Observable<{[id: string] : StationModel[]}>
  {
    return this.http.get<{[id: string] : StationModel[]}>(environment.baseUrl+'stations/raw').pipe(tap(data=>console.log(data)));
  }
  getStationsViewFromApi(): Observable<{[id: string] : StationViewModel[]}>
  {
    return this.http.get<{[id: string] : StationViewModel[]}>(environment.baseUrl+'stations/view').pipe(tap(data=>console.log(data)));
  }
  addLocationToApi(id: number, locationDto: LocationFormModel)
  {
    this.http.post(environment.baseUrl+'locations/' + id, locationDto).subscribe(response =>{console.log(response)})
  }
}
