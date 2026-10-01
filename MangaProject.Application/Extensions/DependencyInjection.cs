using FluentValidation;
using MangaProject.Application.Dtos;
using MangaProject.Application.Interfaces;
using MangaProject.Application.Services;
using MangaProject.Application.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace MangaProject.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMangaService, MangaService>();

        services.AddScoped<IValidator<MangaCreateDto>, CreateMangaDtoValidator>();

        return services;
    }
}
