import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FavoritosService {

  //private apiUrl = 'https://localhost:7113/api'; // URL da API

  constructor(private http: HttpClient) { }

  // Método para obter os terrenos favoritos do usuário
  getFavoritos(userId: string): Observable<any[]> {
    const token = localStorage.getItem('token');
    if (!token) {
      throw new Error('Usuário não autenticado');
    }
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.get<any[]>(`$https://localhost:7113/api/Favoritos/${userId}`, { headers });
  }

  // Método para remover um terreno dos favoritos
  removerFavorito(terrenoId: string): Observable<void> {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.delete<void>(`https://localhost:7113/api/Favoritos/${terrenoId}`, { headers });
  }
}
