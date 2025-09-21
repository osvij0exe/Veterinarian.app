using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Abstracts
{
    public interface IReposirotyBase<TEntity>
        where TEntity : Entity
    {
        Task<TEntity> GetByIdAsync(Guid id,string? relationships = null);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync(string? relatiohShips = null);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> ExistsAsync(Guid id);
    }
}
