import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalheTerrenoComponent } from './detalhe-terreno.component';

describe('DetalheTerrenoComponent', () => {
  let component: DetalheTerrenoComponent;
  let fixture: ComponentFixture<DetalheTerrenoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DetalheTerrenoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetalheTerrenoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
