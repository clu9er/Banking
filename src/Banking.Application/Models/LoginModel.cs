namespace Banking.Application.Models;

public record LoginModel
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}