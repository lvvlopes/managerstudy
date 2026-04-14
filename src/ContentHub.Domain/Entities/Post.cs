namespace ContentHub.Domain.Entities;

public sealed class Post
{
    public int Id { get; set; }
    public int FonteId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime? DataPost { get; set; }
    public DateTime DataCadastro { get; set; }
    
    // PDF Fields
    public string? NomeArquivoPdf { get; set; }
    public byte[]? ConteudoPdf { get; set; }
    public string? ExplicacaoPdfJson { get; set; }
    public DateTime? DataUploadPdf { get; set; }

    public Fonte? Fonte { get; set; }
}
