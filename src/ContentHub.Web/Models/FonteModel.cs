namespace ContentHub.Web.Models;

public sealed class FonteModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string UrlPerfil { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string Categoria { get; set; } = string.Empty;

    public string DisplayName => string.IsNullOrWhiteSpace(Categoria)
        ? Nome
        : $"{Nome} · {Categoria}";
}
