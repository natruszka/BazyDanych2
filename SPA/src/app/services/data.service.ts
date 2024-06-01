import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {DataModel} from "../models/data-model";
import {environment} from "../environments/environment";
import {query} from "@angular/animations";
import {parallel} from "@angular/cdk/testing";
import {H} from "@angular/cdk/keycodes";

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient) { }
  formatDate(date: Date) : string
  {
    const year: number = date.getFullYear();
    const month: string = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const day: string = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  getDataFromApi(): Observable<DataModel[]>
  {
    const endDate: string = this.formatDate(new Date());
    const startDate = this.formatDate(new Date(new Date().setDate(new Date().getDate()-5)));

    let params = new HttpParams();
    params.append('startDate', startDate);
    params.append('endDate', endDate);
    console.log(params)
    return this.http.get<DataModel[]>(environment.baseUrl+`weather/historical?startdate=${startDate}&endDate=${endDate}`, ).pipe(tap(data=>console.log(data)));
  }
  getHistoricalDataFromApi(startDate: Date, endDate: Date): Observable<DataModel[]>
  {
    return this.http.get<DataModel[]>(environment.baseUrl+`weather/historical?startdate=${startDate}&endDate=${endDate}`, ).pipe(tap(data=>console.log(data)));
  }
  aggregateData()
  {
    this.http.post(environment.baseUrl+'weather/aggregate', null).subscribe(response =>{console.log(response)});
  }
  getServerList() : Observable<{[key: string] : number}>
  {
    return this.http.get<{[key:string]:number}>(environment.baseUrl +'servers').pipe(tap(data=>console.log(data)));
  }
  seedData(serverId: number, locationId: number, date: Date)
  {
    // const dateString: string = this.formatDate(date);
    this.http.post(environment.baseUrl+'weather/seed/'+serverId+'/'+locationId+`?date=${date}`, null).subscribe(response =>{console.log(response)});
  }
}
