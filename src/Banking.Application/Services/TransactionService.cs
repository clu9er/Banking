using Banking.Application.Interfaces;
using Banking.Domain.Enteties;
using Banking.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Banking.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IWorkContext _workContext;
    private readonly UserManager<User> _userManager;

    public TransactionService(IWorkContext workContext,
        UserManager<User> userManager)
    {
        _workContext = workContext;
        _userManager = userManager;
    }

    public async Task Deposit(decimal amount)
    {
        var currentUser = await _workContext.GetCurrentUser();
        currentUser.TotalBalance += amount;
        await _userManager.UpdateAsync(currentUser);
    }

    public async Task TransferFunds(decimal amount, string recieverId)
    {
        var currentUser = await _workContext.GetCurrentUser();
        var reciever = await _userManager.FindByIdAsync(recieverId) ?? throw new UserNotFoundException(recieverId);

        if (currentUser.TotalBalance < amount) throw new InsufficientFundsException();
        if (currentUser.Id == reciever.Id) throw new SelfTransferException();

        currentUser.TotalBalance -= amount;
        reciever.TotalBalance += amount;

        await _userManager.UpdateAsync(currentUser);
        await _userManager.UpdateAsync(reciever);
    }

    public async Task Withdraw(decimal amount)
    {
        var currentUser = await _workContext.GetCurrentUser();

        if (amount > currentUser.TotalBalance) throw new InsufficientFundsException();

        currentUser.TotalBalance -= amount;

        await _userManager.UpdateAsync(currentUser);
    }
}
