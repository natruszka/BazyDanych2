import { Routes } from '@angular/router';
import {HomeComponent} from "./components/home/home.component";
import {LocationsComponent} from "./components/locations/locations.component";
import {StationsComponent} from "./components/stations/stations.component";
import {ReadingsComponent} from "./components/readings/readings.component";
import {DataComponent} from "./components/data/data.component";
import {LocationsAddComponent} from "./components/locations-add/locations-add.component";

export const routes: Routes = [
  {path: "", pathMatch: "full", component: HomeComponent},
  {path: "locations", pathMatch: "full", component: LocationsComponent},
  {path: "stations", pathMatch: "full", component: StationsComponent},
  {path: "readings", pathMatch: "full", component: ReadingsComponent},
  {path: "data", pathMatch: "full", component: DataComponent},
  {path: "locations/add/:id", pathMatch: "full", component: LocationsAddComponent}
];
