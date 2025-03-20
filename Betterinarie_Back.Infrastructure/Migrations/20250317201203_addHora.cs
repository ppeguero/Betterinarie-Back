using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Betterinarie_Back.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addHora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Mascotas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PublicIdImagen",
                table: "Mascotas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "URLImagen",
                table: "Mascotas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Hora",
                table: "Consultas",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "PublicIdImagen",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "URLImagen",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "Hora",
                table: "Consultas");
        }
    }
}
