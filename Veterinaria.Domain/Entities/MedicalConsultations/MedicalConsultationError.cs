using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinaria.Domain.Entities.MedicalConsultations
{
    public static class MedicalConsultationError
    {
        public static Error medicalConsultationNotFound => new Error("MedicalConsultation.Error", $"The medical consultation was not found");
    }
}
