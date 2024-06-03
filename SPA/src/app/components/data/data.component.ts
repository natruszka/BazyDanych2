import {Component, OnInit} from '@angular/core';
import {KeyValuePipe, NgForOf} from "@angular/common";
import {DataService} from "../../services/data.service";
import {DataModel} from "../../models/data-model";
import {FormBuilder, ReactiveFormsModule, Validators} from "@angular/forms";

@Component({
  selector: 'app-data',
  standalone: true,
  imports: [
    KeyValuePipe,
    NgForOf,
    ReactiveFormsModule
  ],
  templateUrl: './data.component.html',
  styleUrl: './data.component.css'
})
export class DataComponent implements OnInit{
  data: DataModel[] = [];
  frameForm = this.formBuilder.group({
    startDate: [[Validators.required]],
    endDate: [[Validators.required]]
  })
  constructor(private service: DataService, private formBuilder: FormBuilder) {
  }
  getData()
  {
    this.service.getDataFromApi().subscribe(data=>this.data = data)
  }
  getHistoricalData()
  {
    let startDate = this.frameForm.value.startDate as unknown as Date;
    let endDate = this.frameForm.value.endDate as unknown as Date;
    console.log(startDate, endDate)

      if(startDate > endDate)
      {
        [startDate, endDate] = [endDate, startDate]
      }
      this.service.getHistoricalDataFromApi(startDate, endDate).subscribe(data =>this.data = data);

  }
  ngOnInit(): void {
    this.getData();
  }

}
