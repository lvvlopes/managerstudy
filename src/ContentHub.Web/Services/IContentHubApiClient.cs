using ContentHub.Web.Models;
using ContentHub.Web.ViewModels;

namespace ContentHub.Web.Services;

public interface IContentHubApiClient
{
    string BaseUrl { get; }
    bool IsAvailable { get; }

    Task<IReadOnlyCollection<PostModel>> GetPostsAsync(
        string? categoria,
        CancellationToken cancellationToken = default);

    Task<PostModel?> GetPostAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CategoriaModel>> GetCategoriasAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FonteModel>> GetFontesAsync(CancellationToken cancellationToken = default);
    Task<ApiResult<CategoriaModel>> CreateCategoriaAsync(CriarCategoriaViewModel categoria, CancellationToken cancellationToken = default);
    Task<ApiResult<FonteModel>> CreateFonteAsync(CriarFonteViewModel fonte, CancellationToken cancellationToken = default);
    Task<ApiResult<PostModel>> CreatePostAsync(CriarPostViewModel post, CancellationToken cancellationToken = default);
    Task<ApiResult<PostModel>> UpdatePostAsync(EditarPostViewModel post, CancellationToken cancellationToken = default);
}
