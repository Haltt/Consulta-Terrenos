using Consulta_Terrenos.Server.Data;
using Consulta_Terrenos.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Consulta_Terrenos.Server.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar as consultas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("API responsável pelo gerenciamento de consultas.")]
    public class ConsultasController(ConsultaTerrenosDbContext context) : Controller
    {
        private readonly ConsultaTerrenosDbContext _context = context;

        /// <summary>
        /// Registra uma nova consulta de terreno.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite registrar a consulta de um usuário ao visualizar os detalhes de um terreno.
        /// A consulta deve conter o ID do terreno e o ID do usuário que realizou a consulta.
        /// </remarks>
        /// <param name="consulta">Objeto Consulta contendo o ID do terreno e o ID do usuário.</param>
        /// <returns>Retorna os detalhes da consulta registrada.</returns>
        /// <response code="200">Consulta registrada com sucesso.</response>
        /// <response code="400">Erro ao registrar a consulta.</response>
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Registrar consulta de terreno", Description = "Registra a consulta de um usuário ao visualizar os detalhes de um terreno.")]
        [SwaggerResponse(200, "Consulta registrada com sucesso.", typeof(Consultas))]
        [SwaggerResponse(400, "Erro ao registrar a consulta.")]
        public async Task<IActionResult> RegistrarConsulta([FromBody] Consultas consulta)
        {
            try
            {
                consulta.DataConsulta = DateTime.UtcNow; // Registra o horário da consulta
                _context.Consultas.Add(consulta);
                await _context.SaveChangesAsync();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Erro ao registrar consulta.", Details = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Consultar as consultas e favoritos do usuário nos últimos 3 meses", Description = "Retorna as consultas de um usuário nos últimos 3 meses, consolidadas por mês, e também a quantidade de terrenos favoritos.")]
        [SwaggerResponse(200, "Dados retornados com sucesso.")]
        [SwaggerResponse(404, "Nenhuma consulta ou favorito encontrado para o usuário.")]
        public async Task<IActionResult> ConsultarConsultasEFavoritos(int userId)
        {
            var dataLimite = DateTime.UtcNow.AddMonths(-3);

            // Consulta para buscar as consultas do usuário nos últimos 3 meses, agrupadas por mês
            var consultas = await _context.Consultas
                .Where(c => c.UsersId == userId && c.DataConsulta >= dataLimite)
                .GroupBy(c => new
                {
                    Ano = c.DataConsulta.Year,
                    Mes = c.DataConsulta.Month
                })
                .Select(g => new
                {
                    Ano = g.Key.Ano,
                    Mes = g.Key.Mes,
                    QuantidadeConsultas = g.Count()
                })
                .OrderBy(g => g.Ano).ThenBy(g => g.Mes)
                .ToListAsync();

            // Consulta para contar os terrenos favoritos
            var quantidadeFavoritos = await _context.Favoritos
                .Where(f => f.UsersId == userId)
                .CountAsync();

            if (!consultas.Any())
            {
                return NotFound(new { Message = "Nenhuma consulta encontrada nos últimos 3 meses." });
            }

            return Ok(new
            {
                Consultas = consultas,
                QuantidadeFavoritos = quantidadeFavoritos
            });
        }
    }
}
