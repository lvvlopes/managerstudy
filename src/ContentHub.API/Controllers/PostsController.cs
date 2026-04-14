using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.API.Controllers;

[ApiController]
[Route("posts")]
public sealed class PostsController(
    IPostService postService, 
    IPdfExplanationService pdfExplanationService,
    IPostRepository postRepository,
    ILogger<PostsController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<PostDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<PostDto>>> Get(
        [FromQuery] string? categoria,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Listando posts. Categoria: {Categoria}, Page: {Page}, PageSize: {PageSize}",
            categoria,
            page,
            pageSize);

        var posts = await postService.GetPagedAsync(categoria, page, pageSize, cancellationToken);
        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDto>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var post = await postService.GetByIdAsync(id, cancellationToken);
            return Ok(post);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDto>> Post([FromBody] CriarPostDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var post = await postService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDto>> Put(
        int id,
        [FromBody] AtualizarPostDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var post = await postService.UpdateAsync(id, dto, cancellationToken);
            return Ok(post);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("{id:int}/pdf-explanation")]
    [ProducesResponseType(typeof(PdfExplanationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PdfExplanationResponse>> UploadPdfExplanation(
        int id,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate file
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "Nenhum arquivo foi enviado." });
            }

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "Apenas arquivos PDF são aceitos." });
            }

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return BadRequest(new { error = "Arquivo muito grande. Máximo 5MB." });
            }

            // Get post
            var post = await postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound(new { error = $"Post com ID {id} não encontrado." });
            }

            // Read file content
            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream, cancellationToken);
                fileContent = memoryStream.ToArray();
            }

            logger.LogInformation("Processing PDF for post {PostId}. File: {FileName}, Size: {FileSize}",
                id, file.FileName, file.Length);

            // Generate explanation using AI
            var explanation = await pdfExplanationService.ExplainPdfAsync(
                fileContent,
                file.FileName,
                cancellationToken);

            // Save PDF and explanation to post
            post.NomeArquivoPdf = file.FileName;
            post.ConteudoPdf = fileContent;
            post.DataUploadPdf = DateTime.UtcNow;
            post.ExplicacaoPdfJson = System.Text.Json.JsonSerializer.Serialize(explanation);

            await postRepository.UpdateAsync(post);

            logger.LogInformation("PDF explanation saved successfully for post {PostId}", id);

            return Ok(explanation with { PostId = id });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Invalid operation uploading PDF for post {PostId}", id);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading PDF for post {PostId}", id);
            return BadRequest(new { error = "Erro ao processar o PDF. Tente novamente." });
        }
    }
}
