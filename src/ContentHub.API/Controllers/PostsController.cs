using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.API.Controllers;

[ApiController]
[Route("posts")]
public sealed class PostsController(IPostService postService, ILogger<PostsController> logger) : ControllerBase
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
}
