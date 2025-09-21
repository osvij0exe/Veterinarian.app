using FluentValidation;
using Veterinarian.Application.Vets;

namespace Veterinarian.Api.Validations
{
    public class VetRequestValidator : AbstractValidator<VetRequest>
    {
        public VetRequestValidator()
        {
            RuleFor(x => x.GivenName)
                .MaximumLength(250)
                .NotEmpty()
                .NotNull()
                .WithMessage("Given name must be not empty");

            RuleFor(x => x.FamilyName)
                .MaximumLength(250)
                .NotEmpty()
                .NotNull()
                .WithMessage("Family name must be not empty");

            RuleFor(x => x.ProfessionalId)
                .MaximumLength(250)
                .NotEmpty()
                .NotNull()
                .WithMessage("Professional Id must be not empty");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Contact)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.SpecialityId)
                .NotNull()
                .WithMessage("Speciality is required");
                
                
        }
    }
}
