using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Common;

namespace Veterinaria.Domain.Entities.Vets
{
    public interface IVetsRepository : IReposirotyBase<Vet>
    {
        Task<List<Vet>> SearchVet(string? search);

        Task<PaginationResult<Vet>> SearchVet(string? search, int page = 1, int pageSize = 5); 


    }
    

}
