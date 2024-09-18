using Banking.Application.Configuration;
using Banking.Application.Helpers;
using Banking.Application.Interfaces;
using Banking.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Banking.Domain.Constants;
using Banking.Domain.Enteties;

namespace Banking.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;

    public AuthenticationService(IOptions<JwtSettings> jwtSettings, UserManager<User> userManager)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
    }

    public async Task<TokenModel> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Login);
        user ??= await _userManager.FindByNameAsync(model.Login);

        if (user == null) throw new Exception("User not found or password is incorrect");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (!isPasswordValid) throw new Exception("User not found or password is incorrect");

        var accessToken = JwtHelper.GenerateAccessToken(_jwtSettings, user);
        var refreshToken = JwtHelper.GenerateRefreshToken();

        await SetRefreshToken(user, refreshToken);

        var result = new TokenModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return result;
    }

    public async Task<TokenModel> RefreshToken(TokenModel model)
    {
        var principal = JwtHelper.GetPrincipalFromExpiredToken(_jwtSettings, model.AccessToken);

        var email = (principal?.FindFirst(ClaimTypes.Email)?.Value) ?? throw new Exception("Invalid token");
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("Invalid token");

        var currentToken = await _userManager.GetAuthenticationTokenAsync(user, RefreshTokenConstants.PROVIDER_NAME, RefreshTokenConstants.TOKEN_NAME);
        var isValid = currentToken == model.RefreshToken || user.RefreshTokenExpirationDate >= DateTime.UtcNow;

        if (!isValid) throw new Exception("Invalid token");

        var refreshToken = JwtHelper.GenerateRefreshToken();
        var accessToken = JwtHelper.GenerateAccessToken(_jwtSettings, user);

        await SetRefreshToken(user, refreshToken);

        var result = new TokenModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return result;
    }

    private async Task SetRefreshToken(User user, string refreshToken)
    {
        var expirationDate = DateTime.UtcNow.AddDays(_jwtSettings.ExpirationInDays);

        var token = await _userManager.GetAuthenticationTokenAsync(user, RefreshTokenConstants.PROVIDER_NAME, RefreshTokenConstants.TOKEN_NAME);
        if (token != null)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, RefreshTokenConstants.PROVIDER_NAME, RefreshTokenConstants.TOKEN_NAME);
        }

        await _userManager.SetAuthenticationTokenAsync(user, RefreshTokenConstants.PROVIDER_NAME, RefreshTokenConstants.TOKEN_NAME, refreshToken);
        user.RefreshTokenExpirationDate = expirationDate;

        await _userManager.UpdateAsync(user);
    }
}
