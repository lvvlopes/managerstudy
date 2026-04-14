using ContentHub.Web.Models;

namespace ContentHub.Web.ViewModels;

public sealed record FeedViewModel(
    IReadOnlyCollection<PostModel> Posts,
    IReadOnlyCollection<CategoryFilterViewModel> Categorias,
    string? CategoriaSelecionada,
    string ApiBaseUrl,
    bool ApiDisponivel);
