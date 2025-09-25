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
    public interface IApplicationUserRepository
    {
        Task<IdentityUser> Login(string email, string passowrd);
        Task<bool> AddAsync(IdentityUser user);
        Task<bool> AddRefreshTokenAsync(Guid Id, string UserId, string Token, DateTime ExpiresAtUtc);
        Task<RefreshToken> RefresTokenProviderAsync(string refreshToken);
        Task<IList<string>> UserRolesAsync(IdentityUser identity);
        Task<IdentityUser> GetUserById(string identityId);
        Task DelectUserAsync(IdentityUser identityUser);


    }
}
