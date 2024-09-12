import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultasUsuarioComponent } from './consultas-usuario.component';

describe('ConsultasUsuarioComponent', () => {
  let component: ConsultasUsuarioComponent;
  let fixture: ComponentFixture<ConsultasUsuarioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConsultasUsuarioComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConsultasUsuarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
