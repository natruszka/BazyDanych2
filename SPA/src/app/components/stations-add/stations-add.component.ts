import {Component, OnInit} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {StationFormModel} from "../../models/station-form-model";
import {StationsService} from "../../services/stations.service";
import {LocationsService} from "../../services/locations.service";
import {NgForOf} from "@angular/common";
import {servers} from "../../environments/environment";

@Component({
  selector: 'app-stations-add',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgForOf
  ],
  templateUrl: './stations-add.component.html',
  styleUrl: './stations-add.component.css'
})
export class StationsAddComponent implements OnInit{
  locationIds: number[] = [];
  public id:number = 0;
  frameForm = this.formBuilder.group({
    locationId: [0, [Validators.required]],
    latitude: [0, [Validators.required]],
    longitude: [0, [Validators.required]],
    elevation: [0, [Validators.required]],
  })
  constructor(private router: Router, private route: ActivatedRoute, private service: StationsService, private locationService: LocationsService, private formBuilder: FormBuilder) {
  }
  ngOnInit(): void {
    this.route.paramMap.subscribe(
      params => {
        const key = params.get('id');
        if (key && servers.hasOwnProperty(key)) {
          this.id = servers[key];
        } else {
          this.id = 0; // Default value if the key is not found or null
        }
      })
    this.getLocationIds(this.id);
  }
  getLocationIds(id: number){
    this.locationService.getAllLocationIds(id).subscribe(data => this.locationIds = data);
  }
  onSubmit(){
    let station: StationFormModel = {
      locationId: this.frameForm.value.locationId as number,
      latitude: this.frameForm.value.latitude as number,
      longitude: this.frameForm.value.longitude as number
    }
    console.log(station);
    this.service.addStationToApi(this.id, station);
    setTimeout(()=>{
      this.router.navigate(["/stations"])
    }, 1000)
  }
}
