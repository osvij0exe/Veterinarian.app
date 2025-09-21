using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinaria.Domain.Entities.Sécialities
{
    public static class SpecialityError
    {
        public static Error SpecialityNotFound => new Error("Speciality.NotFound", "The speciality was not found");
    }
}
