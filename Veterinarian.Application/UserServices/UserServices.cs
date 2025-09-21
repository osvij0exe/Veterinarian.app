using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Abstracts;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Infrastructure.ServicesFiles;

namespace Veterinarian.Application.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserContext _userContext;
        private readonly IUserRepository _userRepository;

        public UserServices(ApplicationDbContext dbContext,
            UserContext userContext,
            IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userContext = userContext;
            _userRepository = userRepository;
        }
        public async Task<Result<UserResponse>> GetUserById(string id,CancellationToken cancellationToken)
        {
            var userId = await _userContext.GetUserIdAsync(cancellationToken);

            if(string.IsNullOrWhiteSpace(userId))
            {
                return Result.Failure<UserResponse>(new Error("Unauthorized.Error", "Unauthorized"));
            };

            if(id != userId)
            {
                return Result.Failure<UserResponse>(new Error("Forbiden.Error","Access denied"));
            }

            var user = await _userRepository.GetUserByIdAsync(id);

            if(user is  null)
            {
                return Result.Failure<UserResponse>(new Error("NotFound.Error", "the user was not found"));
            }

            var userMapped = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatAtUtc = user.CreateAtUtc,
                UpdateAtUtc = user.UpdateAtUtc,
            };

            return Result.Success(userMapped);

        }
        public async Task<Result<UserResponse>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var identityId = await _userContext.GetUserIdAsync(cancellationToken);
            
            if(string.IsNullOrWhiteSpace(identityId))
            {
                return Result.Failure<UserResponse>(new Error("Unauthorized.Error", "Unauthorized"));
            }

            var user = await _userRepository.GetUserByIdAsync(identityId);

            if (user is null)
            {
                return Result.Failure<UserResponse>(new Error("NotFound.Error", "the user was not found"));
            }

            var userMapped = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatAtUtc = user.CreateAtUtc,
                UpdateAtUtc = user.UpdateAtUtc,
            };
            return Result.Success(userMapped);

        }

    }
}
