using FluentValidation;
using Veterinarian.Application.MedicalConsultations;

namespace Veterinarian.Api.Validations
{
    public class MedicalConsultationRequestValidator : AbstractValidator<MedicalConsultationRequest>
    {
        public MedicalConsultationRequestValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .NotNull()
                .WithMessage("Appointment date is required");
            RuleFor(x => x.Duration)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(15)
                .WithMessage("Duration mus be greater than 15 minutes");
            RuleFor(x => x.Price)
                .NotEmpty()
                .NotNull()
                .WithMessage("Price must be not empty");
            RuleFor(x => x.PetId)
                .Null()
                .WithMessage("Pet is required");
            RuleFor(x => x.VetId)
                .Null()
                .WithMessage("Vet is required");
        }
    }
}
