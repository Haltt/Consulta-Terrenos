using Consulta_Terrenos.Server.Data;
using Consulta_Terrenos.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Consulta_Terrenos.Server.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os terrenos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("API responsável pelo gerenciamento de terrenos, incluindo cadastro e consulta.")]
    public class TerrenosController(ConsultaTerrenosDbContext context) : Controller
    {
        private readonly ConsultaTerrenosDbContext _context = context;

        /// <summary>
        /// Lista terrenos com base nos filtros fornecidos.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite filtrar terrenos por área de interesse, tamanho, preço e tipo de uso.
        /// Todos os parâmetros são opcionais.
        /// </remarks>
        /// <param name="areaInteresse">Área de interesse do terreno.</param>
        /// <param name="tamanhoMin">Tamanho mínimo do terreno.</param>
        /// <param name="tamanhoMax">Tamanho máximo do terreno.</param>
        /// <param name="precoMin">Preço mínimo do terreno.</param>
        /// <param name="precoMax">Preço máximo do terreno.</param>
        /// <param name="tipoUso">Tipo de uso do terreno (e.g., residencial, comercial).</param>
        /// <returns>Retorna uma lista de terrenos que atendem aos filtros especificados.</returns>
        /// <response code="200">Terrenos encontrados com sucesso.</response>
        /// <response code="404">Nenhum terreno encontrado com os filtros aplicados.</response>
        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Lista terrenos com filtros", Description = "Lista terrenos com base em filtros como área de interesse, tamanho, preço e tipo de uso.")]
        [SwaggerResponse(200, "Terrenos encontrados com sucesso.", typeof(List<Terrenos>))]
        [SwaggerResponse(404, "Nenhum terreno encontrado.")]
        public async Task<IActionResult> ListarTerrenos(
            [FromQuery] string? areaInteresse,
            [FromQuery] double? tamanhoMin,
            [FromQuery] double? tamanhoMax,
            [FromQuery] decimal? precoMin,
            [FromQuery] decimal? precoMax,
            [FromQuery] string? tipoUso)
        {
            var query = _context.Terrenos.AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(areaInteresse))
                query = query.Where(t => t.Coordenadas.Contains(areaInteresse));
            if (tamanhoMin.HasValue)
                query = query.Where(t => t.Tamanho >= tamanhoMin.Value);
            if (tamanhoMax.HasValue)
                query = query.Where(t => t.Tamanho <= tamanhoMax.Value);
            if (precoMin.HasValue)
                query = query.Where(t => t.Preco >= precoMin.Value);
            if (precoMax.HasValue)
                query = query.Where(t => t.Preco <= precoMax.Value);
            if (!string.IsNullOrEmpty(tipoUso))
                query = query.Where(t => t.TipoUso == tipoUso);

            var terrenos = await query.ToListAsync();

            if (!terrenos.Any())
                return NotFound(new { Message = "Nenhum terreno encontrado com os filtros aplicados." });

            return Ok(terrenos);
        }

        /// <summary>
        /// Consulta terrenos favoritos com base no terrenoId.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite consultar terreno específico.
        /// O terrenoId é um parâmetro obrigatório.
        /// </remarks>
        /// <param name="terrenoId">ID do terreno específico.</param>
        /// <returns>Retorna um terreno específico.</returns>
        /// <response code="200">Terreno encontrado com sucesso.</response>
        /// <response code="404">Nenhum terreno encontrado.</response>
        [Authorize]
        [HttpGet("consultar-terreno/{terrenoId}")]
        [SwaggerOperation(Summary = "Consulta terrenos favoritos de um usuário", Description = "Consulta terrenos favoritos com base no ID do usuário e, opcionalmente, o ID de um terreno específico.")]
        [SwaggerResponse(200, "Terrenos encontrados com sucesso.", typeof(List<Terrenos>))]
        [SwaggerResponse(404, "Nenhum terreno encontrado.")]
        public async Task<IActionResult> ConsultarTerrenos(int terrenoId)
        {
            var query = _context.Terrenos.AsQueryable();

            // Filtrar por terrenos favoritos do usuário
            query = query.Where(t => _context.Favoritos.Any(f => f.TerrenosId == t.Id));
            var terrenos = await query.ToListAsync();

            if (!terrenos.Any())
                return NotFound(new { Message = "Nenhum terreno encontrado com os filtros aplicados." });

            return Ok(terrenos);
        }

        /// <summary>
        /// Cadastra um novo terreno no sistema.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite cadastrar um novo terreno fornecendo o tamanho, preço, tipo de uso e coordenadas.
        /// </remarks>
        /// <param name="request">Objeto contendo as informações do terreno a ser cadastrado.</param>
        /// <returns>Retorna uma mensagem de sucesso após o cadastro do terreno.</returns>
        /// <response code="200">Terreno cadastrado com sucesso.</response>
        /// <response code="400">Erro ao cadastrar o terreno.</response>
        [Authorize]
        [HttpPost("cadastrar-terreno")]
        [SwaggerOperation(Summary = "Cadastra um novo terreno", Description = "Permite cadastrar um novo terreno no sistema.")]
        [SwaggerResponse(200, "Terreno cadastrado com sucesso.", typeof(object))]
        [SwaggerResponse(400, "Erro ao cadastrar o terreno.")]
        public async Task<IActionResult> CadastrarTerreno([FromBody] Terrenos request)
        {
            try
            {
                // Criar um novo objeto Terreno com os dados fornecidos na requisição
                var terreno = new Terrenos
                {
                    Tamanho = request.Tamanho,
                    Preco = request.Preco,
                    TipoUso = request.TipoUso,
                    Coordenadas = request.Coordenadas
                };

                // Adicionar o novo terreno ao banco de dados
                _context.Terrenos.Add(terreno);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Terreno cadastrado com sucesso" });
            }
            catch (Exception ex)
            {
                // Capturar qualquer exceção e retornar uma resposta de erro
                return BadRequest(new { Message = "Erro ao cadastrar o terreno.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Busca terrenos dentro de um raio a partir de uma localidade (coordenada ou nome de local).
        /// </summary>
        /// <remarks>
        /// A API pode receber tanto uma coordenada no formato "latitude, longitude" quanto um nome de local.
        /// Se for uma coordenada, calcula a distância diretamente.
        /// Se for um nome de local, realiza uma geocodificação para converter o nome em coordenadas antes de realizar a busca.
        /// </remarks>
        /// <param name="localidade">Pode ser uma coordenada no formato "latitude, longitude" ou o nome de um local (por exemplo, "São Paulo").</param>
        /// <param name="raio">Raio de busca em metros a partir da coordenada fornecida ou geocodificada.</param>
        /// <returns>Retorna uma lista de terrenos encontrados dentro do raio especificado.</returns>
        /// <response code="200">Terrenos encontrados com sucesso.</response>
        /// <response code="400">Erro ao processar a solicitação (por exemplo, erro na geocodificação ou formato de coordenadas inválido).</response>
        /// <response code="404">Nenhum terreno encontrado dentro do raio especificado.</response>
        [Authorize]
        [HttpGet("consultar-terrenos/ST_Distance_Sphere/{localidade}/{raio}")]
        [SwaggerOperation(Summary = "Busca terrenos em um raio a partir de uma localidade.", Description = "A localidade pode ser uma coordenada (latitude, longitude) ou o nome de um local. A API retorna os terrenos dentro do raio especificado.")]
        [SwaggerResponse(200, "Terrenos encontrados com sucesso.")]
        [SwaggerResponse(400, "Erro ao processar a solicitação.")]
        [SwaggerResponse(404, "Nenhum terreno encontrado dentro do raio especificado.")]
        public async Task<IActionResult> BuscarTerrenosPorLocalidadeERaio(string localidade, double raio)
        {
            double latitude, longitude;

            // Verifica se a localidade é uma coordenada no formato "latitude,longitude"
            if (IsCoordinate(localidade))
            {
                var coordenadas = localidade.Split(',');
                latitude = double.Parse(coordenadas[0], CultureInfo.InvariantCulture);
                longitude = double.Parse(coordenadas[1], CultureInfo.InvariantCulture);
            }
            else
            {
                // Caso a localidade seja um nome de local, faz a geocodificação
                var coordenadasGeocodificadas = await Geocode(localidade);
                if (coordenadasGeocodificadas == null)
                {
                    return BadRequest(new { Message = "Erro ao geocodificar a localidade fornecida." });
                }
                latitude = coordenadasGeocodificadas.Value.Latitude;
                longitude = coordenadasGeocodificadas.Value.Longitude;
            }

            // Consulta SQL para buscar terrenos dentro do raio
            var query = @"
                SELECT *,
                ST_Distance_Sphere(
                    POINT(SUBSTRING_INDEX(coordenadas, ',', -1), SUBSTRING_INDEX(coordenadas, ',', 1)),
                    POINT(@longitude, @latitude)
                ) AS distancia
                FROM terrenos
                WHERE 
                    coordenadas LIKE '%,%' AND
                    ST_Distance_Sphere(
                        POINT(SUBSTRING_INDEX(coordenadas, ',', -1), SUBSTRING_INDEX(coordenadas, ',', 1)),
                        POINT(@longitude, @latitude)
                    ) <= @raio";

            var parametros = new[]
                    {
                new MySqlParameter("@latitude", latitude),
                new MySqlParameter("@longitude", longitude),
                new MySqlParameter("@raio", raio)
            };

            var terrenos = await _context.Terrenos.FromSqlRaw(query, parametros).ToListAsync();

            if (terrenos.Count == 0)
            {
                return NotFound(new { Message = "Nenhum terreno encontrado dentro do raio especificado." });
            }

            return Ok(terrenos);
        }

        private static bool IsCoordinate(string localidade)
        {
            var parts = localidade.Split(',');
            if (parts.Length == 2)
            {
                // Usa InvariantCulture para garantir que o ponto seja tratado como separador decimal
                return double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out _) &&
                       double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            }
            return false;
        }

        private async Task<(double Latitude, double Longitude)?> Geocode(string localidade)
        {
            var httpClient = new HttpClient();
            var apiKey = "AIzaSyD6swlNTZCW8-3HrhY-0Q4JzKJvnSO3rBc"; // Substitua pela sua chave de API do Google Maps
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={localidade}&key={apiKey}";

            var response = await httpClient.GetStringAsync(url);
            var resultado = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(response);

            if (resultado.Status == "OK")
            {
                var location = resultado.Results.First().Geometry.Location;
                return (location.Lat, location.Lng);
            }

            return null; // Se não for possível geocodificar a localidade
        }
    }
}
