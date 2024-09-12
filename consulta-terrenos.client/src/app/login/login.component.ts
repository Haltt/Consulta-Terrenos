import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginData = {
    email: '',
    senha: ''
  };

  constructor(private http: HttpClient, private router: Router) { }

  login() {
    this.http.post('https://localhost:7113/api/Users/login', this.loginData)
      .subscribe((response: any) => {
        if (response.token) {
          localStorage.setItem('token', response.token);
          this.router.navigate(['/']);
        } else {
          alert('Login falhou.');
        }
      }, (error) => {
        console.error('Erro ao fazer login:', error);
        alert('Erro ao acessar a API.');
      });
  }
}
