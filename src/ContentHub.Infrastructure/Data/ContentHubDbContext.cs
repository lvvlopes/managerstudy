using ContentHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Infrastructure.Data;

public sealed class ContentHubDbContext(DbContextOptions<ContentHubDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Fonte> Fontes => Set<Fonte>();
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasKey(categoria => categoria.Id);
            entity.Property(categoria => categoria.Nome).HasMaxLength(100).IsRequired();
            entity.HasIndex(categoria => categoria.Nome).IsUnique();

            entity.HasData(
                new Categoria { Id = 1, Nome = "Inglês" },
                new Categoria { Id = 2, Nome = "Tech" },
                new Categoria { Id = 3, Nome = "Notícias" });
        });

        modelBuilder.Entity<Fonte>(entity =>
        {
            entity.ToTable("Fontes");
            entity.HasKey(fonte => fonte.Id);
            entity.Property(fonte => fonte.Nome).HasMaxLength(150).IsRequired();
            entity.Property(fonte => fonte.UrlPerfil).HasMaxLength(500).IsRequired();

            entity.HasOne(fonte => fonte.Categoria)
                .WithMany(categoria => categoria.Fontes)
                .HasForeignKey(fonte => fonte.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");
            entity.HasKey(post => post.Id);
            entity.Property(post => post.Url).HasMaxLength(1000).IsRequired();
            entity.Property(post => post.Descricao).HasMaxLength(2000);
            entity.Property(post => post.DataCadastro).IsRequired();

            entity.HasOne(post => post.Fonte)
                .WithMany(fonte => fonte.Posts)
                .HasForeignKey(post => post.FonteId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
