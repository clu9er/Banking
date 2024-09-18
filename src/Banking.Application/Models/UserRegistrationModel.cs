namespace Banking.Application.Models;

public record UserRegistrationModel
{
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
