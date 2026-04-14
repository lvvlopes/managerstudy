using ContentHub.Web.Models;

namespace ContentHub.Web.ViewModels;

public sealed record CategoryFilterViewModel(
    string DisplayName,
    string QueryValue,
    bool IsActive)
{
    public static IReadOnlyCollection<CategoryFilterViewModel> Build(
        IReadOnlyCollection<CategoriaModel> categorias,
        string? categoriaSelecionada)
    {
        var nomes = categorias.Count > 0
            ? categorias.Select(categoria => categoria.Nome)
            : ["Inglês", "Tech", "Notícias"];

        var filtros = nomes
            .Select(nome =>
            {
                var displayName = ToDisplayName(nome);
                var queryValue = ToQueryValue(nome);
                var isActive = IsSelected(categoriaSelecionada, displayName, queryValue);

                return new CategoryFilterViewModel(displayName, queryValue, isActive);
            })
            .DistinctBy(filtro => filtro.QueryValue)
            .OrderBy(filtro => filtro.DisplayName)
            .ToList();

        return filtros;
    }

    private static string ToDisplayName(string nome) =>
        nome.Equals("Tech", StringComparison.OrdinalIgnoreCase) ? "Tecnologia" : nome;

    private static string ToQueryValue(string nome) =>
        nome.Equals("Tecnologia", StringComparison.OrdinalIgnoreCase) ? "Tech" : nome;

    private static bool IsSelected(string? categoriaSelecionada, string displayName, string queryValue)
    {
        if (string.IsNullOrWhiteSpace(categoriaSelecionada))
        {
            return false;
        }

        return categoriaSelecionada.Equals(displayName, StringComparison.OrdinalIgnoreCase) ||
               categoriaSelecionada.Equals(queryValue, StringComparison.OrdinalIgnoreCase);
    }
}
