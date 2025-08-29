using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace malharia_back_end.Migrations
{
    /// <inheritdoc />
    public partial class dadosItens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Prioridade",
                table: "ItensPedidos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Retirada",
                table: "ItensPedidos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StatusBordado",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusConferencia",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusCorte",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusCostura",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusDobragem",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusDtf",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusPintura",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusSilk",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusSublimacao",
                table: "ItensPedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "TemBordado",
                table: "ItensPedidos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TemDtf",
                table: "ItensPedidos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TemPintura",
                table: "ItensPedidos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TemSilk",
                table: "ItensPedidos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TemSublimacao",
                table: "ItensPedidos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "Retirada",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusBordado",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusConferencia",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusCorte",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusCostura",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusDobragem",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusDtf",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusPintura",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusSilk",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "StatusSublimacao",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemBordado",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemDtf",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemPintura",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemSilk",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "TemSublimacao",
                table: "ItensPedidos");
        }
    }
}
