using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinarian.Application;
using Veterinarian.Application.Repositories;

namespace Veterinarian.Infrastructure.Repositories
{
    public class PetOwnerRepository : RepositoryBase<PetOwner>, IPetOwnerRepository
    {
        public PetOwnerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
