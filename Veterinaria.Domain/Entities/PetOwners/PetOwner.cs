using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.Pets;

namespace Veterinaria.Domain.Entities.PetOwners
{
    public class PetOwner : Entity
    {
        public Guid PetId { get; set; }
        [NotMapped]
        public Pet Pet { get; set; } = default!;
        public Guid OwnerId { get; set; }
        [NotMapped]
        public Owner Owner { get; set; } = default!;
    }
}
