using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Common;
using Veterinaria.Domain.Entities.Vets;

namespace Veterinaria.Domain.Entities.Pets
{
    public interface IPetsRepository : IReposirotyBase<Pet>
    {
        Task<List<Pet>> SearchPetAsync(string? searchByPet);
        Task<PaginationResult<Pet>> SearchPetAsync(string? search, int page = 1, int pageSize = 5);
    }
}
