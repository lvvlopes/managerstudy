using ContentHub.Domain.Entities;

namespace ContentHub.Application.Interfaces;

public interface IPostRepository
{
    Task<(IReadOnlyCollection<Post> Items, int TotalItems)> GetPagedAsync(
        string? categoria,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    Task UpdateAsync(Post post, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
