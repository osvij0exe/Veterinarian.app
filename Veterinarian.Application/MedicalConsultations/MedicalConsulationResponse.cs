using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.Pets;
using Veterinarian.Application.Owners;
using Veterinarian.Application.Pets;
using Veterinarian.Application.Vets;

namespace Veterinarian.Application.MedicalConsultations
{
    public class MedicalConsulationResponse
    {
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentEnd { get; set; }
        public string MedicalTreatMent { get; set; } = default!;
        public decimal Price { get; set; }
        public Guid PetId { get; set; }
        public PetResources Pet { get; set; } = default!;
        public VetResponse Vet { get; set; } = default!;
    }
}
