using ContentHub.Application.Interfaces;
using ContentHub.Domain.Entities;
using ContentHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Infrastructure.Repositories;

public sealed class CategoriaRepository(ContentHubDbContext context) : ICategoriaRepository
{
    public async Task<IReadOnlyCollection<Categoria>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Nome)
            .ToListAsync(cancellationToken);
    }

    public Task<Categoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id, cancellationToken);
    }

    public Task<Categoria?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        return context.Categorias
            .FirstOrDefaultAsync(categoria => categoria.Nome.ToLower() == nome.ToLower(), cancellationToken);
    }

    public Task AddAsync(Categoria categoria, CancellationToken cancellationToken = default)
    {
        return context.Categorias.AddAsync(categoria, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
