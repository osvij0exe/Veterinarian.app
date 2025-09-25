using Microsoft.AspNetCore.Http;
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
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public UserServices(IUserContext userContext,
            IUserRepository userRepository)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }
        public async Task<Result<UserResponse>> GetUserByIdAsync(string userId,CancellationToken cancellationToken)
        {
            string? CurrentUserId = await _userContext.GetUserIdAsync(cancellationToken);
            
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return Result.Failure<UserResponse>(new Error(StatusCodes.Status401Unauthorized.ToString(), "Unauthorized"));
            }

            if(userId != CurrentUserId)
            {
                return Result.Failure<UserResponse>(new Error(StatusCodes.Status403Forbidden.ToString(),"Access denied"));
            }

            var user = await _userRepository.GetUserByIdAsync(userId);

            if(user is  null)
            {
                return Result.Failure<UserResponse>(new Error(StatusCodes.Status404NotFound.ToString(), "the user was not found"));
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
            var userId = await _userContext.GetUserIdAsync(cancellationToken);
            
            if(string.IsNullOrWhiteSpace(userId))
            {
                return Result.Failure<UserResponse>(new Error(StatusCodes.Status401Unauthorized.ToString(), "Unauthorized"));
            }

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null)
            {
                return Result.Failure<UserResponse>(new Error(StatusCodes.Status404NotFound.ToString(), "the user was not found"));
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
