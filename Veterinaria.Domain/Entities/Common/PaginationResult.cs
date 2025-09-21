using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Common
{
    public class PaginationResult<T> : IcollectionResponse<T>
    {
        public List<T> Items { get; set; } = default!;
        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public int TotalCount { get; set; }

        public int TotalPage => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviosPage => Page > 1;
        public bool HasPreviousPage => Page < TotalPage;
    }
}
