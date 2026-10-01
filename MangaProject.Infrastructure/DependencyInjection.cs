using MangaProject.Domain.Interfaces;
using MangaProject.Infrastructure.Data;
using MangaProject.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace MangaProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MangaConnection")
            ?? throw new InvalidOperationException("A conexão PostgreSQL 'ConnectionStrings:MangaConnection' não foi configurada.");

        services.AddDbContext<MangaBRContext>(options => options.UseNpgsql(connectionString));

        services.AddSingleton<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));

        services.AddScoped<IMangaRepository, MangaRepository>();

        return services;
    }
}
