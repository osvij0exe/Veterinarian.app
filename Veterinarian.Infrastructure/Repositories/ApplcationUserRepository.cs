using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.ApplicationUser;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Security.Token;

namespace Veterinarian.Infrastructure.Repositories
{
    public class ApplcationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationIdentityDbContext _identityContext;

        public ApplcationUserRepository(UserManager<IdentityUser> userManager,
            ApplicationIdentityDbContext identityContext)
        {
            _userManager = userManager;
            _identityContext = identityContext;
        }

        public async Task<bool> AddAsync(IdentityUser user)
        {
            await _identityContext.Set<IdentityUser>().AddAsync(user);
            return true;
        }

        public async Task<bool> AddRefreshTokenAsync(Guid Id, string UserId, string Token, DateTime ExpiresAtUtc)
        {
            var refreshToken = new RefreshToken()
            {
                Id = Id,
                UserId = UserId,
                Token = Token,
                ExpiresAtUtc = ExpiresAtUtc
            };

            await _identityContext.Set<RefreshToken>().AddAsync(refreshToken);
            return true;
        }


        public async Task<IdentityUser> Login(string email, string passowrd)
        {
            IdentityUser? identityUser = await _userManager.FindByEmailAsync(email);

            return identityUser!;
        }


        public async Task<RefreshToken> RefresTokenProviderAsync(string refreshToken)
        {
            RefreshToken? refresToken = await _identityContext.Set<RefreshToken>()
                .Include(r => r.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            return refresToken!;

        }

        public async Task<IList<string>> UserRolesAsync(IdentityUser identity)
        {
            IList<string> roles = await _userManager.GetRolesAsync(identity);

            return roles;
        }
    }
}
