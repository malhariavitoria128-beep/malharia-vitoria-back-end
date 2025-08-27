using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace malharia_back_end.Migrations
{
    /// <inheritdoc />
    public partial class additemPedidorestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedidos_Pedidos_PedidoId",
                table: "ItensPedidos");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedidos_Pedidos_PedidoId",
                table: "ItensPedidos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedidos_Pedidos_PedidoId",
                table: "ItensPedidos");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedidos_Pedidos_PedidoId",
                table: "ItensPedidos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
