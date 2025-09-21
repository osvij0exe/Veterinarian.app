using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinarian.Application.Specialities;

namespace Veterinarian.Application.Vets
{
    public class VetRequest
    {
        public string GivenName { get; set; } = default!;
        public string FamilyName { get; set; } = default!;
        public string ProfessionalId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Contact { get; set; } = default!;
        public Guid SpecialityId { get; set; }


        //identity
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
