using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinaria.Domain.Entities.Owners
{
    public static class OwnerError
    {
        public static Error OwnerNotFound => new Error("Owner.NotFound", "The owner was not found");
        public static Error OwnersNotFound => new Error("Owners.NotFound", "One or more owners does not exist");
    }
}
