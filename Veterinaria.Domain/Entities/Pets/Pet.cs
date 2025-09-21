using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.PetOwners;

namespace Veterinaria.Domain.Entities.Pets
{
    public class Pet : Entity
    {
        public string UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Specie { get; set; } = default!;
        public string Breed { get; set; } = default!;
        public string GenderStatus { get; set; } = default!;
        public DateOnly BirhtDate { get; set; }
        public List<PetOwner> PetsOwner { get; set; } = default!;
        public List<Owner> Owners { get; set; } = default!;
        public List<MedicalConsultation> Consults { get; set; } = default!;
    }
}
