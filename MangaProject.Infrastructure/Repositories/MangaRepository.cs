using Dapper;
using MangaProject.Domain.Entities;
using MangaProject.Domain.Interfaces;
using MangaProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MangaProject.Infrastructure.Repositories;

public class MangaRepository : IMangaRepository
{
    private readonly MangaBRContext _dbContext;
    private readonly NpgsqlDataSource _dataSource;

    public MangaRepository(MangaBRContext dbContext, NpgsqlDataSource dataSource)
    {
        _dbContext = dbContext;
        _dataSource = dataSource;
    }

    public async Task AddAsync(Manga manga, CancellationToken cancellationToken)
    {
        await _dbContext.Mangas.AddAsync(manga, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid guid, string userId, CancellationToken cancellationToken)
    {
        var manga = await _dbContext.Mangas
            .FirstOrDefaultAsync(v => v.Guid == guid && v.UserId == userId, cancellationToken);

        if (manga is null) return false;
       
        _dbContext.Mangas.Remove(manga);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IReadOnlyCollection<Manga>> GetAllAsync(string userId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT "Guid", "Title", "Volume", "Author", "Price", "UserId"
            FROM "Manga"
            WHERE "UserId" = @UserId
            Order by "Title" ASC, "Volume" ASC;
            """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        var mangas = await connection.QueryAsync<Manga>(
            new CommandDefinition(sql, new { UserId = userId }, cancellationToken: cancellationToken));

        return mangas.AsList();
    }

    public async Task<Manga> GetByGuid(Guid guid, string userId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT "Guid", "Title", "Volume", "Author", "Price", "UserId"
            FROM "Manga"
            WHERE "Guid" = @Guid AND "UserId" = @UserId;
            """;

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        var manga = await connection.QuerySingleOrDefaultAsync<Manga>(
        new CommandDefinition(
            sql,
            new { Guid = guid, UserId = userId },
                cancellationToken: cancellationToken));       

        return manga;
    }

    public async Task<bool> UpdateAsync(Manga manga, string userId, CancellationToken cancellationToken)
    {
        var existingManga = await _dbContext.Mangas
            .FirstOrDefaultAsync(v => v.Guid == manga.Guid && v.UserId == userId, cancellationToken);

        if(existingManga is null) return false;

        existingManga.Title = manga.Title;
        existingManga.Volume = manga.Volume;
        existingManga.Author = manga.Author;
        existingManga.Price = manga.Price;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
