using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinarian.Security.Token;

namespace Veterinarian.Application.Users
{
    public interface IIdentityUserServices
    {
        Task<Result<AccessTokenDto>> Register(RegisterUserDto userDto);
        Task<Result<AccessTokenDto>> Login(LoginUserDto loginUser);

        Task<Result<AccessTokenDto>> RefreshTokenProvider(RefreshTokenDto refreshTokenDto);
    }
}
