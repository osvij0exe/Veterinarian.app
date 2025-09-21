using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.MedicalConsultationUsers;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Users;
using Veterinaria.Domain.Entities.Vets;

namespace Veterinaria.Domain.Entities.MedicalConsultations
{
    public class MedicalConsultation : Entity
    {
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentEnd { get; set; }
        public string MedicalTreatMent { get; set; } = default!;
        public decimal  Price { get; set; }
        public Guid PetId { get; set; }
        public Pet? Pet { get; set; } = default!;
        public Guid VetId { get; set; }
        public Vet? Vet { get; set; } = default!;


        public List<MedicalConsultationUser> MedicalConsultationUsers { get; set; } = default!;
        public List<User> Users { get; set; } = default!;
    }
}
