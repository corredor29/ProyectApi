// Application/UseCase/Continents/UpdateContinentValidator.cs
using FluentValidation;

namespace Application.UseCase.Continents;

public sealed class UpdateContinentValidator : AbstractValidator<UpdateContinent>
{
    public UpdateContinentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");
    }
}