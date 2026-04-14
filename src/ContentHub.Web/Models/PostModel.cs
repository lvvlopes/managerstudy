namespace ContentHub.Web.Models;

public sealed class PostModel
{
    public int Id { get; set; }
    public int FonteId { get; set; }
    public string? Descricao { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Fonte { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime? DataPost { get; set; }
    public string? ImagemUrl { get; set; }

    public string CategoriaDisplay => Categoria.Equals("Tech", StringComparison.OrdinalIgnoreCase)
        ? "Tecnologia"
        : Categoria;

    public string ResolvedImagemUrl => !string.IsNullOrWhiteSpace(ImagemUrl)
        ? ImagemUrl
        : CategoriaDisplay switch
        {
            "Inglês" => "https://images.unsplash.com/photo-1456513080510-7bf3a84b82f8?auto=format&fit=crop&w=900&q=80",
            "Tecnologia" => "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=900&q=80",
            "Notícias" => "https://images.unsplash.com/photo-1495020689067-958852a7765e?auto=format&fit=crop&w=900&q=80",
            _ => "https://images.unsplash.com/photo-1492724441997-5dc865305da7?auto=format&fit=crop&w=900&q=80"
        };
}
