using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinarian.Application.Common;

namespace Veterinarian.Application.Vets
{
    public interface IVetServices
    {
        Task<Result> CreateAndRegisterAsync(VetRequest request,IdentityUser identityUser);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<VetResponse>> GetByIdAsync(Guid id);
        Task<Result<List<VetResponse>>> GetAllAsync();
        Task<Result> UpdateAsync(Guid id, VetRequest resources);
        Task<Result<List<VetResponse>>> SearchVetAsync(string? search);

        Task<Result<PaginationResultDto<VetResponse>>> SearchVetAsync(string? search, int page = 1, int pageSize = 5);

    }
}
