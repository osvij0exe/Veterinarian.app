using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Domain.Entities.Users
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
        Task<bool> AddAsync(User user);

    }
}
