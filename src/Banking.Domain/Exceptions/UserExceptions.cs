namespace Banking.Domain.Exceptions;

public class CurrentUserCannotBeNullException : Exception
{
    public CurrentUserCannotBeNullException()
        : base("Current user cannot be null.")
    {
    }
}

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userId)
        : base($"User with ID {userId} not found.")
    {
    }
}
