namespace Banking.Application.Models.Results;

public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    protected Result(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success(string message = "Operation was successful")
    {
        return new Result(true, message);
    }

    public static Result Failure(string message)
    {
        return new Result(false, message);
    }
}
