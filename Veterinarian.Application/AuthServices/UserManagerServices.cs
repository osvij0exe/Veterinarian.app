using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Application.AuthServices
{
    public class UserManagerServices : IUserManagerServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagerServices(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> AddToRoleAsync(IdentityUser identityUser, string role)
        {
            IdentityResult roleAdded = await _userManager.AddToRoleAsync(identityUser, role);
            if (!roleAdded.Succeeded)
            {
                await _userManager.DeleteAsync(identityUser);
            }

            return roleAdded;
        }

        public async Task<IdentityResult> IdentityUserRegister(IdentityUser user, string password)
        {

            IdentityResult identityResult = await _userManager.CreateAsync(user: user, password: password);

            if (!identityResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
            }

            return identityResult;
        }

        public async void RemoveIdentityUserAsinc(IdentityUser user)
        {
            await _userManager.DeleteAsync(user);
        }
    }
}
