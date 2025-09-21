using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.Pets;
using Veterinarian.Application.Pets;

namespace Veterinarian.Application.MedicalConsultations
{
    public class MedicalConsultationRequest
    {
        public DateTime AppointmentDate { get; set; }
        public int Duration { get; set; }
        public string MedicalTreatMent { get; set; } = default!;
        public decimal Price { get; set; }
        public Guid PetId { get; set; }
        public Guid VetId { get; set; }
        //public PetRequest Pet { get; set; } = default!;
    }
}
