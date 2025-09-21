using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Users;

namespace Veterinarian.Application.Users
{
    public sealed class RegisterUserDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public static class UserMappings
    {
        public static User ToEntity(this RegisterUserDto dto)
        {
            return new User
            {
                Id = Guid.CreateVersion7().ToString(),
                Name = dto.Name,
                Email = dto.Email,
                CreateAtUtc = DateTime.UtcNow,
            };
        }
    }

}
