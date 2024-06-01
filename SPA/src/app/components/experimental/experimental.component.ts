import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {KeyValuePipe, NgForOf, NgOptimizedImage} from "@angular/common";
import {FormBuilder, ReactiveFormsModule, Validators} from "@angular/forms";
import {LocationsService} from "../../services/locations.service";
import {DataService} from "../../services/data.service";

@Component({
  selector: 'app-experimental',
  standalone: true,
  imports: [
    RouterLink,
    NgOptimizedImage,
    ReactiveFormsModule,
    NgForOf,
    KeyValuePipe,
  ],
  templateUrl: './experimental.component.html',
  styleUrl: './experimental.component.css'
})
export class ExperimentalComponent implements OnInit{
servers : {[key: string] : number} = {};
locationIds : number[] = [];
key : any;
frameForm = this.formBuilder.group({
  server: [' ',[Validators.required]],
  locationId: [0, [Validators.required]],
  date: [[Validators.required]]
})
constructor(private router: Router, private route: ActivatedRoute, private dataService: DataService, private locationService: LocationsService, private formBuilder: FormBuilder) {
}
  getLocationIds() {
    this.key = (this.frameForm.value.server);
    if (this.key && this.servers.hasOwnProperty(this.key)) {
      let id: number = this.servers[this.key];
      this.locationService.getAllLocationIds(id).subscribe(data => this.locationIds = data);
    }
  }
  ngOnInit(): void {
    this.dataService.getServerList().subscribe(data=>this.servers = data);
  }
  aggregateData()
  {
    this.dataService.aggregateData();
  }
  onSubmit()
  {
    if(this.key && this.servers.hasOwnProperty(this.key))
    {
      let serverId =  this.servers[this.key] as number;
      let locationId= this.frameForm.value.locationId as number;
      let date = this.frameForm.value.date as unknown as Date;
      console.log(serverId, locationId, date)
      this.dataService.seedData(serverId, locationId, date);
    }
  }
}
