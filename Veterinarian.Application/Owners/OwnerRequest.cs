using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Pets;
using Veterinarian.Application.Pets;

namespace Veterinarian.Application.Owners
{
    public class OwnerRequest
    {
        public string GivenName { get; set; } = default!;
        public string FamilyName { get; set; } = default!;
        public string Contact { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<PetResources>? Pets { get; set; } = default!;


        //identity
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
