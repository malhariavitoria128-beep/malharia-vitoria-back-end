using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace malharia_back_end.Migrations
{
    /// <inheritdoc />
    public partial class pedidoNumeroPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroPedido",
                table: "Pedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroPedido",
                table: "Pedidos");
        }
    }
}
