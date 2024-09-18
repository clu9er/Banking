using Microsoft.AspNetCore.Mvc;
using Banking.Application.Interfaces;
using Banking.Application.Models;
using Banking.Application.Models.Results;

namespace Banking.API.Controllers;

/// <summary>
/// Handles user authentication-related operations such as registration, login, and token refresh.
/// </summary>
[ApiController]
[Route("/api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IUserService userService,
        IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="user">The registration details of the new user.</param>
    /// <returns>A success result if the user is created successfully.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistrationModel user)
    {
        await _userService.CreateUser(user);
        return Ok(Result.Success());
    }

    /// <summary>
    /// Authenticates a user and provides an access token upon successful login.
    /// </summary>
    /// <param name="model">The login credentials (username and password).</param>
    /// <returns>An access token if authentication is successful.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var token = await _authenticationService.Login(model);
        return Ok(token);
    }

    /// <summary>
    /// Refreshes the user's access token using a valid refresh token.
    /// </summary>
    /// <param name="model">The refresh token model containing the current refresh token.</param>
    /// <returns>A new access token if the refresh token is valid.</returns>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel model)
    {
        var result = await _authenticationService.RefreshToken(model);
        return Ok(result);
    }
}
