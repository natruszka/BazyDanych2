import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StationsAddComponent } from './stations-add.component';

describe('StationsAddComponent', () => {
  let component: StationsAddComponent;
  let fixture: ComponentFixture<StationsAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StationsAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StationsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
