using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;

namespace Veterinarian.Application.UserServices
{
    public interface IUserServices
    {
        Task<Result<UserResponse>> GetUserByIdAsync(string id,CancellationToken cancellationToken);
        Task<Result<UserResponse>> GetCurrentUser(CancellationToken cancellationToken);
    }
}
