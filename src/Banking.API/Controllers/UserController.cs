using Microsoft.AspNetCore.Mvc;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Banking.Application.Models.Dtos;

namespace Banking.API.Controllers;

/// <summary>
/// Manages user-related operations such as retrieving user information or paginated user lists.
/// </summary>
[Authorize]
[ApiController]
[Route("/api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IWorkContext _workContext;

    public UserController(IUserService userService, IWorkContext workContext)
    {
        _userService = userService;
        _workContext = workContext;
    }

    /// <summary>
    /// Retrieves a paginated list of users.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of users per page.</param>
    /// <returns>A paginated list of users.</returns>
    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedUsers(int pageNumber, int pageSize)
    {
        var data = await _userService.GetUsersPaged(pageNumber, pageSize);
        return Ok(data);
    }

    /// <summary>
    /// Retrieves a specific user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID, or a not found result if the user does not exist.</returns>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(string userId)
    {
        var user = await _userService.GetUserDtoById(userId);
        return Ok(user);
    }

    /// <summary>
    /// Retrieves the current authenticated user.
    /// </summary>
    /// <returns>The profile of the currently authenticated user.</returns>
    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _workContext.CurrentUser
            .Select(UserProfileDto.FromEntity)
            .FirstOrDefaultAsync();

        return Ok(user);
    }
}
