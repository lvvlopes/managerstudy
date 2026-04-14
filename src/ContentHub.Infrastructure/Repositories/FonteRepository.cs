using ContentHub.Application.Interfaces;
using ContentHub.Domain.Entities;
using ContentHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Infrastructure.Repositories;

public sealed class FonteRepository(ContentHubDbContext context) : IFonteRepository
{
    public async Task<IReadOnlyCollection<Fonte>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Fontes
            .AsNoTracking()
            .Include(fonte => fonte.Categoria)
            .OrderBy(fonte => fonte.Nome)
            .ToListAsync(cancellationToken);
    }

    public Task<Fonte?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return context.Fontes
            .Include(fonte => fonte.Categoria)
            .FirstOrDefaultAsync(fonte => fonte.Id == id, cancellationToken);
    }

    public Task AddAsync(Fonte fonte, CancellationToken cancellationToken = default)
    {
        return context.Fontes.AddAsync(fonte, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
