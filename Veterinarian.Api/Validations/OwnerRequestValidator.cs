using FluentValidation;
using Veterinarian.Application.Owners;

namespace Veterinarian.Api.Validations
{
    public class OwnerRequestValidator : AbstractValidator<OwnerRequest>
    {
        public OwnerRequestValidator()
        {
            RuleFor(x => x.GivenName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .WithMessage("Given Name must be not empty");
            RuleFor(x => x.FamilyName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .WithMessage("Family Name must be not empty");
            RuleFor(x => x.Contact)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .WithMessage("Contact must be not empty");
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleForEach(x => x.Pets)
                .SetValidator(new PetResourcesValidator());
        }
    }
}
