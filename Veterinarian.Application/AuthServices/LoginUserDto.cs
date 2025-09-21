using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinarian.Application.Users
{
    public sealed record LoginUserDto
    {
        public required string Email { get; set; } 
        public required string Password { get; set; } 
    }
}
