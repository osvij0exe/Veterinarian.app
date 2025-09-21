using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinarian.Application.Specialities
{
    public interface ISpecialitiesServices
    {
        Task<Result<List<SpecialitiesResponse>>> GetAllAsync();
        Task<Result<SpecialitiesResponse>> GetByIdAsync(Guid id);
        Task<Result> CreateAsync(SpecialitiesRequest request);
        Task<Result> UpdateAsync(Guid id, SpecialitiesRequest resources);
        Task<Result> DeleteAsync(Guid id);
    }
}
