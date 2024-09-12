using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Consulta_Terrenos.Server.Models
{
    public class Favoritos
    {
        public int Id { get; set; }
        public int UsersId { get; set; }
        public int TerrenosId { get; set; }

        [JsonIgnore]
        public Users? Users { get; set; }
        [JsonIgnore]
        public Terrenos? Terrenos { get; set; }
    }
}
