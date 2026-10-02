using Dapper;
using MangaProject.Domain.Entities;
using MangaProject.Domain.Interfaces;
using MangaProject.Infrastructure.Data;
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

    public async Task<IReadOnlyCollection<Manga>> GetAllAsync(string userId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT "Guid", "Title", "Volume", "Author", "Price", "UserId"
            FROM "Manga"
            WHERE "UserId" = @UserId;
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
}
