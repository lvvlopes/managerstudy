using System.Net.Http.Json;
using System.Text.Json;
using ContentHub.Web.Models;
using ContentHub.Web.ViewModels;

namespace ContentHub.Web.Services;

public sealed class ContentHubApiClient(HttpClient httpClient, ILogger<ContentHubApiClient> logger)
    : IContentHubApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public string BaseUrl => httpClient.BaseAddress?.ToString().TrimEnd('/') ?? string.Empty;
    public bool IsAvailable { get; private set; } = true;

    public async Task<IReadOnlyCollection<PostModel>> GetPostsAsync(
        string? categoria,
        CancellationToken cancellationToken = default)
    {
        var endpoint = string.IsNullOrWhiteSpace(categoria)
            ? "posts"
            : $"posts?categoria={Uri.EscapeDataString(categoria)}";

        try
        {
            using var response = await httpClient.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            IsAvailable = true;
            return DeserializePosts(document.RootElement);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            IsAvailable = false;
            logger.LogWarning(ex, "Falha ao buscar posts na API Content Hub.");
            return [];
        }
    }

    public async Task<IReadOnlyCollection<CategoriaModel>> GetCategoriasAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var categorias = await httpClient.GetFromJsonAsync<List<CategoriaModel>>("categorias", JsonOptions, cancellationToken);
            IsAvailable = true;
            return categorias ?? [];
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            IsAvailable = false;
            logger.LogWarning(ex, "Falha ao buscar categorias na API Content Hub.");
            return [];
        }
    }

    public async Task<PostModel?> GetPostAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var post = await httpClient.GetFromJsonAsync<PostModel>($"posts/{id}", JsonOptions, cancellationToken);
            IsAvailable = true;
            return post;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            IsAvailable = false;
            logger.LogWarning(ex, "Falha ao buscar post {PostId} na API Content Hub.", id);
            return null;
        }
    }

    public async Task<IReadOnlyCollection<FonteModel>> GetFontesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var fontes = await httpClient.GetFromJsonAsync<List<FonteModel>>("fontes", JsonOptions, cancellationToken);
            IsAvailable = true;
            return fontes ?? [];
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            IsAvailable = false;
            logger.LogWarning(ex, "Falha ao buscar fontes na API Content Hub.");
            return [];
        }
    }

    public async Task<ApiResult<FonteModel>> CreateFonteAsync(CriarFonteViewModel fonte, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            fonte.Nome,
            fonte.UrlPerfil,
            fonte.CategoriaId
        };

        return await PostAsync<FonteModel>("fontes", payload, cancellationToken);
    }

    public async Task<ApiResult<CategoriaModel>> CreateCategoriaAsync(
        CriarCategoriaViewModel categoria,
        CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            categoria.Nome
        };

        return await PostAsync<CategoriaModel>("categorias", payload, cancellationToken);
    }

    public async Task<ApiResult<PostModel>> CreatePostAsync(CriarPostViewModel post, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            post.FonteId,
            post.Url,
            post.Descricao,
            post.DataPost
        };

        return await PostAsync<PostModel>("posts", payload, cancellationToken);
    }

    public async Task<ApiResult<PostModel>> UpdatePostAsync(EditarPostViewModel post, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            post.FonteId,
            post.Url,
            post.Descricao,
            post.DataPost
        };

        return await PutAsync<PostModel>($"posts/{post.Id}", payload, cancellationToken);
    }

    private async Task<ApiResult<T>> PutAsync<T>(string endpoint, object payload, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PutAsJsonAsync(endpoint, payload, JsonOptions, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response, cancellationToken);
                IsAvailable = true;
                return ApiResult<T>.Fail(message);
            }

            var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
            IsAvailable = true;
            return data is null
                ? ApiResult<T>.Fail("A API não retornou dados.")
                : ApiResult<T>.Ok(data);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            IsAvailable = false;
            logger.LogWarning(ex, "Falha ao atualizar dados na API Content Hub.");
            return ApiResult<T>.Fail("Não foi possível conectar à API Content Hub.");
        }
    }

    private async Task<ApiResult<T>> PostAsync<T>(string endpoint, object payload, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(endpoint, payload, JsonOptions, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response, cancellationToken);
                IsAvailable = true;
                return ApiResult<T>.Fail(message);
            }

            var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
            IsAvailable = true;
            return data is null
                ? ApiResult<T>.Fail("A API não retornou dados.")
                : ApiResult<T>.Ok(data);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            IsAvailable = false;
            logger.LogWarning(ex, "Falha ao enviar dados para a API Content Hub.");
            return ApiResult<T>.Fail("Não foi possível conectar à API Content Hub.");
        }
    }

    private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var fallback = $"A API retornou {(int)response.StatusCode}.";
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(content))
        {
            return fallback;
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            return document.RootElement.TryGetProperty("error", out var error)
                ? error.GetString() ?? fallback
                : fallback;
        }
        catch (JsonException)
        {
            return content;
        }
    }

    private static IReadOnlyCollection<PostModel> DeserializePosts(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array)
        {
            return JsonSerializer.Deserialize<List<PostModel>>(root.GetRawText(), JsonOptions) ?? [];
        }

        if (root.ValueKind == JsonValueKind.Object &&
            root.TryGetProperty("items", out var items) &&
            items.ValueKind == JsonValueKind.Array)
        {
            return JsonSerializer.Deserialize<List<PostModel>>(items.GetRawText(), JsonOptions) ?? [];
        }

        return [];
    }
}
