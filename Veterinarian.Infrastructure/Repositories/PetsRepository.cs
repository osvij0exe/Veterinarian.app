using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities.Common;
using Veterinaria.Domain.Entities.Pets;
using Veterinarian.Infrastructure.Common;

namespace Veterinarian.Application.Repositories
{
    public class PetsRepository : RepositoryBase<Pet>, IPetsRepository
    {
        public readonly ApplicationDbContext _dbContext; 
        public PetsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;


        }

        public async Task<List<Pet>> SearchPetAsync(string? searchByPet)
        {
            var query = _dbContext.Set<Pet>().AsQueryable();


            return await query
                .Where(q => q.Name.Contains(searchByPet ?? string.Empty)
                    || q.Specie.Contains(searchByPet ?? string.Empty)
                    || q.Breed.Contains(searchByPet ?? string.Empty))                 
                .Include(ow => ow.Owners)
                .AsNoTracking()
                .ToListAsync();


        }

        public async Task<PaginationResult<Pet>> SearchPetAsync(string? search, int page = 1, int pageSize = 5)
        {
            IQueryable<Pet>? query = _dbContext.Set<Pet>().Where(q => q.Name.Contains(search ?? string.Empty)
                    || q.Specie.Contains(search ?? string.Empty)
                    || q.Breed.Contains(search ?? string.Empty))
                .Include(ow => ow.Owners)
                .AsNoTracking();

            var response = await PaginationProvider<Pet>.CreateAsync(query, page,pageSize);
            return response;

        }
    }
}
