using MangaProject.Domain.Entities;

namespace MangaProject.Domain.Interfaces;

public interface IMangaRepository
{
    Task AddAsync(Manga manga, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Manga>> GetAllAsync(string userId, CancellationToken cancellationToken);
    Task<Manga> GetByGuid(Guid guid, string userId, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Manga manga, string userId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid guid, string userId, CancellationToken cancellationToken);
}
