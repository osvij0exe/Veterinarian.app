using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Common;
using Veterinaria.Domain.Entities.Owners;
using Veterinarian.Infrastructure.Common;

namespace Veterinarian.Application.Repositories
{
    public class OwnersRepository : RepositoryBase<Owner>, IOwnerRepositoy
    {
        public readonly ApplicationDbContext _dbContext; 
        public OwnersRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Owner>> GetOwnersByIds(List<Guid> ids)
        {
            var owners = await _dbContext.Set<Owner>()
                .Where(o => ids.Contains(o.Id))
                .AsNoTracking()
                .ToListAsync();

            return owners;
        }

        public async Task<List<Owner>> SearchOwners(string? search)
        {
            var query = _dbContext.Set<Owner>().AsQueryable();

            return await query.Where(q => q.FamilyName.Contains(search ?? string.Empty)
                                  || q.GivenName.Contains(search ?? string.Empty)
                                  || q.Contact.Contains(search ?? string.Empty)
                                  || q.Email.Contains(search ?? string.Empty))
                .Include(p => p.Pets)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PaginationResult<Owner>> SearchOwners(string? search, int page = 1, int pageSize = 5)
        {
            var query = _dbContext.Set<Owner>().AsQueryable().Where(q => q.FamilyName.Contains(search ?? string.Empty)
                                  || q.GivenName.Contains(search ?? string.Empty)
                                  || q.Contact.Contains(search ?? string.Empty)
                                  || q.Email.Contains(search ?? string.Empty))
                .Include(p => p.Pets)
                .OrderBy(p => p.FamilyName)
                .AsNoTracking();


            var response = await PaginationProvider<Owner>.CreateAsync(query, page, pageSize);
            return response;

        }
    }
}
