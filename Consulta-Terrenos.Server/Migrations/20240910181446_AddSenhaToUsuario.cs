using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consulta_Terrenos.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSenhaToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenhaCriptografada",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenhaCriptografada",
                table: "Users");
        }
    }
}
