using MangaProject.Domain.Entities;

namespace MangaProject.Domain.Interfaces;

public interface IMangaRepository
{
    Task AddAsync(Manga manga, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Manga>> GetAllAsync(string userId, CancellationToken cancellationToken);
}
