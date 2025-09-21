using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Common
{
    public interface IcollectionResponse<T>
    {
        public List<T> Items { get; set; }
    }
}
