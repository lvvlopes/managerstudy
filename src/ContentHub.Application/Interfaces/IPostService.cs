using ContentHub.Application.DTOs;

namespace ContentHub.Application.Interfaces;

public interface IPostService
{
    Task<PagedResultDto<PostDto>> GetPagedAsync(
        string? categoria,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PostDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PostDto> CreateAsync(CriarPostDto dto, CancellationToken cancellationToken = default);
    Task<PostDto> UpdateAsync(int id, AtualizarPostDto dto, CancellationToken cancellationToken = default);
}
