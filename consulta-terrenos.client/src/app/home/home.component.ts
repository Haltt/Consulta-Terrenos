import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private apiUrl = 'https://localhost:7113/api'
  usuario = {
    nome: '',
    email: '',
    areaInteresse: '',
    senhaCriptografada: '',
    confirmarSenha: ''
  };

  mensagem: string = ''; // Para exibir mensagens de sucesso ou erro
  erro: string = '';     // Para exibir mensagens de erro

  constructor(public authService: AuthService, private http: HttpClient) { }


  // Método para cadastrar o usuário
  cadastrarUsuario() {
    this.mensagem = '';
    this.erro = '';

    // Verifica se as senhas coincidem
    if (this.usuario.senhaCriptografada !== this.usuario.confirmarSenha) {
      this.erro = 'As senhas não coincidem.';
      return;
    }

    // Prepara os dados do usuário com a senha criptografada
    const usuarioData = {
      ...this.usuario,
      senhaCriptografada: btoa(this.usuario.senhaCriptografada) // Criptografa a senha
    };

    // Faz a requisição POST para cadastrar o usuário
    this.http.post(`${this.apiUrl}/Users`, usuarioData).subscribe({
      next: (response) => {
        // Sucesso no cadastro
        this.mensagem = 'Usuário cadastrado com sucesso!';
        this.limparFormulario(); // Limpa o formulário após o cadastro
      },
      error: (error) => {
        // Verifica se o erro é relacionado ao e-mail duplicado
        if (error.error.message === 'Este e-mail já está cadastrado.') {
          this.erro = 'Este e-mail já está em uso. Por favor, use outro e-mail.';
        } else {
          this.erro = 'Erro ao cadastrar o usuário: ' + error.error?.message || 'Tente novamente mais tarde.';
        }
      }
    });
  }

  // Método para limpar o formulário
  limparFormulario() {
    this.usuario = {
      nome: '',
      email: '',
      areaInteresse: '',
      senhaCriptografada: '',
      confirmarSenha: ''
    };
  }
}
