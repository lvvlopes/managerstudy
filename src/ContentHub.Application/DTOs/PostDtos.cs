namespace ContentHub.Application.DTOs;

public sealed record PostDto(
    int Id,
    int FonteId,
    string Fonte,
    string Categoria,
    string Url,
    string? Descricao,
    DateTime? DataPost,
    DateTime DataCadastro);

public sealed record CriarPostDto(
    int FonteId,
    string Url,
    string? Descricao,
    DateTime? DataPost);

public sealed record AtualizarPostDto(
    int FonteId,
    string Url,
    string? Descricao,
    DateTime? DataPost);

public sealed record PagedResultDto<T>(
    IReadOnlyCollection<T> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages);
