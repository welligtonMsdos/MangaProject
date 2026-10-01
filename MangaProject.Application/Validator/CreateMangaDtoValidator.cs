using FluentValidation;
using MangaProject.Application.Dtos;

namespace MangaProject.Application.Validator;

public sealed class CreateMangaDtoValidator: AbstractValidator<MangaCreateDto>
{
    public CreateMangaDtoValidator()
    {
        RuleFor(m => m.Title)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MinimumLength(3).WithMessage("Título deve ter pelo menos 3 caracteres.")
            .MaximumLength(50).WithMessage("Título não pode exceder 50 caracteres.");

        RuleFor(m => m.Volume)
            .NotEmpty().WithMessage("Volume é obrigatório.")
            .InclusiveBetween(1, 200).WithMessage("Volume deve estar entre 1 e 200.");

        RuleFor(m => m.Author)
            .NotEmpty().WithMessage("Autor é obrigatório.")
            .MinimumLength(3).WithMessage("Autor deve ter pelo menos 3 caracteres.")
            .MaximumLength(100).WithMessage("Autor não pode exceder 100 caracteres.");

        RuleFor(m => m.Price)
            .NotEmpty().WithMessage("Preço é obrigatório.")
            .GreaterThan(0).WithMessage("Preço deve ser positivo.");
    }
}
