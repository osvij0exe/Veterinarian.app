using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.MedicalConsultationUsers;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Users;

namespace Veterinaria.Domain.Entities.Owners
{
    public class Owner : Entity
    {
        public string UserId { get; set; } = default!;
        public string GivenName { get; set; } = default!;
        public string FamilyName { get; set; } = default!;
        public string Contact { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<PetOwner> PetsOwnner { get; set; } = default!;
        public List<Pet> Pets { get; set; } = default!;


        public Owner()
        {
            
        }
        public Owner(Guid id,string givenName, string familyName, string contact, string email)
            :base(id)
        {
            GivenName = givenName;
            FamilyName = familyName;
            Contact = contact;
            Email = email;
        }

        public Owner Create(Guid id, string givenName, string familyName, string contact, string email)
        {
            var dueño = new Owner(id, givenName, familyName, contact, email)
            {
                Id = id,
                GivenName = givenName,
                FamilyName = familyName,
                Contact = contact,
                Email = email
            };

            return dueño;


        }
    }
}
