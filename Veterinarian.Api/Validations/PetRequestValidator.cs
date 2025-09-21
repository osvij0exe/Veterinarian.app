using FluentValidation;
using Veterinarian.Application.Pets;

namespace Veterinarian.Api.Validations
{
    public class PetRequestValidator : AbstractValidator<PetRequest>
    {
        public PetRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .WithMessage("Pet's name must be not empty");
            RuleFor(x => x.Specie)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .WithMessage("Pet's specie must be not empty");
            RuleFor(x => x.Breed)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .WithMessage("Pet's breed must be not empty");
            RuleFor(x => x.GenderStatus)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(1)
                .Must(x => x.Contains("M") || x.Contains("F"))
                .WithMessage("Gender status must be F - Femininum or M - Masculinum");
            RuleFor(x =>x.BirhtDate)
                .NotNull()
                .NotEmpty()
                .WithMessage("Pet's birth date is required");
            RuleForEach(c => c.Owners)
                .SetValidator(new OwnersResourcesValidator());
        }
    }
}
