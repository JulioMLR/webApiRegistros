using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webApiRegistros.Migrations
{
    /// <inheritdoc />
    public partial class Objeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idObjeto",
                table: "Registros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idObjeto",
                table: "Registros");
        }
    }
}
