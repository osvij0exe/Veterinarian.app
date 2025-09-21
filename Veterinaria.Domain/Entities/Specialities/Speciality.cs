using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Vets;

namespace Veterinaria.Domain.Entities.Sécialities
{
    public class Speciality : Entity
    {
        public string Name { get; set; } = default!;
        public List<Vet> Veterinarias { get; set; } = default!;

    }
}
