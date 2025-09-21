using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinaria.Domain.Entities.Vets
{
    public static class VetsError
    {
        public static Error VetNotFoud => new Error("VetNotFound", "The veterinarian was not found");
    }
}
