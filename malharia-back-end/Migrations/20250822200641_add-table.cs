using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace malharia_back_end.Migrations
{
    /// <inheritdoc />
    public partial class addtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Cliente_ClienteId",
                table: "Pedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente");

            migrationBuilder.RenameTable(
                name: "Cliente",
                newName: "Clientes");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_Nome",
                table: "Clientes",
                newName: "IX_Clientes_Nome");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_CPF",
                table: "Clientes",
                newName: "IX_Clientes_CPF");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_CNPJ",
                table: "Clientes",
                newName: "IX_Clientes_CNPJ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Clientes_ClienteId",
                table: "Pedido",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Clientes_ClienteId",
                table: "Pedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "Cliente");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_Nome",
                table: "Cliente",
                newName: "IX_Cliente_Nome");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_CPF",
                table: "Cliente",
                newName: "IX_Cliente_CPF");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_CNPJ",
                table: "Cliente",
                newName: "IX_Cliente_CNPJ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Cliente_ClienteId",
                table: "Pedido",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
