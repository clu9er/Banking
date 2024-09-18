using Banking.Application.Interfaces;
using Banking.Application.Models;
using Microsoft.AspNetCore.Identity;
using Banking.Domain.Enteties;
using Microsoft.EntityFrameworkCore;
using Banking.Domain.Constants;
using Banking.Application.Models.Dtos;
using Banking.Domain.Exceptions;
using Banking.Infrastructure.Persistence.Redis;

namespace Banking.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RedisCacheService _redisCacheService;

    public UserService(UserManager<User> userManager, RedisCacheService redisCacheService)
    {
        _userManager = userManager;
        _redisCacheService = redisCacheService;
    }

    public async Task CreateUser(UserRegistrationModel model)
    {
        var user = new User()
        {
            Email = model.Email,
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var error = string.Join(',', result.Errors.Select(x => x.Description).ToArray());
            throw new Exception(error);
        }
    }

    public async Task<PaginatedResult<UserDto>> GetUsersPaged(int page, int pageSize)
    {
        int totalRecords = await _userManager.Users.CountAsync();

        int totalPages = (int)Math.Ceiling((double) totalRecords / pageSize);

        var users = await _userManager.Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(UserDto.FromEntity)
                    .ToListAsync();

        var result = new PaginatedResult<UserDto>
        {
            TotalRecords = totalRecords,
            PageCount = totalPages,
            PageSize = pageSize,
            Items = users
        };

        return result;
    }

    public async Task<UserDto> GetUserDtoById(string id)
    {
        string key = string.Format(UserCacheConstants.UserCacheKey, id);

        return await _redisCacheService.GetOrCreateAsync(key, async () =>
        {
            var user = await _userManager.Users
                .Select(UserDto.FromEntity)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new UserNotFoundException(id);

            return user;

        }, TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(15));
    }
}
