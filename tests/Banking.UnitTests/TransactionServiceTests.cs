using Banking.Application.Interfaces;
using Banking.Application.Services;
using Banking.Domain.Enteties;
using Banking.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Banking.UnitTests;

public class TransactionServiceTests
{
    private readonly Mock<IWorkContext> _mockWorkContext;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _mockWorkContext = new Mock<IWorkContext>();
        _mockUserManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
        );

        _transactionService = new TransactionService(
            _mockWorkContext.Object,
            _mockUserManager.Object
        );
    }

    [Fact]
    public async Task Deposit_ShouldIncreaseBalance_WhenCurrentUserExists()
    {
        // Arrange
        var user = new User { TotalBalance = 100m };
        _mockWorkContext.Setup(wc => wc.GetCurrentUser())
            .ReturnsAsync(user);

        // Act
        await _transactionService.Deposit(50m);

        // Assert
        Assert.Equal(150m, user.TotalBalance);
        _mockUserManager.Verify(um => um.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task TransferFunds_ShouldTransferAmount_WhenValid()
    {
        // Arrange
        var sender = new User { Id = "1", TotalBalance = 100m };
        var receiver = new User { Id = "2", TotalBalance = 50m };

        _mockWorkContext.Setup(wc => wc.GetCurrentUser())
            .ReturnsAsync(sender);
        _mockUserManager.Setup(um => um.FindByIdAsync("2"))
            .ReturnsAsync(receiver);

        // Act
        await _transactionService.TransferFunds(30m, "2");

        // Assert
        Assert.Equal(70m, sender.TotalBalance);
        Assert.Equal(80m, receiver.TotalBalance);
        _mockUserManager.Verify(um => um.UpdateAsync(sender), Times.Once);
        _mockUserManager.Verify(um => um.UpdateAsync(receiver), Times.Once);
    }

    [Fact]
    public async Task TransferFunds_ShouldThrowException_WhenInsufficientFunds()
    {
        // Arrange
        var sender = new User { Id = "1", TotalBalance = 10m };
        var receiver = new User { Id = "2", TotalBalance = 50m };

        _mockWorkContext.Setup(wc => wc.GetCurrentUser())
            .ReturnsAsync(sender);
        _mockUserManager.Setup(um => um.FindByIdAsync("2"))
            .ReturnsAsync(receiver);

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientFundsException>(() => _transactionService.TransferFunds(30m, "2"));
    }

    [Fact]
    public async Task TransferFunds_ShouldThrowException_WhenTransferToSelf()
    {
        // Arrange
        var user = new User { Id = "1", TotalBalance = 100m };

        _mockWorkContext.Setup(wc => wc.GetCurrentUser())
            .ReturnsAsync(user);
        _mockUserManager.Setup(um => um.FindByIdAsync("1"))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<SelfTransferException>(() => _transactionService.TransferFunds(30m, "1"));
    }

    [Fact]
    public async Task Withdraw_ShouldDecreaseBalance_WhenSufficientFunds()
    {
        // Arrange
        var user = new User { TotalBalance = 100m };
        _mockWorkContext.Setup(wc => wc.GetCurrentUser())
            .ReturnsAsync(user);

        // Act
        await _transactionService.Withdraw(50m);

        // Assert
        Assert.Equal(50m, user.TotalBalance);
        _mockUserManager.Verify(um => um.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Withdraw_ShouldThrowException_WhenInsufficientFunds()
    {
        // Arrange
        var user = new User { TotalBalance = 30m };
        _mockWorkContext.Setup(wc => wc.GetCurrentUser())
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientFundsException>(() => _transactionService.Withdraw(50m));
    }
}
