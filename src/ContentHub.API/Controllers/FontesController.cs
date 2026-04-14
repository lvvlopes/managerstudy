using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.API.Controllers;

[ApiController]
[Route("fontes")]
public sealed class FontesController(IFonteService fonteService, ILogger<FontesController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<FonteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<FonteDto>>> Get(CancellationToken cancellationToken)
    {
        logger.LogInformation("Listando fontes.");
        var fontes = await fonteService.GetAllAsync(cancellationToken);
        return Ok(fontes);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FonteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FonteDto>> Post([FromBody] CriarFonteDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var fonte = await fonteService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = fonte.Id }, fonte);
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
