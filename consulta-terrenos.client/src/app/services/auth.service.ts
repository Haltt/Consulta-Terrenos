import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = 'https://localhost:7113/api/Users'; // URL da API
  private userId: string | null = null; // Armazena o ID do usuário logado

  constructor(private http: HttpClient, private router: Router) { }

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials);
  }

  // Método para salvar o token e extrair o ID do usuário
  setToken(token: string): void {
    localStorage.setItem('token', token);
    try {
      const decodedToken: any = jwtDecode(token);
      // Certifique-se de que o campo userId existe no payload do token
      const userId = decodedToken.id || null;
      localStorage.setItem('userId', userId);
    } catch (error) {
      console.error('Erro ao decodificar o token:', error);
    }
  }

  // Método para obter o ID do usuário logado
  getUserId(): string | null {
    return localStorage.getItem('userId');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    this.router.navigate(['/']);
  }

  // Verifica se o usuário está logado
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
