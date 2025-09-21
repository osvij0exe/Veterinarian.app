using FluentValidation;
using Veterinarian.Application.Specialities;

namespace Veterinarian.Api.Validations
{
    public class specialitiesRequestValidator : AbstractValidator<SpecialitiesRequest>
    {
        public specialitiesRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(250)
                .NotNull()
                .NotEmpty()
                .WithMessage("Speciality name must be not empty");
        }

    }
}
