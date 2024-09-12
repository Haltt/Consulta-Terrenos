using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consulta_Terrenos.Server.Models
{
    public class Users
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string AreaInteresse { get; set; }
        public string SenhaCriptografada { get; set; }
    }

    public class Login
    {
        [NotMapped]
        public string Email { get; set; }
        [NotMapped]
        public string Senha { get; set; }
    }

    public class TokenResponse
    {
        [NotMapped]
        public string Token { get; set; }
    }
}
