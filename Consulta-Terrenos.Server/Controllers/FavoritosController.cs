using Consulta_Terrenos.Server.Data;
using Consulta_Terrenos.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Consulta_Terrenos.Server.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os favoritos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("API responsável pelo gerenciamento de favoritos, incluindo cadastro e consulta.")]
    public class FavoritosController(ConsultaTerrenosDbContext context) : Controller
    {
        private readonly ConsultaTerrenosDbContext _context = context;

        /// <summary>
        /// Marca ou remove um terreno como favorito para o usuário.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite que um usuário marque ou remova um terreno como favorito.
        /// O terreno deve ser passado no corpo da requisição.
        /// </remarks>
        /// <param name="favorito">Objeto Favorito contendo o Id do terreno e o Id do usuário.</param>
        /// <returns>Retorna o status da operação.</returns>
        /// <response code="200">Favorito atualizado com sucesso.</response>
        /// <response code="400">Erro ao atualizar o favorito.</response>
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Marcar ou remover terreno como favorito", Description = "Permite ao usuário marcar ou remover um terreno como favorito.")]
        [SwaggerResponse(200, "Favorito atualizado com sucesso.")]
        [SwaggerResponse(400, "Erro ao atualizar o favorito.")]
        public async Task<IActionResult> MarcarFavorito([FromBody] Favoritos favorito)
        {
            try
            {
                var favoritoExistente = await _context.Favoritos
                    .FirstOrDefaultAsync(f => f.UsersId == favorito.UsersId && f.TerrenosId == favorito.TerrenosId);

                if (favoritoExistente != null)
                {
                    // Remove dos favoritos se já estiver marcado
                    _context.Favoritos.Remove(favoritoExistente);
                    await _context.SaveChangesAsync();
                    return Ok(new { Message = "Terreno removido dos favoritos." });
                }
                else
                {
                    // Adiciona aos favoritos se ainda não estiver marcado
                    _context.Favoritos.Add(favorito);
                    await _context.SaveChangesAsync();
                    return Ok(new { Message = "Terreno marcado como favorito." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Erro ao atualizar favorito.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Lista os terrenos favoritos de um usuário.
        /// </summary>
        /// <param name="userId">O ID do usuário cujos favoritos serão listados.</param>
        /// <returns>Lista dos terrenos favoritos do usuário.</returns>
        /// <response code="200">Retorna a lista de terrenos favoritos.</response>
        /// <response code="404">Nenhum favorito encontrado para o usuário.</response>
        [Authorize]
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Listar terrenos favoritos", Description = "Retorna uma lista dos terrenos favoritos de um usuário específico.")]
        [SwaggerResponse(200, "Lista de terrenos favoritos retornada com sucesso.", typeof(List<Favoritos>))]
        [SwaggerResponse(404, "Nenhum favorito encontrado para o usuário.")]
        public async Task<IActionResult> ListarFavoritos(int userId)
        {
            var favoritos = await _context.Favoritos
                .Where(f => f.UsersId == userId)
                .Include(f => f.Terrenos) // Faz o join com a tabela de terrenos
                .Select(f => new
                {
                    f.Id,
                    f.UsersId,
                    Terrenos = new
                    {
                        f.Terrenos.Id,
                        f.Terrenos.Coordenadas,
                        f.Terrenos.Tamanho,
                        f.Terrenos.Preco,
                        f.Terrenos.TipoUso
                    }
                })
                .ToListAsync();

            if (!favoritos.Any())
            {
                return NotFound(new { Message = "Nenhum favorito encontrado para o usuário." });
            }

            return Ok(favoritos);
        }
    }
}
