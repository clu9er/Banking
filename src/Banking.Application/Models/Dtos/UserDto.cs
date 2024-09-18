using Banking.Domain.Enteties;
using System.Linq.Expressions;

namespace Banking.Application.Models.Dtos;
public class UserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }

    public static Expression<Func<User, UserDto>> FromEntity = user => new UserDto
    {
        Id = user.Id,
        Email = user.Email!
    };
}

public class UserProfileDto : UserDto
{
    public decimal TotalBalance { get; set; }

    public static new Expression<Func<User, UserProfileDto>> FromEntity = user => new UserProfileDto
    {
        Id = user.Id,
        Email = user.Email!,
        TotalBalance = Math.Round(user.TotalBalance, 2)
    };
}
