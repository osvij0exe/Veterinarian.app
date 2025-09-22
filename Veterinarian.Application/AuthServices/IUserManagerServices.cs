using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Application.AuthServices
{
    public interface IUserManagerServices
    {
        Task<IdentityResult> IdentityUserRegister(IdentityUser user, string password);
        Task<IdentityResult> AddToRoleAsync(IdentityUser identityUser, string role);
        void RemoveIdentityUserAsinc(IdentityUser user);
    }
}
