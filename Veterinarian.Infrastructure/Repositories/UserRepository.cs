using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application;

namespace Veterinarian.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserContext _userContext;

        public UserRepository(ApplicationDbContext applicationDbContext,
            IUserContext userContext)
        {
            _applicationDbContext = applicationDbContext;
            _userContext = userContext;
        }

        public async Task<bool> AddAsync(User user)
        {
            await _applicationDbContext.Set<User>().AddAsync(user);
            return true;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {

            User? user = await _applicationDbContext.Set<User>()
                .Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    IdentityId = u.IdentityId,
                    CreateAtUtc = u.CreateAtUtc,
                    UpdateAtUtc = u.UpdateAtUtc,
                })
                .FirstOrDefaultAsync();

            return user!;
        }
    }
}
