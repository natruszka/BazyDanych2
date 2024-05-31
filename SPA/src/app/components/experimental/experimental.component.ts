import { Component } from '@angular/core';
import {RouterLink} from "@angular/router";
import {NgOptimizedImage} from "@angular/common";
import {MatFormField, MatFormFieldModule, MatLabel} from "@angular/material/form-field";
import {
  MatDatepicker,
  MatDatepickerInput,
  MatDatepickerModule,
  MatDatepickerToggle
} from "@angular/material/datepicker";
import {MatInputModule} from "@angular/material/input";
import {provideNativeDateAdapter} from "@angular/material/core";

@Component({
  selector: 'app-experimental',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    RouterLink,
    NgOptimizedImage,
    MatFormField,
    MatLabel,
    MatDatepickerToggle,
    MatDatepicker,
    MatDatepickerInput,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './experimental.component.html',
  styleUrl: './experimental.component.css'
})
export class ExperimentalComponent {

}
