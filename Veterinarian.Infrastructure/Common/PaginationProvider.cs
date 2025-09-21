using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Common;

namespace Veterinarian.Infrastructure.Common
{
    public sealed record PaginationProvider<T> 
    {
        public static async Task<PaginationResult<T>> CreateAsync(
            IQueryable<T> query,
            int page,
            int pageSize)
        {
            int totalCount = await query.CountAsync();
            List<T> items = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return new PaginationResult<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
