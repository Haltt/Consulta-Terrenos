import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component'
import { DetalheTerrenoComponent } from './detalhe-terreno/detalhe-terreno.component'
import { ConsultasUsuarioComponent } from './consultas-usuario/consultas-usuario.component';
import { ConsultaTerrenosComponent } from './consulta-terrenos/consulta-terrenos.component';
import { MeusTerrenosFavoritosComponent } from './meus-terrenos-favoritos/meus-terrenos-favoritos.component';


const routes: Routes = [
  { path: 'home', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'terrenos/:id', component: DetalheTerrenoComponent },
  { path: 'consultas-usuario', component: ConsultasUsuarioComponent },
  { path: 'consulta-terrenos', component: ConsultaTerrenosComponent },
  { path: 'meus-terrenos-favoritos', component: MeusTerrenosFavoritosComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
