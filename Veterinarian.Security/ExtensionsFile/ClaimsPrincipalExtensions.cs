using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Security.ExtensionsFile
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetIdentityId(this ClaimsPrincipal? principal)
        {
            string? identityId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            return identityId;
        }
    }
}
