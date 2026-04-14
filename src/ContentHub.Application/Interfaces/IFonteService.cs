using ContentHub.Application.DTOs;

namespace ContentHub.Application.Interfaces;

public interface IFonteService
{
    Task<IReadOnlyCollection<FonteDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FonteDto> CreateAsync(CriarFonteDto dto, CancellationToken cancellationToken = default);
}
