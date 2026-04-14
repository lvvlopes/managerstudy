using ContentHub.Domain.Entities;

namespace ContentHub.Application.Interfaces;

public interface ICategoriaRepository
{
    Task<IReadOnlyCollection<Categoria>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Categoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Categoria?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task AddAsync(Categoria categoria, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
