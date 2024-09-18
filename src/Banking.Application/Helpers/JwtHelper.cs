using Banking.Application.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Banking.Domain.Enteties;

namespace Banking.Application.Helpers;

public static class JwtHelper
{
    public static TokenValidationParameters GetTokenValidationParameters(JwtSettings settings)
    {
        var key = settings.SecretKey;
        var keyBytes = Encoding.ASCII.GetBytes(key!);
        return new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
    }

    public static string GenerateAccessToken(JwtSettings _jwtSettings, User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(user.Email))
        {
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        }

        return claims;
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static ClaimsPrincipal? GetPrincipalFromExpiredToken(JwtSettings configuration, string accessToken)
    {
        var tokenValidationParameters = GetTokenValidationParameters(configuration);
        tokenValidationParameters.ValidateLifetime = false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new Exception("Invalid token");
        }
        return principal;
    }
}
