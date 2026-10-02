using MangaProject.Application.Dtos;

namespace MangaProject.Application.Interfaces;

public interface IMangaService
{
    Task<MangaDto> CreateAsync(string userId, MangaCreateDto request, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<MangaDto>> GetAllAsync(string userId, CancellationToken cancellationToken);
    Task<MangaDto> GetByGuidAsync(Guid guid, string userId, CancellationToken cancellationToken);
    Task<MangaDto?> UpdateAsync(Guid guid, string userId, UpdateMangaDto request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid guid, string userId, CancellationToken cancellationToken);
}
