using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinaria.Domain.Entities.Pets
{
    public static class PetError
    {
        public static Error PetNotFound => new Error("Pet.NotFound", "The pet was not found");
    }
}
