using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Infrastructure.ServicesFiles;

namespace Veterinarian.Application.UnitsOfWork
{
    public class PetsUnitOfWork : UnitOfWork
    {
        public PetsUnitOfWork(ApplicationDbContext dbContext,
            IPetsRepository petsRepository,
            IPetOwnerRepository petOwnerRepository,
            IOwnerRepositoy ownerRepositoy,
            IUserContext userContext)
            : base(dbContext)
        {
            PetsRepository = petsRepository;
            OwnerRepositoy = ownerRepositoy;
            PetOwnerRepository = petOwnerRepository;
            UserContext = userContext;
        }
        public IUserContext UserContext;
        public IPetOwnerRepository PetOwnerRepository { get;  }
        public IOwnerRepositoy OwnerRepositoy { get; }
        public IPetsRepository PetsRepository { get; }
    }
}
