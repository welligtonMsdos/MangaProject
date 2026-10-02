using MangaProject.Api.Common;
using MangaProject.Api.Exceptions;
using MangaProject.Application.Dtos;
using MangaProject.Application.Interfaces;
using MangaProject.Domain.Entities;
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
}
