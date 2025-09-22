using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinarian.Application.Common;

namespace Veterinarian.Application.Owners
{
    public interface IOwnerServices
    {
        Task<Result> CreateAndRegisterAsync(OwnerRequest request,IdentityUser identityUser);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<OwnerResponse>>> GetAllAsync();
        Task<Result<OwnerResponse>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(Guid id, OwnerUpdateRequest resources);
        Task<Result<List<OwnerResponse>>> SearchOnwers(string? search);
        Task<Result<PaginationResultDto<OwnerResponse>>> SearchOnwers(string? search, int page = 1, int pageSize = 5);
    }
}
