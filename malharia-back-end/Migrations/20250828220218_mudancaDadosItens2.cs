using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace malharia_back_end.Migrations
{
    /// <inheritdoc />
    public partial class mudancaDadosItens2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemConferencia",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TemCorte",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TemCostura",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TemDobragem",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemConferencia",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemCorte",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemCostura",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemDobragem",
                table: "ItensPedidos");
        }
    }
}
