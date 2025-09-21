using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.MedicalConsultationUsers;

namespace Veterinaria.Domain.Entities.Users
{
    public class User
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public DateTime CreateAtUtc { get; set; }
        public DateTime? UpdateAtUtc { get; set; }

        //Id from the identityProvider
        public string IdentityId { get; set; } = default!;




        public List<MedicalConsultationUser> MedicalConsutlationUser { get; set; } = default!;
        public List<MedicalConsultation> MedicalConsultations { get; set; } = default!;
    }
}
