using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Application.Owners
{
    public class OwnerUpdateRequest
    {
        public string GivenName { get; set; } = default!;
        public string FamilyName { get; set; } = default!;
        public string Contact { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
