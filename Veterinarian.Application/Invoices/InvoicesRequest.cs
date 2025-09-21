using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinarian.Application.MedicalConsultations;

namespace Veterinarian.Application.Invoices
{
    public class InvoicesRequest
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public bool Paid { get; set; }
        public Guid MedicalConsultationId { get; set; }
        //public MedicalConsultationRequest MedicalConsultation { get; set; } = default!;

    }
}
