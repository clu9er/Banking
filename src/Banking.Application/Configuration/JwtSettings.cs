namespace Banking.Application.Configuration;

public class JwtSettings
{
    public required string SecretKey { get; set; }
    public int ExpirationInMinutes { get; set; }
    public int ExpirationInDays { get; set; }
}
