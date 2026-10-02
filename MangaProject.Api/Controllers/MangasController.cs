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
    public async Task<ActionResult<IReadOnlyCollection<MangaDto>>> GetAll(
        CancellationToken cancellationToken)
    {

        var mangas = await _mangaService.GetAllAsync(UserId, cancellationToken);

        return Ok(Result<IEnumerable<MangaDto>>.Ok(mangas));
    }      

    [HttpGet("{guid:guid}")]
    public async Task<ActionResult<MangaDto>> GetByGuid(
        Guid guid,
        CancellationToken cancellationToken)
    {
        //var manga = await _mangaService.GetByGuidAsync(guid, UserId, cancellationToken);

        //return manga is null ? NotFound() : Ok(manga);

        return null;
    }
}
