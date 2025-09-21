using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Common;

namespace Veterinaria.Domain.Entities.Owners
{
    public interface IOwnerRepositoy : IReposirotyBase<Owner>
    {
        Task<List<Owner>> GetOwnersByIds(List<Guid> ids);
        Task<List<Owner>> SearchOwners(string? search);
        Task<PaginationResult<Owner>> SearchOwners(string? search, int page = 1, int pageSize = 5);
    }
}
