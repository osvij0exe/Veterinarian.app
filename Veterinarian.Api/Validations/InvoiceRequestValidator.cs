using FluentValidation;
using Veterinarian.Application.Invoices;

namespace Veterinarian.Api.Validations
{
    public class InvoiceRequestValidator : AbstractValidator<InvoicesRequest>
    {
        public InvoiceRequestValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .NotEmpty()
                .WithMessage("Amount is required");
            RuleFor(x => x.PaymentMethod)
                .Null()
                .NotEmpty()
                .WithMessage("Payment method is required");
            RuleFor(x => x.Paid)
                .Null()
                .NotEmpty()
                .WithMessage("Paid is required");
            RuleFor(x => x.MedicalConsultationId)
                .NotNull()
                .WithMessage("Medical consultation is required");
        }
    }
}
