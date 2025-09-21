using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinarian.Application.Common;
using Veterinarian.Application.MedicalConsultations;

namespace Veterinarian.Application.Invoices
{
    public interface IInvoicesServices
    {
        Task<Result> CreateAsync(InvoicesRequest request);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> UpdateAsync(Guid id, InvoicesRequest resources);
        Task<Result<List<InvoicesResponse>>> GetAllAsync();
        Task<Result<PaginationResultDto<InvoicesResponse>>> SearchAsync(string? search, int page = 1, int pageSize = 5);
        Task<Result<InvoicesResponse>> GetByIdAsync(Guid id);
    }
}
