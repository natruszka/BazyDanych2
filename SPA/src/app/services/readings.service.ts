import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../environments/environment";
import {LocationModel} from "../models/location-model";
import {LocationFormModel} from "../models/location-form-model";
import {ReadingModel} from "../models/reading-model";

@Injectable({
  providedIn: 'root'
})
export class ReadingsService {

  constructor(private http: HttpClient) { }
  getReadingsFromApi(): Observable<{[id: string] : ReadingModel[]}>
  {
    return this.http.get<{[id: string] : ReadingModel[]}>(environment.baseUrl+'weather/readings').pipe(tap(data=>console.log(data)));
  }
  addLocationToApi(id: number, locationDto: LocationFormModel)
  {
    this.http.post(environment.baseUrl+'locations/' + id, locationDto).subscribe(response =>{console.log(response)})
  }
}
