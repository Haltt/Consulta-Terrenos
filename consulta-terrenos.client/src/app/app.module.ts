import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './jwt.interceptor.interceptor';

import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ConsultaTerrenosComponent } from './consulta-terrenos/consulta-terrenos.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MeusTerrenosFavoritosComponent } from './meus-terrenos-favoritos/meus-terrenos-favoritos.component';
import { DetalheTerrenoComponent } from './detalhe-terreno/detalhe-terreno.component';
import { ConsultasUsuarioComponent } from './consultas-usuario/consultas-usuario.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    ConsultaTerrenosComponent,
    MeusTerrenosFavoritosComponent,
    DetalheTerrenoComponent,
    ConsultasUsuarioComponent 
  ],
  imports: [
    FormsModule,
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FontAwesomeModule   
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
