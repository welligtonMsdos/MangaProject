using FluentValidation;
using MangaProject.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MangaProject.Api.Middleware;

public sealed class MangaValidationMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly RequestDelegate _next;

    public MangaValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
       HttpContext context,
       IValidator<MangaCreateDto> createValidator)
    {
        if (HttpMethods.IsPost(context.Request.Method) && context.Request.Path.Equals("/api/mangas"))
        {
            var request = await DeserializeAsync<MangaCreateDto>(context);

            if (request is null || !await IsValidAsync(context, request, createValidator)) return;
        }        

        await _next(context);
    }

    private static async Task<TRequest?> DeserializeAsync<TRequest>(HttpContext context)
    {
        try
        {
            context.Request.EnableBuffering();

            var request = await JsonSerializer.DeserializeAsync<TRequest>(context.Request.Body, JsonOptions, context.RequestAborted);

            context.Request.Body.Position = 0;

            if (request is null)
                await WriteProblemAsync(context, new Dictionary<string, string[]> { ["body"] = ["O corpo da requisição é obrigatório."] });

            return request;
        }
        catch (JsonException)
        {
            context.Request.Body.Position = 0;

            await WriteProblemAsync(context, new Dictionary<string, string[]> { ["body"] = ["O corpo da requisição contém JSON inválido."] });

            return default;
        }
    }

    private static async Task<bool> IsValidAsync<TRequest>(HttpContext context, TRequest request, IValidator<TRequest> validator)
    {
        var result = await validator.ValidateAsync(request, context.RequestAborted);

        if (result.IsValid) return true;

        var errors = result.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());

        await WriteProblemAsync(context, errors);

        return false;
    }

    private static Task WriteProblemAsync(HttpContext context, IDictionary<string, string[]> errors)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return context.Response.WriteAsJsonAsync(new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "A requisição contém campos inválidos.",
            Instance = context.Request.Path
        }, context.RequestAborted);
    }
}
