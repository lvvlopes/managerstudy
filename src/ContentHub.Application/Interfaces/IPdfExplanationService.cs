using ContentHub.Application.DTOs;

namespace ContentHub.Application.Interfaces;

public interface IPdfExplanationService
{
    Task<PdfExplanationResponse> ExplainPdfAsync(byte[] pdfContent, string documentName, CancellationToken cancellationToken = default);
    string ExtractTextFromPdf(byte[] pdfContent);
}
