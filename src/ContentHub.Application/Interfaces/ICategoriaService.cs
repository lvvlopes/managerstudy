using ContentHub.Application.DTOs;

namespace ContentHub.Application.Interfaces;

public interface ICategoriaService
{
    Task<IReadOnlyCollection<CategoriaDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoriaDto> CreateAsync(CriarCategoriaDto dto, CancellationToken cancellationToken = default);
}
