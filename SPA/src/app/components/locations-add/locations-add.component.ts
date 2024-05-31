import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators, FormArray, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {ActivatedRoute, Route, Router} from "@angular/router";
import {LocationsService} from "../../services/locations.service";
import {LocationFormModel} from "../../models/location-form-model";
import {servers} from "../../environments/environment";

@Component({
  selector: 'app-locations-add',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './locations-add.component.html',
  styleUrl: './locations-add.component.css'
})
export class LocationsAddComponent implements OnInit{
  public id:number = 0;
  frameForm = this.formBuilder.group({
    latitude: [0, [Validators.required]],
    longitude: [0, [Validators.required]],
    city: ['', [Validators.required]],
    country: ['', [Validators.required]],
    elevation: [0, [Validators.required]],
  })
  constructor(private router: Router, private route: ActivatedRoute, private service: LocationsService, private formBuilder: FormBuilder) {
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
  }
  onSubmit(){
    let location: LocationFormModel = {
      latitude: this.frameForm.value.latitude as number,
      longitude: this.frameForm.value.longitude as number,
      city: this.frameForm.value.city as string,
      country: this.frameForm.value.country as string,
      elevation: this.frameForm.value.elevation as number
    }
    console.log(location);
    this.service.addLocationToApi(this.id, location);
    setTimeout(()=>{
      this.router.navigate(["/locations"])
    }, 1000)
  }

}
