using System.Text.Json.Serialization;

namespace Consulta_Terrenos.Server.Models
{
    public class Consultas
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int UsersId { get; set; }
        public int TerrenosId { get; set; }
        public DateTime DataConsulta { get; set; }

        [JsonIgnore]
        public Users? Users { get; set; }
        [JsonIgnore]
        public Terrenos? Terrenos { get; set; }
    }
}
