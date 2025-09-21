using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinarian.Application.Owners;

namespace Veterinarian.Application.Pets
{
    public class PetRequest
    {
        public string Name { get; set; } = default!;
        public string Specie { get; set; } = default!;
        public string Breed { get; set; } = default!;
        public string GenderStatus { get; set; } = default!;
        public DateOnly BirhtDate { get; set; }
        //public List<PetOwner> Owners { get; set; } = default!;
        public List<OwnersResources>? Owners { get; set; } = default!;
        //public Guid? OwnerId { get; set; }
    }
}
