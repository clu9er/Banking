using Banking.Domain.Enteties;

namespace Banking.Application.Interfaces;

public interface IWorkContext
{
    public IQueryable<User> CurrentUser { get; }
    public Task<User> GetCurrentUser();
}