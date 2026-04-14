using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using ContentHub.Domain.Entities;

namespace ContentHub.Application.Services;

public sealed class PostService(
    IPostRepository postRepository,
    IFonteRepository fonteRepository) : IPostService
{
    private const int MaxPageSize = 100;

    public async Task<PagedResultDto<PostDto>> GetPagedAsync(
        string? categoria,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : Math.Min(pageSize, MaxPageSize);

        var (posts, totalItems) = await postRepository.GetPagedAsync(categoria, page, pageSize, cancellationToken);
        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedResultDto<PostDto>(
            posts.Select(MapToDto).ToList(),
            page,
            pageSize,
            totalItems,
            totalPages);
    }

    public async Task<PostDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken);
        if (post is null)
        {
            throw new KeyNotFoundException("Post não encontrado.");
        }

        return MapToDto(post);
    }

    public async Task<PostDto> CreateAsync(CriarPostDto dto, CancellationToken cancellationToken = default)
    {
        var fonte = await ValidateFonteAndUrlAsync(dto.FonteId, dto.Url, cancellationToken);

        var post = new Post
        {
            FonteId = dto.FonteId,
            Fonte = fonte,
            Url = dto.Url.Trim(),
            Descricao = string.IsNullOrWhiteSpace(dto.Descricao) ? null : dto.Descricao.Trim(),
            DataPost = dto.DataPost,
            DataCadastro = DateTime.UtcNow
        };

        await postRepository.AddAsync(post, cancellationToken);
        await postRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(post);
    }

    public async Task<PostDto> UpdateAsync(int id, AtualizarPostDto dto, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetByIdAsync(id, cancellationToken);
        if (post is null)
        {
            throw new KeyNotFoundException("Post não encontrado.");
        }

        var fonte = await ValidateFonteAndUrlAsync(dto.FonteId, dto.Url, cancellationToken);

        post.FonteId = dto.FonteId;
        post.Fonte = fonte;
        post.Url = dto.Url.Trim();
        post.Descricao = string.IsNullOrWhiteSpace(dto.Descricao) ? null : dto.Descricao.Trim();
        post.DataPost = dto.DataPost;

        await postRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(post);
    }

    private async Task<Fonte> ValidateFonteAndUrlAsync(
        int fonteId,
        string url,
        CancellationToken cancellationToken)
    {
        if (!IsValidUrl(url))
        {
            throw new ArgumentException("URL do post é inválida.");
        }

        var fonte = await fonteRepository.GetByIdAsync(fonteId, cancellationToken);
        if (fonte is null)
        {
            throw new KeyNotFoundException("Fonte não encontrada.");
        }

        return fonte;
    }

    private static bool IsValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url) ||
            !Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return false;
        }

        return true;
    }

    private static PostDto MapToDto(Post post) => new(
        post.Id,
        post.FonteId,
        post.Fonte?.Nome ?? string.Empty,
        post.Fonte?.Categoria?.Nome ?? string.Empty,
        post.Url,
        post.Descricao,
        post.DataPost,
        post.DataCadastro);
}
