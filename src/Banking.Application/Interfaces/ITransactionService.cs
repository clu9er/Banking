namespace Banking.Application.Interfaces;

public interface ITransactionService
{
    public Task Withdraw(decimal amount);
    public Task Deposit(decimal amount);
    public Task TransferFunds(decimal amount, string toUserId);
}
