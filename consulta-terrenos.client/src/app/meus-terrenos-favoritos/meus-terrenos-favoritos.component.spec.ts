import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeusTerrenosFavoritosComponent } from './meus-terrenos-favoritos.component';

describe('MeusTerrenosFavoritosComponent', () => {
  let component: MeusTerrenosFavoritosComponent;
  let fixture: ComponentFixture<MeusTerrenosFavoritosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MeusTerrenosFavoritosComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MeusTerrenosFavoritosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
