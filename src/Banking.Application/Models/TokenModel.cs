namespace Banking.Application.Models;

public record TokenModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
