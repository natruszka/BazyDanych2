import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../environments/environment";
import {LocationModel} from "../models/location-model";
import {LocationFormModel} from "../models/location-form-model";

@Injectable({
  providedIn: 'root'
})
export class LocationsService {

  constructor(private http: HttpClient) { }
  getLocationsFromApi(): Observable<{[id: string] : LocationModel[]}>
  {
    return this.http.get<{[id: string] : LocationModel[]}>(environment.baseUrl+'locations').pipe(tap(data=>console.log(data)));
  }
  addLocationToApi(id: number, locationDto: LocationFormModel)
  {
    this.http.post(environment.baseUrl+'locations/' + id, locationDto).subscribe(response =>{console.log(response)})
  }
}
