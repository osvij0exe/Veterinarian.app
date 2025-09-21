using System;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.Sécialities;

namespace Veterinaria.Domain.Entities.Vets
{
    public class Vet : Entity
    {
        public string GivenName { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string FamilyName { get; set; } = default!;
        public string ProfessionalId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Contact { get; set; } = default!;
        public Guid SpecialityId { get; set; } = default!;
        public Speciality Speciality { get; set; } = default!;
        public List<MedicalConsultation> MedicalConsultations { get; set; } = default!;



    }
}
