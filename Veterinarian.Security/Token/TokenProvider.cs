﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Veterinarian.Security.SettingsFolder;

namespace Veterinarian.Security.Token
{

    public sealed class TokenProvider(IOptions<JwtAuthOptions> options)
    {
        private readonly JwtAuthOptions _jwtAuthOptions = options.Value;


        public AccessTokenDto Create(TokenRequest tokenRequest)
        {
            return new AccessTokenDto(GenerateAcessToken(tokenRequest), GenerateRefreshToken());
        }

        public string GenerateAcessToken(TokenRequest tokenRequest)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims =
                [
                    new(JwtRegisteredClaimNames.Sub, tokenRequest.UserId),
                    new(JwtRegisteredClaimNames.Email, tokenRequest.Email),
                    //Adding roles
                    ..tokenRequest.Roles.Select(role => new Claim(ClaimTypes.Role, role))
                ];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtAuthOptions.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtAuthOptions.Issuer,
                Audience = _jwtAuthOptions.Audience
            };

            var handler = new JsonWebTokenHandler();

            var accessToken = handler.CreateToken(tokenDescriptor);

            return accessToken;
        }
        private static string GenerateRefreshToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
