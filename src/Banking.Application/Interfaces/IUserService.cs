using Banking.Application.Models;
using Banking.Application.Models.Dtos;

namespace Banking.Application.Interfaces;

public interface IUserService
{
    public Task CreateUser(UserRegistrationModel model);
    public Task<PaginatedResult<UserDto>> GetUsersPaged(int page, int pageSize);
    public Task<UserDto> GetUserDtoById(string id);
}
