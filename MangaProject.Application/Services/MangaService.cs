using MangaProject.Application.Dtos;
using MangaProject.Application.Interfaces;
using MangaProject.Domain.Entities;
using MangaProject.Domain.Interfaces;

namespace MangaProject.Application.Services;

public class MangaService : IMangaService
{
    private readonly IMangaRepository _mangaRepository;

    public MangaService(IMangaRepository mangaRepository)
    {
        _mangaRepository = mangaRepository;
    }

    public async Task<MangaDto> CreateAsync(string userId, MangaCreateDto request, CancellationToken cancellationToken)
    {
        ValidateUserId(userId);

        ArgumentNullException.ThrowIfNull(request);

        var manga = new Manga
        {
            Guid = Guid.NewGuid(),
            Title = request.Title,
            Volume = request.Volume,
            Author = request.Author,
            Price = request.Price,
            UserId = userId
        };

        await _mangaRepository.AddAsync(manga, cancellationToken);

        return ToDto(manga);
    }

    public async Task<IReadOnlyCollection<MangaDto>> GetAllAsync(string userId, CancellationToken cancellationToken)
    {
        ValidateUserId(userId);

        return (await _mangaRepository.GetAllAsync(userId, cancellationToken)).Select(ToDto).ToList();
    }

    private static void ValidateUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("O identificador do usuário é obrigatório.", nameof(userId));
    }

    private static MangaDto ToDto(Manga manga) => new(
        manga.Guid,
        manga.Title,
        manga.Volume,
        manga.Author,
        manga.Price);

    public async Task<MangaDto> GetByGuidAsync(Guid guid, string userId, CancellationToken cancellationToken)
    {
        ValidateUserId(userId);

        var manga = await _mangaRepository.GetByGuid(guid, userId, cancellationToken);

        if (manga is null) return null;       

        return ToDto(manga);
    }
}
