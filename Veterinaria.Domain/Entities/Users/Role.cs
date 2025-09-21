using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Users
{
    public static class Role
    {
        public const string Admin = nameof(Admin);
        public const string AuxiliaryMember = nameof(AuxiliaryMember);
        public const string VetMember = nameof(VetMember);
        public const string Owner = nameof(Owner);
    }
}
