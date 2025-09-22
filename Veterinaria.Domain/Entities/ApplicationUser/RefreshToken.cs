using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Users;

namespace Veterinaria.Domain.Entities.ApplicationUser
{
    public sealed class RefreshToken
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiresAtUtc { get; set; }

        public IdentityUser? User { get; set; } = default!;
    }
}
