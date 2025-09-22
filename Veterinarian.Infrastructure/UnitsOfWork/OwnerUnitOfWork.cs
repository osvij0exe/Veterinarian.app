using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.ApplicationUser;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Infrastructure;
using Veterinarian.Infrastructure.ServicesFiles;

namespace Veterinarian.Application.UnitsOfWork
{
    public class OwnerUnitOfWork : UnitOfWork
    {
        public readonly ApplicationIdentityDbContext _identityDbContext;
        public readonly ApplicationDbContext _applicationDbContext;
        public OwnerUnitOfWork(ApplicationDbContext dbContext,
            ApplicationIdentityDbContext applicationIdentityDbContext,
            IOwnerRepositoy ownerRepositoy,
            IPetsRepository petsRepository,
            IUserContext userContext,
            IUserRepository userRepository,
            IApplicationUserRepository identityRepository) 
            : base(dbContext)
        {
            _applicationDbContext = dbContext;
            _identityDbContext = applicationIdentityDbContext;
            OwnerRepositoy = ownerRepositoy;
            PetsRepository = petsRepository;
            UserContext = userContext;
            IdentityRepository = identityRepository;
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; }
        public IApplicationUserRepository IdentityRepository { get; }
        public IUserContext UserContext;
        public IOwnerRepositoy OwnerRepositoy { get; }
        public IPetsRepository PetsRepository { get; }
    }
}
