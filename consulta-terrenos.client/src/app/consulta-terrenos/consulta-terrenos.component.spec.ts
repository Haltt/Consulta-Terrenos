import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ConsultaTerrenosComponent } from './consulta-terrenos.component';

describe('ConsultaTerrenosComponent', () => {
  let component: ConsultaTerrenosComponent;
  let fixture: ComponentFixture<ConsultaTerrenosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConsultaTerrenosComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConsultaTerrenosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
