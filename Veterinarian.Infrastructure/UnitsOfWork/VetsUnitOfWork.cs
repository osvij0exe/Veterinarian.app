using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.ApplicationUser;
using Veterinaria.Domain.Entities.Sécialities;
using Veterinaria.Domain.Entities.Users;
using Veterinaria.Domain.Entities.Vets;
using Veterinarian.Infrastructure;
using Veterinarian.Infrastructure.ServicesFiles;

namespace Veterinarian.Application.UnitsOfWork
{
    public class VetsUnitOfWork : UnitOfWork
    {
        public readonly ApplicationIdentityDbContext _applicationIdentityDbContext;
        public readonly ApplicationDbContext _applicationDbContext;

        public VetsUnitOfWork(ApplicationDbContext dbContext,
            ApplicationIdentityDbContext applicationIdentityDbContext,
            IVetsRepository vetsRepository,
            ISpecialityRepository specialityRepository,
            IUserContext userContext,
            IIdentityRepository identityRepository,
            IUserRepository userRepository) 
            : base(dbContext)
        {
            VetsRepository = vetsRepository;
            SpecialityRepository = specialityRepository;
            UserContext = userContext;
            IdentityRepository = identityRepository;
            UserRepository = userRepository;
            _applicationIdentityDbContext = applicationIdentityDbContext;
            _applicationDbContext = dbContext;
        }



        public IUserRepository UserRepository;
        public IUserContext UserContext;
        public IIdentityRepository IdentityRepository;
        public IVetsRepository VetsRepository { get; }
        public ISpecialityRepository SpecialityRepository { get; }
    }
}
