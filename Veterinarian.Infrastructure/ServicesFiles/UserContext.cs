using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Domain.Entities.Users;
using Veterinarian.Application;
using Veterinarian.Security.ExtensionsFile;

namespace Veterinarian.Infrastructure.ServicesFiles
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMemoryCache _memoryCache;

        public UserContext(IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext applicationDbContext,
            IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
            _memoryCache = memoryCache;
        }

        private const string CacheKeyPrefix = "users:id:";
        private static readonly TimeSpan CahceDuration = TimeSpan.FromMinutes(30);

        public async Task<string?> GetUserIdAsync(CancellationToken cancellationToken)
        {
            //claim extension method we create
            string? identityId = _httpContextAccessor.HttpContext?.User.GetIdentityId();
            if(identityId is null)
            {
                return null!;
            }

            string cacheKey = $"{CacheKeyPrefix}{identityId}";

            string? userId = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetSlidingExpiration(CahceDuration);

                string? userId = await _applicationDbContext.Set<User>()
                .Where(u => u.IdentityId == identityId)
                .Select(u => u.Id)
                .FirstOrDefaultAsync(cancellationToken);

                return userId;

            });

            return userId!;

        }
    }
}
