using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Security.SettingsFolder
{
    public sealed class JwtAuthOptions
    {
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string Key { get; init; } = default!;
        public int ExpirationInMinutes { get; init; }
        public int RefreshTokenExpirationDays { get; init; }
    }
}

