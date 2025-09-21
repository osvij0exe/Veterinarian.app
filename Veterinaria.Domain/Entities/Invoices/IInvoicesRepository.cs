using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Common;

namespace Veterinaria.Domain.Entities.Invoices
{
    public interface IInvoicesRepository : IReposirotyBase<Invoice>
    {
        Task<PaginationResult<Invoice>> SearchInvoices(string? search, int page = 1, int pageSize = 5);

    }
}
