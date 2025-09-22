using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.ApplicationUser;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Infrastructure.UnitsOfWork;
using Veterinarian.Security.SettingsFolder;
using Veterinarian.Security.Token;

namespace Veterinarian.Application.Users
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly UsertIdentityUnitOfWork _identityUnitOfWork;
        private readonly TokenProvider _tokenProvider;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationUserServices(UsertIdentityUnitOfWork identityUnitOfWork,
            TokenProvider tokenProvider,
            UserManager<IdentityUser> userManager,
            IOptions<JwtAuthOptions> options)
        {
            _identityUnitOfWork = identityUnitOfWork;
            _tokenProvider = tokenProvider;
            _userManager = userManager;
        }

        public async Task<Result<AccessTokenDto>> Login(LoginUserDto loginUser)
        {
            var identityUser = await _identityUnitOfWork.IdentityRepository.Login(loginUser.Email,loginUser.Password);

            if(identityUser is null || !await _userManager.CheckPasswordAsync(identityUser,loginUser.Password))
            {
                return Result.Failure<AccessTokenDto>(new Error("Login.Error", "Unauthorized"));
            }

            IList<string> roles = await _identityUnitOfWork.IdentityRepository.UserRolesAsync(identityUser);

            var tokenRequest = new TokenRequest(identityUser.Id, identityUser.Email!,roles);
            AccessTokenDto accessTokenDto = _tokenProvider.Create(tokenRequest);

            var refreshToken = new RefreshToken
            {
                Id = Guid.CreateVersion7(),
                UserId = identityUser.Id,
                Token = accessTokenDto.RefreshToken,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(_identityUnitOfWork._jwtAuthOptions.Value.RefreshTokenExpirationDays)
            };

            await _identityUnitOfWork.IdentityRepository.AddRefreshTokenAsync(
                Id: refreshToken.Id,
                UserId: refreshToken.UserId,
                Token: refreshToken.Token,
                ExpiresAtUtc: refreshToken.ExpiresAtUtc);
            await _identityUnitOfWork.IdentityDbContext.SaveChangesAsync();


            return Result.Success(accessTokenDto);

        }

        public async Task<Result<AccessTokenDto>> RefreshTokenProvider(RefreshTokenDto refreshTokenDto)
        {
            RefreshToken? refreshToken = await _identityUnitOfWork.IdentityRepository.RefresTokenProviderAsync(refreshTokenDto.RefreshToken);

            if(refreshToken is null)
            {
                return Result.Failure<AccessTokenDto>(new Error("Unauthorazed.error", "An error ocurred, try it agian"));
            }

            if(refreshToken.ExpiresAtUtc < DateTime.UtcNow)
            {
                return Result.Failure<AccessTokenDto>(new Error("Unauthorazed.error", "An error ocurred, try it agian"));
            }


            IList<string> roles = await _identityUnitOfWork.IdentityRepository.UserRolesAsync(refreshToken.User!);

            // new token
            var tokenRequest = new TokenRequest(refreshToken.User!.Id, refreshToken.User.Email!,roles);
            var accessToken =  _tokenProvider.Create(tokenRequest);

            refreshToken.Token = accessToken.RefreshToken;
            refreshToken.ExpiresAtUtc = DateTime.UtcNow.AddDays(_identityUnitOfWork._jwtAuthOptions.Value.RefreshTokenExpirationDays);


            await _identityUnitOfWork.IdentityDbContext.SaveChangesAsync();
            return Result.Success(accessToken);

        }

        public async Task<Result<AccessTokenDto>> Register(RegisterUserDto registerUser, IdentityUser identityUser)
        {
            //transacción entre tablas dentro de la misma base de datos
            using IDbContextTransaction transaction = await _identityUnitOfWork.IdentityDbContext.Database.BeginTransactionAsync();
            _identityUnitOfWork.ApplicationDbContext.Database.SetDbConnection(_identityUnitOfWork.IdentityDbContext.Database.GetDbConnection());
            await _identityUnitOfWork.ApplicationDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());


            //Aplication User ApplicationDbContext
            User user = registerUser.ToEntity();
            user.IdentityId = identityUser.Id;

            await _identityUnitOfWork.UserRepository.AddAsync(user);
            await _identityUnitOfWork.ApplicationDbContext.SaveChangesAsync();

            var tokenRequest = new TokenRequest(identityUser.Id, identityUser.Email!,[Role.AuxiliaryMember]);
            AccessTokenDto accessToken = _tokenProvider.Create(tokenRequest);

            var refresh = new RefreshToken
            {
                Id = Guid.CreateVersion7(),
                UserId = identityUser.Id,
                Token = accessToken.RefreshToken,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(_identityUnitOfWork._jwtAuthOptions.Value.RefreshTokenExpirationDays)
            };

            await _identityUnitOfWork.IdentityRepository
                .AddRefreshTokenAsync(
                Id: refresh.Id,
                UserId: refresh.UserId,
                Token: refresh.Token,
                ExpiresAtUtc: refresh.ExpiresAtUtc);

            await _identityUnitOfWork.IdentityDbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Result.Success(accessToken);

        }
    }
}
