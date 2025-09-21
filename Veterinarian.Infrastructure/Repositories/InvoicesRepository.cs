using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Common;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.Pets;
using Veterinarian.Infrastructure.Common;

namespace Veterinarian.Application.Repositories
{
    public class InvoicesRepository : RepositoryBase<Invoice>, IInvoicesRepository
    {
        public readonly ApplicationDbContext _dbContext;
        public InvoicesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginationResult<Invoice>> SearchInvoices(string? search, int page = 1, int pageSize = 5)
        {
            var query = _dbContext.Set<Invoice>()
                .AsQueryable()
                .Where(q => q.MedicalConsultation.Pet!.Name.Contains(search ?? string.Empty)
                    || q.MedicalConsultation.Pet.Breed.Contains(search ?? string.Empty)
                    || q.MedicalConsultation.Pet.Specie.Contains(search ?? string.Empty))
                .Include(q => q.MedicalConsultation.Pet)
                .Include(m => m.MedicalConsultation.Vet)
                .OrderBy(q => q.MedicalConsultation.Pet!.Name)
                .AsNoTracking();

            var response = await PaginationProvider<Invoice>.CreateAsync(query, page, pageSize);
            return response;
                
        }
    }
}
