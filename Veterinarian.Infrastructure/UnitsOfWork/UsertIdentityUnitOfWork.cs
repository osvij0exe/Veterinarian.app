using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.ApplicationUser;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application;
using Veterinarian.Application.UnitsOfWork;
using Veterinarian.Security.SettingsFolder;

namespace Veterinarian.Infrastructure.UnitsOfWork
{
    public class UsertIdentityUnitOfWork : UnitOfWork
    {

        public UsertIdentityUnitOfWork(
            ApplicationDbContext dbContext, 
            ApplicationIdentityDbContext identityDbContext,
            IIdentityRepository identityRepository,
            IUserRepository userRepository,
            IOptions<JwtAuthOptions> options) 
            : base(dbContext)
        {
            IdentityDbContext = identityDbContext;
            ApplicationDbContext = dbContext;
            IdentityRepository = identityRepository;
            UserRepository = userRepository;
            _jwtAuthOptions = options;


        }

        public readonly IdentityDbContext IdentityDbContext;
        public readonly ApplicationDbContext ApplicationDbContext;
        public IOptions<JwtAuthOptions> _jwtAuthOptions { get; }
        public IUserRepository UserRepository { get; }
        public IIdentityRepository IdentityRepository { get; }
    }
}
