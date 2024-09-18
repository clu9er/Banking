namespace Banking.Domain.Exceptions;

public class SelfTransferException : Exception
{
    public SelfTransferException()
        : base("You cannot send money to yourself.")
    {
    }
}
