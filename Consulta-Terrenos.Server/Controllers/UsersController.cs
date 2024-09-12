using Consulta_Terrenos.Server.Data;
using Consulta_Terrenos.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Consulta_Terrenos.Server.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("API responsável pelo gerenciamento de usuários, incluindo cadastro e consultas.")]
    public class UsersController(ConsultaTerrenosDbContext context) : Controller
    {
        private readonly ConsultaTerrenosDbContext _context = context;

        /// Obter usuário pelo ID.
        /// </summary>
        /// <remarks>
        /// Retorna as informações de um usuário específico usando o ID fornecido.
        /// </remarks>
        /// <param name="id">ID do usuário que será retornado.</param>
        /// <returns>Retorna os detalhes do usuário.</returns>
        /// <response code="200">Usuário encontrado com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter usuário pelo ID", Description = "Retorna as informações de um usuário específico usando o ID.")]
        [SwaggerResponse(200, "Usuário encontrado com sucesso.", typeof(Users))]
        [SwaggerResponse(404, "Usuário não encontrado.")]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        /// <param name="request">Objeto contendo as informações do usuário (Nome, Email, Área de Interesse, Senha).</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Usuário cadastrado com sucesso.</response>
        /// <response code="400">Erro de validação ou e-mail já existente.</response>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(200, "Usuário cadastrado com sucesso.")]
        [SwaggerResponse(400, "Erro de validação ou e-mail já existente.")] 
        public async Task<IActionResult> CadastrarUsuario([FromBody] Users request)
        {
            // Verifica se já existe um usuário com o mesmo e-mail
            var usuarioExistente = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuarioExistente != null)
            {
                return BadRequest(new { Message = "Este e-mail já está em uso." });
            }

            var passwordHasher = new PasswordHasher<Users>();
            var usuario = new Users
            {
                Nome = request.Nome,
                Email = request.Email,
                AreaInteresse = request.AreaInteresse,
                SenhaCriptografada = passwordHasher.HashPassword(null, request.SenhaCriptografada)
            };

            // Adiciona o novo usuário ao banco de dados
            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Usuário cadastrado com sucesso!" });
        }

        /// <summary>
        /// Login de usuário.
        /// </summary>
        /// <remarks>
        /// Permite que um usuário faça login utilizando seu e-mail e senha, retornando um token JWT se o login for bem-sucedido.
        /// </remarks>
        /// <param name="request">Credenciais de login (e-mail e senha).</param>
        /// <returns>Retorna um token JWT em caso de sucesso.</returns>
        /// <response code="200">Login bem-sucedido, token JWT retornado.</response>
        /// <response code="401">Usuário não encontrado ou senha inválida.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login de usuário", Description = "Permite que um usuário faça login e obtenha um token JWT.")]
        [SwaggerResponse(200, "Login bem-sucedido.", typeof(TokenResponse))]
        [SwaggerResponse(401, "Usuário não encontrado ou senha inválida.")]
        public IActionResult Login([FromBody] Login request)
        {
            var usuario = _context.Users.SingleOrDefault(u => u.Email == request.Email);
            if (usuario == null) return Unauthorized(new { Message = "Usuário não encontrado" });

            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaCriptografada, request.Senha);

            if (result == PasswordVerificationResult.Success)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("W3443FDFDF34DF34343fdf344SDTSDFSDFSDFSHA4545354345SDFGDFGDFLTTGdffgfdGDFGD17");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", usuario.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized(new { Message = "Senha inválida" });
        }
    }
}
