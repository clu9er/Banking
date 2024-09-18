namespace Banking.Domain.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException()
        : base("You don't have enough money.")
    {
    }
}
