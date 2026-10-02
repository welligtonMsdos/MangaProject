using MangaProject.Application.Interfaces;
using MangaProject.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MangaProject.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMangaService, MangaService>();        

        return services;
    }
}
