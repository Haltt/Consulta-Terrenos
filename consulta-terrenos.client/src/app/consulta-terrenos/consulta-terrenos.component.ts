import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode'; 

@Component({
  selector: 'app-consulta-terrenos',
  templateUrl: './consulta-terrenos.component.html',
  styleUrls: ['./consulta-terrenos.component.css'] 
})
export class ConsultaTerrenosComponent implements OnInit {
  filtros = {
    areaInteresse: '',
    tamanhoMin: '',
    tamanhoMax: '',
    precoMin: '',
    precoMax: '',
    tipoUso: ''
  };

  buscaAvancada = {
    localidade: '',  // Pode ser um nome de lugar ou coordenadas "latitude,longitude"
    raio: ''         // Raio em metros
  };

  mensagemErro: string | null = null;
  terrenos: any[] = [];
  pesquisaRealizada: boolean = false;
  usuarioId: number | null = null; // Armazena o ID do usuário logado
  favoritos: number[] = []; // Lista de IDs de terrenos favoritados

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

  buscarTerrenos() {
    const params = {
      areaInteresse: this.filtros.areaInteresse,
      tamanhoMin: this.filtros.tamanhoMin,
      tamanhoMax: this.filtros.tamanhoMax,
      precoMin: this.filtros.precoMin,
      precoMax: this.filtros.precoMax,
      tipoUso: this.filtros.tipoUso
    };

    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    this.http.get('https://localhost:7113/api/Terrenos', { headers, params }).subscribe((data: any) => {
      this.terrenos = data;
      this.pesquisaRealizada = true;
      if (this.terrenos.length === 0) {
        this.mensagemErro = 'Nenhum terreno encontrado com os filtros aplicados.';
      } else {
        this.mensagemErro = null;
      }
    }, (error) => {
      console.error('Erro ao buscar terrenos:', error);
      this.mensagemErro = 'Erro ao buscar terrenos. Tente novamente mais tarde.';
    });
  }

  buscarTerrenosAvancados() {
    const params = {
      localidade: this.buscaAvancada.localidade,
      raio: this.buscaAvancada.raio
    };

    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    this.http.get(`https://localhost:7113/api/Terrenos/consultar-terrenos/ST_Distance_Sphere/${params.localidade}/${params.raio}`, { headers }).subscribe(
      (data: any) => {
        this.terrenos = data;
        this.pesquisaRealizada = true;
        if (this.terrenos.length === 0) {
          this.mensagemErro = 'Nenhum terreno encontrado com os filtros avançados aplicados.';
        } else {
          this.mensagemErro = null;
        }
      },
      (error) => {
        console.error('Erro ao buscar terrenos avançados:', error);

        // Verificar se há uma mensagem de erro no corpo da resposta
        if (error.error && error.error.Message) {
          this.mensagemErro = error.error.Message; // Mostrar a mensagem retornada pela API
        } else {
          this.mensagemErro = 'Erro ao buscar terrenos avançados. Tente novamente mais tarde.';
        }
      }
    );
  }

  carregarFavoritos() {
    if (this.usuarioId !== null) {
      const token = localStorage.getItem('token');
      const headers = { Authorization: `Bearer ${token}` };

      this.http.get(`https://localhost:7113/api/Favoritos/${this.usuarioId}`, { headers }).subscribe((data: any) => {
        this.favoritos = data.map((favorito: any) => favorito.usersId); // Salvar os IDs dos terrenos favoritos
      }, error => {
        console.error('Erro ao carregar favoritos:', error);
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

  abrirDetalhes(terrenoId: number) {
    this.router.navigate(['/terrenos', terrenoId]);
  }

  isFavorito(usersId: number): boolean {
    return this.favoritos.includes(usersId);
  }
}
