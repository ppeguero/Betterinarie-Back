using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Betterinarie_Back.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nuevosCambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dosis",
                table: "Medicamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Medicamentos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Medicamentos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Medicamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dosis",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                table: "Medicamentos");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Medicamentos");
        }
    }
}
