using ContentHub.Application.Interfaces;
using ContentHub.Domain.Entities;
using ContentHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Infrastructure.Repositories;

public sealed class PostRepository(ContentHubDbContext context) : IPostRepository
{
    public async Task<(IReadOnlyCollection<Post> Items, int TotalItems)> GetPagedAsync(
        string? categoria,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Posts
            .AsNoTracking()
            .Include(post => post.Fonte)
            .ThenInclude(fonte => fonte!.Categoria)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            var categoriaNormalizada = categoria.Trim().ToLower();
            query = query.Where(post => post.Fonte != null &&
                                        post.Fonte.Categoria != null &&
                                        post.Fonte.Categoria.Nome.ToLower() == categoriaNormalizada);
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(post => post.DataCadastro)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalItems);
    }

    public Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return context.Posts
            .Include(post => post.Fonte)
            .ThenInclude(fonte => fonte!.Categoria)
            .FirstOrDefaultAsync(post => post.Id == id, cancellationToken);
    }

    public Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        return context.Posts.AddAsync(post, cancellationToken).AsTask();
    }

    public Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        context.Posts.Update(post);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
