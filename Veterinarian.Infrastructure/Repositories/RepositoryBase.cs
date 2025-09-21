using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinarian.Application.Repositories
{
    public class RepositoryBase<TEntity> : IReposirotyBase<TEntity>
        where TEntity : Entity
    {
        private readonly ApplicationDbContext _dbContext;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            var value = await _dbContext.Set<TEntity>().AddAsync(entity);

            return true;

        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);

        }

        public async Task<bool> ExistsAsync(Guid id)
        {


            var value = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

                return true;

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(string? relationships = null)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(relationships))
            {
                foreach (var relationship in relationships.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(relationship);
                }

            }

            var value = await query.ToListAsync();

            return value;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            var value = await _dbContext.Set<TEntity>()
                .ToListAsync();
            return value;
        }
        public async Task<TEntity> GetByIdAsync(Guid id, string? relationships = null)
        {

            var query = _dbContext.Set<TEntity>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(relationships))
            {
                foreach(var relationship in relationships.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(relationship);
                }
            }

            var value = await query.FirstOrDefaultAsync(e => e.Id == id);

            //var value = await _dbContext.Set<TEntity>().FindAsync(id);
            return value!;
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {

            var value = await _dbContext.Set<TEntity>().FindAsync(id);
            return value!;
        }



        public void UpdateAsync(TEntity entity) { }
    }
}
