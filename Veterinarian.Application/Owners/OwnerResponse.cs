using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Pets;
using Veterinarian.Application.Pets;

namespace Veterinarian.Application.Owners
{
    public class OwnerResponse
    {
        public Guid OwnerId { get; set; }
        public string GivenName { get; set; } = default!;
        public string FamilyName { get; set; } = default!;
        public string Contact { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<PetResponse>? Pets { get; set; } = default!;
    }
}
