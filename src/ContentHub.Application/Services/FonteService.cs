using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using ContentHub.Domain.Entities;

namespace ContentHub.Application.Services;

public sealed class FonteService(
    IFonteRepository fonteRepository,
    ICategoriaRepository categoriaRepository) : IFonteService
{
    public async Task<IReadOnlyCollection<FonteDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var fontes = await fonteRepository.GetAllAsync(cancellationToken);
        return fontes.Select(MapToDto).ToList();
    }

    public async Task<FonteDto> CreateAsync(CriarFonteDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            throw new ArgumentException("Nome da fonte é obrigatório.");
        }

        if (!Uri.TryCreate(dto.UrlPerfil, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("URL do perfil é inválida.");
        }

        var categoria = await categoriaRepository.GetByIdAsync(dto.CategoriaId, cancellationToken);
        if (categoria is null)
        {
            throw new KeyNotFoundException("Categoria não encontrada.");
        }

        var fonte = new Fonte
        {
            Nome = dto.Nome.Trim(),
            UrlPerfil = dto.UrlPerfil.Trim(),
            CategoriaId = dto.CategoriaId,
            Categoria = categoria
        };

        await fonteRepository.AddAsync(fonte, cancellationToken);
        await fonteRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(fonte);
    }

    private static FonteDto MapToDto(Fonte fonte) => new(
        fonte.Id,
        fonte.Nome,
        fonte.UrlPerfil,
        fonte.CategoriaId,
        fonte.Categoria?.Nome ?? string.Empty);
}
