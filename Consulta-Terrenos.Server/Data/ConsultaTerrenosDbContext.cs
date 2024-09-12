using Consulta_Terrenos.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Consulta_Terrenos.Server.Data
{
    public class ConsultaTerrenosDbContext(DbContextOptions<ConsultaTerrenosDbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Terrenos> Terrenos { get; set; }
        public DbSet<Consultas> Consultas{ get; set; }
        public DbSet<Favoritos> Favoritos { get; set; }
    }
}
