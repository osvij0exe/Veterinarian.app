using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Application.Pets
{
    public class PetUpdateRequest
    {
        public string Name { get; set; } = default!;
        public string Specie { get; set; } = default!;
        public string Breed { get; set; } = default!;
        public string GenderStatus { get; set; } = default!;
        public DateOnly BirhtDate { get; set; }
    }
}
