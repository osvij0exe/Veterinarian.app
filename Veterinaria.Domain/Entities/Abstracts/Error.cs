using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Abstracts
{
    public record Error(string Code,string NameError)
    {
        public static Error None = new(string.Empty, string.Empty);

        public static Error NullValue = new("Error.NullValue", "Null value was provided");
    }
}
