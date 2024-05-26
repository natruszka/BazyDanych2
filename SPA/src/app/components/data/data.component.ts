import {Component, OnInit} from '@angular/core';
import {KeyValuePipe, NgForOf} from "@angular/common";
import {DataService} from "../../services/data.service";
import {DataModel} from "../../models/data-model";

@Component({
  selector: 'app-data',
  standalone: true,
    imports: [
        KeyValuePipe,
        NgForOf
    ],
  templateUrl: './data.component.html',
  styleUrl: './data.component.css'
})
export class DataComponent implements OnInit{
  data: DataModel[] = [];
  constructor(private service: DataService) {
  }
  getData()
  {
    this.service.getDataFromApi().subscribe(data=>this.data = data)
  }
  ngOnInit(): void {
    this.getData();
  }

}
