using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentHub.Infrastructure.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categorias",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categorias", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Fontes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                UrlPerfil = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                CategoriaId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Fontes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Fontes_Categorias_CategoriaId",
                    column: x => x.CategoriaId,
                    principalTable: "Categorias",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Posts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FonteId = table.Column<int>(type: "int", nullable: false),
                Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                DataPost = table.Column<DateTime>(type: "datetime2", nullable: true),
                DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Posts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Posts_Fontes_FonteId",
                    column: x => x.FonteId,
                    principalTable: "Fontes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Categorias",
            columns: new[] { "Id", "Nome" },
            values: new object[,]
            {
                { 1, "Inglês" },
                { 2, "Tech" },
                { 3, "Notícias" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Categorias_Nome",
            table: "Categorias",
            column: "Nome",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Fontes_CategoriaId",
            table: "Fontes",
            column: "CategoriaId");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_FonteId",
            table: "Posts",
            column: "FonteId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Posts");
        migrationBuilder.DropTable(name: "Fontes");
        migrationBuilder.DropTable(name: "Categorias");
    }
}
