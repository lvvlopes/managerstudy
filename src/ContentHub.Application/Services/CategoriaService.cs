using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using ContentHub.Domain.Entities;

namespace ContentHub.Application.Services;

public sealed class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{
    public async Task<IReadOnlyCollection<CategoriaDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categorias = await categoriaRepository.GetAllAsync(cancellationToken);
        return categorias.Select(MapToDto).ToList();
    }

    public async Task<CategoriaDto> CreateAsync(CriarCategoriaDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            throw new ArgumentException("Nome da categoria é obrigatório.");
        }

        var nome = dto.Nome.Trim();
        var categoriaExistente = await categoriaRepository.GetByNomeAsync(nome, cancellationToken);
        if (categoriaExistente is not null)
        {
            throw new InvalidOperationException("Categoria já cadastrada.");
        }

        var categoria = new Categoria { Nome = nome };

        await categoriaRepository.AddAsync(categoria, cancellationToken);
        await categoriaRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(categoria);
    }

    private static CategoriaDto MapToDto(Categoria categoria) => new(categoria.Id, categoria.Nome);
}
