using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.MedicalConsultations;

namespace Veterinaria.Domain.Entities.Invoices
{
    public class Invoice : Entity
    {
        public string UserId { get; set; } = default!;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public bool Paid { get; set; }
        public Guid  MedicalConsultationId { get; set; }
        public MedicalConsultation MedicalConsultation { get; set; } = default!;

        public Invoice()
        {
            
        }
        public Invoice(Guid Id,decimal amount, string paymentMethod, Guid medicalConsultationId)
        :base(Id)
        {
            Amount = amount;
            PaymentMethod = paymentMethod;
            MedicalConsultationId = medicalConsultationId;
        }

        public Invoice Create(Guid Id,decimal amount , string paymentMethod, Guid MedicalConsultationId)
        {
            var invocie = new Invoice()
            {
                Amount = amount,
                PaymentMethod = paymentMethod,

            };

            return invocie;


        }
    }
}
