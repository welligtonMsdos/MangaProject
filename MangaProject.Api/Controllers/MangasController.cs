using MangaProject.Api.Common;
using MangaProject.Application.Dtos;
using MangaProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaProject.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class MangasController : BaseController
{
   private readonly IMangaService _mangaService;
    
    public MangasController(IMangaService mangaService)
    {
        _mangaService = mangaService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<MangaDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<MangaDto>> Create(
        [FromBody] MangaCreateDto request,
        CancellationToken cancellationToken)
    {
        var manga = await _mangaService.CreateAsync(UserId, request, cancellationToken);        

        return CreatedAtAction(nameof(GetByGuid),
                               new { guid = manga.Guid },
                               Result<MangaDto>.Ok(manga, "Manga criado com sucesso!"));
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<MangaDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<IEnumerable<MangaDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var mangas = await _mangaService.GetAllAsync(UserId, cancellationToken);       

        return Ok(Result<IEnumerable<MangaDto>>.Ok(mangas));
    }

    [HttpGet("{guid:guid}")]
    [ProducesResponseType(typeof(Result<MangaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<MangaDto>>> GetByGuid(Guid guid, CancellationToken cancellationToken)
    {
        var manga = await _mangaService.GetByGuidAsync(guid, UserId, cancellationToken);

        if (manga == null) return NotFound();

        return Ok(Result<MangaDto>.Ok(manga));
    }

    [HttpPut("{guid:guid}")]
    [ProducesResponseType(typeof(Result<MangaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<MangaDto>>> Update(
        Guid guid,
        [FromBody] UpdateMangaDto request,
        CancellationToken cancellationToken)
    {
        var manga = await _mangaService.UpdateAsync(guid, UserId, request, cancellationToken);

        if (manga == null) return NotFound();

        return Ok(Result<MangaDto>.Ok(manga));
    }

    [HttpDelete("{guid:guid}")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<string>>> Delete(Guid guid, CancellationToken cancellationToken)
    {
        var deleted = await _mangaService.DeleteAsync(guid, UserId, cancellationToken);

        if (!deleted) return NotFound();

        return Ok(Result<string>.Ok("Manga deletado com sucesso!"));
    }
}
