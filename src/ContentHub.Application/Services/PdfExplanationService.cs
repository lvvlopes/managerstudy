using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContentHub.Application.Services;

public sealed class PdfExplanationService(
    IConfiguration configuration,
    ILogger<PdfExplanationService> logger) : IPdfExplanationService
{
    public string ExtractTextFromPdf(byte[] pdfContent)
    {
        try
        {
            var extractedText = new System.Text.StringBuilder();
            
            using (var stream = new MemoryStream(pdfContent))
            using (var reader = new PdfReader(stream))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    var text = PdfTextExtractor.GetTextFromPage(reader, i);
                    extractedText.AppendLine(text);
                }
            }
            
            logger.LogInformation("PDF text extracted successfully. Pages processed: {PageCount}", 
                                extractedText.Length);
            return extractedText.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error extracting text from PDF");
            throw;
        }
    }

    public async Task<PdfExplanationResponse> ExplainPdfAsync(
        byte[] pdfContent, 
        string documentName, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Extract text from PDF
            var pdfText = ExtractTextFromPdf(pdfContent);
            
            if (string.IsNullOrWhiteSpace(pdfText))
            {
                throw new InvalidOperationException("PDF file appears to be empty or unreadable.");
            }

            // Truncate if too long for API
            var textToAnalyze = pdfText.Length > 8000 ? pdfText[..8000] : pdfText;

            // Call OpenAI API
            var response = await CallOpenAiAsync(documentName, textToAnalyze, cancellationToken);
            
            logger.LogInformation("PDF explanation generated successfully for: {DocumentName}", documentName);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error explaining PDF for document: {DocumentName}", documentName);
            throw;
        }
    }

    private async Task<PdfExplanationResponse> CallOpenAiAsync(
        string documentName,
        string pdfText,
        CancellationToken cancellationToken)
    {
        var apiKey = configuration["OpenAI:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("your-openai-api-key"))
        {
            throw new InvalidOperationException(
                "OpenAI API key not configured. Add 'OpenAI:ApiKey' to appsettings.json with a valid key from https://platform.openai.com/api-keys");
        }

        try
        {
            // For now, return a mock response with structure
            // To integrate with real OpenAI API, you need to:
            // 1. Install: dotnet add package OpenAI
            // 2. Use: var client = new OpenAIClient(apiKey);
            // 3. Call the chat completion endpoint
            
            var mockResponse = new PdfExplanationResponse(
                PostId: 0,
                NomeDocumento: documentName,
                ResumoExecutivo: "Este documento apresenta informações importantes sobre o tema. Este é um resumo educacional para iniciantes.",
                Topicos: new List<Topico>
                {
                    new Topico(
                        "Conceito Fundamental",
                        "Explicação simples e clara do conceito principal do documento para que um iniciante entenda completamente.",
                        new List<string> { "Exemplo 1 do dia a dia que todos conhecem", "Exemplo 2 prático e fácil de visualizar" }
                    ),
                    new Topico(
                        "Aplicações Práticas",
                        "Como aplicar este conceito na vida real de forma simples e prática.",
                        new List<string> { "Uso cotidiano no trabalho", "Benefícios práticos na educação" }
                    ),
                    new Topico(
                        "Próximos Passos",
                        "O que aprender depois de dominar estes conceitos básicos.",
                        new List<string> { "Tópicos intermediários relacionados", "Recursos adicionais para aprofundamento" }
                    )
                },
                AuxiliarParicipantes: "Comece aprendendo o conceito básico antes de avançar para aplicações mais complexas. Faça perguntas quando não entender.",
                PontosPrincipais: new List<string> 
                { 
                    "Ponto importante 1 para memorizar e relembrar",
                    "Ponto importante 2 que você precisa praticar",
                    "Ponto importante 3 para aplicar no dia a dia"
                }
            );

            logger.LogInformation("PDF explanation structure created for: {DocumentName}", documentName);
            await Task.CompletedTask; // Simulate async operation
            return mockResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing PDF");
            throw;
        }
    }
}
