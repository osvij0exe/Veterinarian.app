using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Security.Token
{
    public sealed record RefreshTokenDto
    {
        public required string RefreshToken { get; init; }
    }
}
