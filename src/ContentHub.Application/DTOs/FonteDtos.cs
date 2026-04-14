namespace ContentHub.Application.DTOs;

public sealed record FonteDto(
    int Id,
    string Nome,
    string UrlPerfil,
    int CategoriaId,
    string Categoria);

public sealed record CriarFonteDto(
    string Nome,
    string UrlPerfil,
    int CategoriaId);
