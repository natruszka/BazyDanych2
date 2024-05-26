import {Component, OnInit} from '@angular/core';
import {LocationsService} from "../../services/locations.service";
import {JsonPipe, KeyValuePipe, NgForOf} from "@angular/common";
import {LocationModel} from "../../models/location-model";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-locations',
  standalone: true,
  imports: [
    JsonPipe,
    NgForOf,
    KeyValuePipe,
    RouterLink
  ],
  templateUrl: './locations.component.html',
  styleUrl: './locations.component.css'
})
export class LocationsComponent implements OnInit{
  locations: {[id: string] : LocationModel[]} = {};
  constructor(private service: LocationsService) {
  }

  getLocations(){
    this.service.getLocationsFromApi().subscribe(data=>this.locations = data)
  }

  ngOnInit(): void {
    this.getLocations()
  }
}
