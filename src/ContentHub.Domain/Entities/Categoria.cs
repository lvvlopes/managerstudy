namespace ContentHub.Domain.Entities;

public sealed class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public ICollection<Fonte> Fontes { get; set; } = new List<Fonte>();
}
