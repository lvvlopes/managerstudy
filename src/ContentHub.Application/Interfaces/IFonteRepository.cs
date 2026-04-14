using ContentHub.Domain.Entities;

namespace ContentHub.Application.Interfaces;

public interface IFonteRepository
{
    Task<IReadOnlyCollection<Fonte>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Fonte?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Fonte fonte, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
