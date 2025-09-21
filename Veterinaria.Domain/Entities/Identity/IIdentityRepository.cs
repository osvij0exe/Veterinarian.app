using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Users;


namespace Veterinaria.Domain.Entities.ApplicationUser
{
    public interface IIdentityRepository
    {
        Task<IdentityResult> Register(IdentityUser user,string password);
        Task<IdentityUser> Login(string email, string passowrd);
        Task<bool> AddAsync(IdentityUser user);
        Task<bool> AddRefreshTokenAsync(Guid Id, string UserId, string Token, DateTime ExpiresAtUtc);
        Task<RefreshToken> RefresTokenProviderAsync(string refreshToken);
        Task<IdentityResult> AddToRoleAsync(IdentityUser identityUser, string role);
        Task<IList<string>> UserRolesAsync(IdentityUser identity);
    }
}
