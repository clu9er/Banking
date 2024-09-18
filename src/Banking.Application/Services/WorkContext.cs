using Banking.Application.Interfaces;
using Banking.Domain.Enteties;
using Banking.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Banking.Application.Services;

public class WorkContext : IWorkContext
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public WorkContext(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IQueryable<User> CurrentUser
    {
        get
        {
            var claims = _signInManager.Context.User;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = _userManager.Users.Where(x => x.Id == userId);
            return query;
        }
    }

    public async Task<User> GetCurrentUser()
    {
        var claims = _signInManager.Context.User;
        var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var query = _userManager.Users.Where(x => x.Id == userId);

        var user = await query.FirstOrDefaultAsync() ?? throw new CurrentUserCannotBeNullException();

        return user;
    }
}
