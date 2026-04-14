using System;
using ContentHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContentHub.Infrastructure.Migrations;

[DbContext(typeof(ContentHubDbContext))]
partial class ContentHubDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("ContentHub.Domain.Entities.Categoria", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Nome")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.HasKey("Id");
            b.HasIndex("Nome").IsUnique();
            b.ToTable("Categorias", (string)null);

            b.HasData(
                new { Id = 1, Nome = "Inglês" },
                new { Id = 2, Nome = "Tech" },
                new { Id = 3, Nome = "Notícias" });
        });

        modelBuilder.Entity("ContentHub.Domain.Entities.Fonte", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<int>("CategoriaId").HasColumnType("int");

            b.Property<string>("Nome")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.Property<string>("UrlPerfil")
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            b.HasKey("Id");
            b.HasIndex("CategoriaId");
            b.ToTable("Fontes", (string)null);
        });

        modelBuilder.Entity("ContentHub.Domain.Entities.Post", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<DateTime>("DataCadastro").HasColumnType("datetime2");
            b.Property<DateTime?>("DataPost").HasColumnType("datetime2");

            b.Property<string>("Descricao")
                .HasMaxLength(2000)
                .HasColumnType("nvarchar(2000)");

            b.Property<int>("FonteId").HasColumnType("int");

            b.Property<string>("Url")
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            b.HasKey("Id");
            b.HasIndex("FonteId");
            b.ToTable("Posts", (string)null);
        });

        modelBuilder.Entity("ContentHub.Domain.Entities.Fonte", b =>
        {
            b.HasOne("ContentHub.Domain.Entities.Categoria", "Categoria")
                .WithMany("Fontes")
                .HasForeignKey("CategoriaId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Categoria");
        });

        modelBuilder.Entity("ContentHub.Domain.Entities.Post", b =>
        {
            b.HasOne("ContentHub.Domain.Entities.Fonte", "Fonte")
                .WithMany("Posts")
                .HasForeignKey("FonteId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Fonte");
        });

        modelBuilder.Entity("ContentHub.Domain.Entities.Categoria", b =>
        {
            b.Navigation("Fontes");
        });

        modelBuilder.Entity("ContentHub.Domain.Entities.Fonte", b =>
        {
            b.Navigation("Posts");
        });
#pragma warning restore 612, 618
    }
}
