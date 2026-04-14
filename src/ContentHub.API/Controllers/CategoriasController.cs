using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.API.Controllers;

[ApiController]
[Route("categorias")]
public sealed class CategoriasController(ICategoriaService categoriaService, ILogger<CategoriasController> logger)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CategoriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CategoriaDto>>> Get(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando categorias.");
        var categorias = await categoriaService.GetAllAsync(cancellationToken);
        return Ok(categorias);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoriaDto>> Post(
        [FromBody] CriarCategoriaDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var categoria = await categoriaService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = categoria.Id }, categoria);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
