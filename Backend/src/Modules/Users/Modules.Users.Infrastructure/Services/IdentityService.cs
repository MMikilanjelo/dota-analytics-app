using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Common.Contracts;
using Common.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Modules.Users.Application.UseCases.Contracts.Services;
using Modules.Users.Domain.Aggregates.UserAggregate;

namespace Modules.Users.Infrastructure.Services;

public class IdentityService(IConfiguration configuration, IClock clock) : IIdentityService
{
    public string GenerateAccessToken(UserEntity userEntity)
    {
        string secretKey = configuration["Jwt:Secret"]!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, userEntity.Id.ToString())
            ]),
            Expires = clock.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Base64UrlEncode(bytes);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}