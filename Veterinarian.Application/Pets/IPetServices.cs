using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinarian.Application.Common;
using Veterinarian.Infrastructure.Common;

namespace Veterinarian.Application.Pets
{
    public interface IPetServices
    {
        Task<Result> CreateAsync(PetRequest request);
        Task<Result> DeleteAsync(Guid Id);
        Task<Result> UpdateAsync(Guid Id, PetUpdateRequest resources);
        Task<Result<List<PetResponse>>> GetAllAsync();
        Task<Result<PetResponse>> GetByIdAsync(Guid Id);
        Task<Result<List<PetResponse>>> SearchPetAsync(string? search);
        Task<Result<PaginationResultDto<PetResponse>>> SearchPetAsync(string? search, int page = 1, int pageSize = 5);
    }
}
