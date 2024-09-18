using Banking.Application.Models;

namespace Banking.Application.Interfaces;

public interface IAuthenticationService
{
    public Task<TokenModel> Login(LoginModel model);
    public Task<TokenModel> RefreshToken(TokenModel model);
}
