namespace ContentHub.Application.DTOs;

public sealed record PdfExplanationRequest(
    int PostId,
    string NomeDocumento,
    string ConteudoPdf);

public sealed record Topico(
    string Titulo,
    string Descricao,
    List<string> Exemplos);

public sealed record PdfExplanationResponse(
    int PostId,
    string NomeDocumento,
    string ResumoExecutivo,
    List<Topico> Topicos,
    string AuxiliarParicipantes,
    List<string> PontosPrincipais);
