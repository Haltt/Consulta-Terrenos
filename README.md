# Consulta Terrenos

Este projeto � uma aplica��o para consulta de terrenos com busca avan�ada, onde o usu�rio pode realizar pesquisas com base em filtros como �rea de interesse, pre�o, tipo de uso e tamb�m pode realizar consultas espaciais com base em coordenadas (latitude, longitude) ou nomes de locais, utilizando um raio de busca.

## Funcionalidades

- **Consulta de Terrenos**: Permite que os usu�rios filtrem terrenos por tamanho, pre�o e tipo de uso.
- **Busca Avan�ada**: Realiza consultas espaciais com base em coordenadas geogr�ficas ou nomes de locais, utilizando o raio especificado.
- **Autentica��o JWT**: O sistema utiliza autentica��o JWT para proteger as APIs de consulta e gerenciamento de favoritos.
- **Favoritar Terrenos**: Usu�rios logados podem favoritar ou remover terrenos dos seus favoritos.
- **Consulta Espacial**: Implementada com `ST_Distance_Sphere` do MariaDB para c�lculos de dist�ncia.

## Tecnologias Utilizadas

- **Frontend**: 
  - Angular
  - Bootstrap
  - Font Awesome

- **Backend**:
  - ASP.NET Core
  - Entity Framework Core
  - MariaDB
  - Autentica��o JWT

- **Geocodifica��o**: Google Maps API ou outro servi�o para geocodificar nomes de locais em coordenadas.

## Requisitos

- **Node.js** (vers�o 14 ou superior)
- **Angular CLI**
- **.NET 6.0 SDK**
- **MariaDB** (vers�o 10.5 ou superior)
- **Chave de API do Google Maps** (para geocodifica��o)

## Como Rodar o Projeto

### Backend

1. Clone o reposit�rio:
    ```bash
    git clone https://github.com/Haltt/Consulta-Terrenos.git
    ```

2. Navegue at� o diret�rio do backend:
    ```bash
    cd Consulta-Terrenos/Consulta-Terrenos.Server
    ```

3. Instale as depend�ncias do projeto:
    ```bash
    dotnet restore
    ```

4. Configure a string de conex�o com o banco de dados MariaDB no arquivo `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "server=localhost;port=3306;database=ConsultaTerrenos;user=root;password=your_password;"
    }
    ```

5. Adicione sua chave de API do Google Maps no arquivo `appsettings.json` para geocodifica��o:
    ```json
    "GoogleMapsApiKey": "YOUR_GOOGLE_MAPS_API_KEY"
    ```

6. Execute as migra��es do banco de dados:
    ```bash
    dotnet ef database update
    ```

7. Inicie o servidor:
    ```bash
    dotnet run
    ```

### Frontend

1. Navegue at� o diret�rio do frontend:
    ```bash
    cd Consulta-Terrenos/Consulta-Terrenos.Client
    ```

2. Instale as depend�ncias:
    ```bash
    npm install
    ```

3. Inicie o servidor de desenvolvimento do Angular:
    ```bash
    ng serve
    ```

4. Acesse a aplica��o no navegador:
    ```
    http://localhost:4200
    ```

## Como Usar a Aplica��o

### Autentica��o

- Ao acessar a aplica��o, o usu�rio precisa se autenticar. Isso � feito com base no login via JWT.

### Consulta de Terrenos

- O usu�rio pode utilizar a pesquisa simples para filtrar terrenos por �rea de interesse, tamanho, pre�o e tipo de uso.
- A **Busca Avan�ada** permite que o usu�rio forne�a uma localidade (coordenadas ou nome de um local) e um raio de busca para obter os terrenos dentro do raio especificado.

### Favoritar Terrenos

- Ap�s a autentica��o, o usu�rio pode marcar terrenos como favoritos e tamb�m remov�-los de sua lista de favoritos.

## APIs Documentadas com Swagger

As APIs do backend s�o documentadas e podem ser acessadas atrav�s do Swagger UI. Para visualizar a documenta��o, rode o projeto backend e acesse:


## Estrutura do Projeto

- `Consulta-Terrenos.Server/`: Diret�rio do backend ASP.NET Core.
- `Consulta-Terrenos.Client/`: Diret�rio do frontend Angular.
- `Consulta-Terrenos.Shared/`: Modelos e DTOs compartilhados entre o cliente e o servidor.

## Contribui��es

Contribui��es s�o bem-vindas! Para contribuir:

1. Fa�a um fork do projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudan�as (`git commit -am 'Adiciona nova feature'`).
4. Fa�a o push para a branch (`git push origin feature/nova-feature`).
5. Abra um Pull Request.

## Licen�a

Este projeto est� licenciado sob a Licen�a MIT - veja o arquivo [LICENSE](LICENSE) para mais detalhes.

