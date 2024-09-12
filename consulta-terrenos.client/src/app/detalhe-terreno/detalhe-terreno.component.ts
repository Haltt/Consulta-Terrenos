import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode'

@Component({
  selector: 'app-detalhe-terreno',
  templateUrl: './detalhe-terreno.component.html',
  styleUrls: ['./detalhe-terreno.component.css']
})
export class DetalheTerrenoComponent implements OnInit {
  terrenoId: number | null = null;
  terreno: any = null;
  usuarioId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
    } else {
      this.usuarioId = this.decodeToken(token).id; // Obtém o ID do usuário
      this.terrenoId = Number(this.route.snapshot.paramMap.get('id')); // Obtém o ID do terreno da URL
      if (this.terrenoId) {
        this.obterDetalhesTerreno();
        this.registrarConsulta(); // Registrar consulta ao carregar a página
      }
    }
  }

  decodeToken(token: string): any {
    try {
      return jwtDecode(token); // Decodificar o token JWT para obter o ID do usuário
    } catch (error) {
      console.error('Erro ao decodificar token:', error);
      return null;
    }
  }

  obterDetalhesTerreno(): void {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    this.http.get(`https://localhost:7113/api/Terrenos/consultar-terreno/${this.terrenoId}`, { headers })
      .subscribe((data: any) => {
        this.terreno = data;
        console.log('Te', data)
      }, (error) => {
        console.error('Erro ao obter detalhes do terreno:', error);
      });
  }

  registrarConsulta(): void {
    const consulta = {
      usersId: this.usuarioId,
      terrenosId: this.terrenoId
    };

    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    this.http.post('https://localhost:7113/api/Consultas', consulta, { headers })
      .subscribe(response => {
        console.log('Consulta registrada com sucesso:', response);
      }, error => {
        console.error('Erro ao registrar consulta:', error);
      });
  }
}
