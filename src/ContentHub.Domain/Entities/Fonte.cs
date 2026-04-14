namespace ContentHub.Domain.Entities;

public sealed class Fonte
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string UrlPerfil { get; set; } = string.Empty;
    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
