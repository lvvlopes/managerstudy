namespace ContentHub.Domain.Entities;

public sealed class Post
{
    public int Id { get; set; }
    public int FonteId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime? DataPost { get; set; }
    public DateTime DataCadastro { get; set; }

    public Fonte? Fonte { get; set; }
}
