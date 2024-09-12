import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode'

@Component({
  selector: 'app-consultas-usuario',
  templateUrl: './consultas-usuario.component.html',
  styleUrls: ['./consultas-usuario.component.css']
})
export class ConsultasUsuarioComponent implements OnInit {
  consultas: any[] = [];
  quantidadeFavoritos: number = 0;
  mensagemErro: string | null = null;
  usuarioId: number | null = null;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
    } else {
      this.decodeToken(token);
      this.carregarConsultas();
    }
  }

  decodeToken(token: string): void {
    try {
      const decodedToken: any = jwtDecode(token);
      this.usuarioId = decodedToken.id; // Supondo que o ID do usuário esteja no token
    } catch (error) {
      console.error('Erro ao decodificar token:', error);
      this.mensagemErro = 'Erro ao obter o ID do usuário.';
    }
  }

  carregarConsultas(): void {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    this.http.get(`https://localhost:7113/api/consultas/${this.usuarioId}`, { headers })
      .subscribe((data: any) => {
        this.consultas = data.consultas;
        this.quantidadeFavoritos = data.quantidadeFavoritos;
      }, error => {
        this.mensagemErro = 'Erro ao carregar as consultas e favoritos.';
        console.error(error);
      });
  }
}
