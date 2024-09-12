import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-meus-terrenos-favoritos',
  templateUrl: './meus-terrenos-favoritos.component.html',
  styleUrl: './meus-terrenos-favoritos.component.css'
})

export class MeusTerrenosFavoritosComponent implements OnInit {
  favoritos: any[] = [];
  mensagemErro: string | null = null;
  usuarioId: number | null = null;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.router.navigate(['/login']);
    } else {
      this.decodeToken(token); // Decodificar o token para obter o ID do usuário
      this.carregarFavoritos(); // Carregar a lista de favoritos do usuário
    }
  }

  decodeToken(token: string) {
    try {
      const decodedToken: any = jwtDecode(token); // Decodificar o token JWT
      this.usuarioId = decodedToken.id; // Supondo que o campo `id` esteja presente no token
    } catch (error) {
      console.error('Erro ao decodificar o token:', error);
      this.mensagemErro = 'Erro na autenticação. Por favor, faça login novamente.';
    }
  }

  carregarFavoritos(): void {
    if (this.usuarioId !== null) {
      const token = localStorage.getItem('token');
      const headers = { Authorization: `Bearer ${token}` };

      this.http.get(`https://localhost:7113/api/Favoritos/${this.usuarioId}`, { headers })
        .subscribe((data: any) => {  // Espera-se que `data` seja uma lista
          if (data && data.length > 0) {
            this.favoritos = data;
            this.mensagemErro = null; // Limpa a mensagem de erro se favoritos forem encontrados
          } else {
            this.favoritos = [];
            this.mensagemErro = 'Você não tem terrenos favoritos.';
          }
        }, error => {
          this.mensagemErro = 'Erro ao carregar os favoritos.';
          console.error(error);
        });
    }
  }

  favoritarTerreno(terrenoId: number) {
    if (this.usuarioId !== null) {
      const favorito = { usersId: this.usuarioId, terrenosId: terrenoId };
      this.http.post('https://localhost:7113/api/Favoritos', favorito).subscribe(response => {
        console.log('Favorito atualizado:', response);
        // Atualizar a lista de favoritos após marcar/desmarcar
        this.carregarFavoritos();
      }, error => {
        console.error('Erro ao favoritar/desfavoritar terreno:', error);
      });
    } else {
      console.error('Usuário não autenticado.');
    }
  }
}
