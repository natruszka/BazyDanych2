import {Component, OnInit} from '@angular/core';
import {KeyValuePipe, NgForOf} from "@angular/common";
import {LocationModel} from "../../models/location-model";
import {LocationsService} from "../../services/locations.service";
import {StationModel} from "../../models/station-model";
import {StationsService} from "../../services/stations.service";
import {RouterLink} from "@angular/router";
import {StationViewModel} from "../../models/station-view-model";

@Component({
  selector: 'app-stations',
  standalone: true,
  imports: [
    KeyValuePipe,
    NgForOf,
    RouterLink
  ],
  templateUrl: './stations.component.html',
  styleUrl: './stations.component.css'
})
export class StationsComponent implements OnInit{
  stations: {[id: string] : StationModel[]} = {};
  stationsView: {[id: string] : StationViewModel[]} = {};
  constructor(private service: StationsService) {
  }

  getStations(){
    this.service.getStationsFromApi().subscribe(data=>this.stations = data)
  }
  getStationsViews(){
    this.service.getStationsViewFromApi().subscribe(data=>this.stationsView = data)
  }

  ngOnInit(): void {
    this.getStations()
    this.getStationsViews()
  }
}
