using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ConteudoPdf",
                table: "Posts",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUploadPdf",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExplicacaoPdfJson",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeArquivoPdf",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConteudoPdf",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DataUploadPdf",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ExplicacaoPdfJson",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "NomeArquivoPdf",
                table: "Posts");
        }
    }
}
