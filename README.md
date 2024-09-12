# Consulta Terrenos

Este projeto é uma aplicação para consulta de terrenos com busca avançada, onde o usuário pode realizar pesquisas com base em filtros como área de interesse, preço, tipo de uso e também pode realizar consultas espaciais com base em coordenadas (latitude, longitude) ou nomes de locais, utilizando um raio de busca.

## Funcionalidades

- **Consulta de Terrenos**: Permite que os usuários filtrem terrenos por tamanho, preço e tipo de uso.
- **Busca Avançada**: Realiza consultas espaciais com base em coordenadas geográficas ou nomes de locais, utilizando o raio especificado.
- **Autenticação JWT**: O sistema utiliza autenticação JWT para proteger as APIs de consulta e gerenciamento de favoritos.
- **Favoritar Terrenos**: Usuários logados podem favoritar ou remover terrenos dos seus favoritos.
- **Consulta Espacial**: Implementada com `ST_Distance_Sphere` do MariaDB para cálculos de distância.

## Tecnologias Utilizadas

- **Frontend**: 
  - Angular
  - Bootstrap
  - Font Awesome

- **Backend**:
  - ASP.NET Core
  - Entity Framework Core
  - MariaDB
  - Autenticação JWT

- **Geocodificação**: Google Maps API ou outro serviço para geocodificar nomes de locais em coordenadas.

## Requisitos

- **Node.js** (versão 14 ou superior)
- **Angular CLI**
- **.NET 6.0 SDK**
- **MariaDB** (versão 10.5 ou superior)
- **Chave de API do Google Maps** (para geocodificação)

## Como Rodar o Projeto

### Backend

1. Clone o repositório:
    ```bash
    git clone https://github.com/Haltt/Consulta-Terrenos.git
    ```

2. Navegue até o diretório do backend:
    ```bash
    cd Consulta-Terrenos/Consulta-Terrenos.Server
    ```

3. Instale as dependências do projeto:
    ```bash
    dotnet restore
    ```

4. Configure a string de conexão com o banco de dados MariaDB no arquivo `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "server=localhost;port=3306;database=ConsultaTerrenos;user=root;password=your_password;"
    }
    ```

5. Adicione sua chave de API do Google Maps no arquivo `appsettings.json` para geocodificação:
    ```json
    "GoogleMapsApiKey": "YOUR_GOOGLE_MAPS_API_KEY"
    ```

6. Execute as migrações do banco de dados:
    ```bash
    dotnet ef database update
    ```

7. Inicie o servidor:
    ```bash
    dotnet run
    ```

### Frontend

1. Navegue até o diretório do frontend:
    ```bash
    cd Consulta-Terrenos/Consulta-Terrenos.Client
    ```

2. Instale as dependências:
    ```bash
    npm install
    ```

3. Inicie o servidor de desenvolvimento do Angular:
    ```bash
    ng serve
    ```

4. Acesse a aplicação no navegador:
    ```
    http://localhost:4200
    ```

## Como Usar a Aplicação

### Autenticação

- Ao acessar a aplicação, o usuário precisa se autenticar. Isso é feito com base no login via JWT.

### Consulta de Terrenos

- O usuário pode utilizar a pesquisa simples para filtrar terrenos por área de interesse, tamanho, preço e tipo de uso.
- A **Busca Avançada** permite que o usuário forneça uma localidade (coordenadas ou nome de um local) e um raio de busca para obter os terrenos dentro do raio especificado.

### Favoritar Terrenos

- Após a autenticação, o usuário pode marcar terrenos como favoritos e também removê-los de sua lista de favoritos.

## APIs Documentadas com Swagger

As APIs do backend são documentadas e podem ser acessadas através do Swagger UI. Para visualizar a documentação, rode o projeto backend e acesse:


## Estrutura do Projeto

- `Consulta-Terrenos.Server/`: Diretório do backend ASP.NET Core.
- `Consulta-Terrenos.Client/`: Diretório do frontend Angular.
- `Consulta-Terrenos.Shared/`: Modelos e DTOs compartilhados entre o cliente e o servidor.

## Contribuições

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -am 'Adiciona nova feature'`).
4. Faça o push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

## Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para mais detalhes.

