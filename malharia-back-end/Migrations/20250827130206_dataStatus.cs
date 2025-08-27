using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace malharia_back_end.Migrations
{
    /// <inheritdoc />
    public partial class dataStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataEntrega",
                table: "Pedidos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Pedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEntrega",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pedidos");
        }
    }
}
