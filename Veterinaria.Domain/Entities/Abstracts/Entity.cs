using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Abstracts
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        protected Entity(Guid id)
        {
           Id = id; 
        }
        protected Entity()
        {
            
        }
    }
}
