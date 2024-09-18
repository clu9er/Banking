using Microsoft.AspNetCore.Identity;

namespace Banking.Domain.Enteties;

public class User : IdentityUser
{
    public decimal TotalBalance { get; set; } = decimal.Zero;
    public DateTime? RefreshTokenExpirationDate { get; set; }
}
