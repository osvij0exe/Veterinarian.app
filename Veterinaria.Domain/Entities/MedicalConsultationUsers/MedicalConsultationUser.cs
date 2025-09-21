using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.Users;

namespace Veterinaria.Domain.Entities.MedicalConsultationUsers
{
    public class MedicalConsultationUser : Entity
    {
        public string UserId { get; set; } = default!;
        [NotMapped]
        public User Users { get; set; } = default!;
        public Guid MedcialConsultationId { get; set; }
        [NotMapped]
        public MedicalConsultation MedicalConsultations { get; set; } = default!;
    }
}
