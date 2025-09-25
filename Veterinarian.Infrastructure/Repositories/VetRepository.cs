using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Common;
using Veterinaria.Domain.Entities.Users;
using Veterinaria.Domain.Entities.Vets;

using Veterinarian.Infrastructure.Common;

namespace Veterinarian.Application.Repositories
{
    public class VetRepository : RepositoryBase<Vet>, IVetsRepository
    {
        public readonly ApplicationDbContext _dbContext;
        public VetRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Vet>> SearchVet(string? search)
        {
            var query = _dbContext.Set<Vet>().AsQueryable();

            return await query.Where(q => q.FamilyName.Contains(search ?? string.Empty)
                        || q.GivenName.Contains(search ?? string.Empty)
                        || q.Email.Contains(search ?? string.Empty)
                        || q.Speciality.Name.Contains(search ?? string.Empty))
                .Include(v => v.Speciality)
                .OrderBy(q => q.FamilyName)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<PaginationResult<Vet>> SearchVet(string? search, int page = 1, int pageSize = 5)
        {
            IQueryable<Vet>? query = _dbContext.Set<Vet>().Where(q => q.FamilyName.Contains(search ?? string.Empty)
            || q.GivenName.Contains(search ?? string.Empty)
            || q.Email.Contains(search ?? string.Empty)
            || q.Speciality.Name.Contains(search ?? string.Empty))
                .Include(v => v.Speciality) 
                .OrderBy(q => q.FamilyName)
                .AsNoTracking();

            var response = await PaginationProvider<Vet>.CreateAsync(query, page, pageSize);
            return response;
        }



    }
}
