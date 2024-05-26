import {Component, OnInit} from '@angular/core';
import {JsonPipe, KeyValuePipe, NgForOf} from "@angular/common";
import {RouterLink} from "@angular/router";
import {ReadingModel} from "../../models/reading-model";
import {ReadingsService} from "../../services/readings.service";

@Component({
  selector: 'app-locations',
  standalone: true,
  imports: [
    JsonPipe,
    NgForOf,
    KeyValuePipe,
    RouterLink
  ],
  templateUrl: './readings.component.html',
  styleUrl: './readings.component.css'
})
export class ReadingsComponent implements OnInit{
  readings: {[id: string] : ReadingModel[]} = {};
  constructor(private service: ReadingsService) {
  }

  getReadings(){
    this.service.getReadingsFromApi().subscribe(data=>this.readings = data)
  }

  ngOnInit(): void {
    this.getReadings()
  }
}
