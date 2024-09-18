using Banking.Application.Interfaces;
using Banking.Application.Models.Requests;
using Banking.Application.Models.Results;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

/// <summary>
/// Manages transaction-related operations, including withdrawals, transfers, and deposits.
/// Authorization is required for all endpoints.
/// </summary>
[Authorize]
[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Withdraws a specified amount from the authenticated user's account.
    /// </summary>
    /// <param name="request">The withdrawal request containing the amount to be withdrawn.</param>
    /// <returns>A success response if the withdrawal is processed successfully.</returns>
    [HttpPost("withdraw")]
    public async Task<IActionResult> WithDraw([FromBody] TransactionRequest request)
    {
        await _transactionService.Withdraw(request.Amount);
        return Ok(Result.Success());
    }

    /// <summary>
    /// Transfers a specified amount from the authenticated user's account to another user's account.
    /// </summary>
    /// <param name="request">The transfer request containing the amount and recipient ID.</param>
    /// <returns>A success response if the transfer is processed successfully.</returns>
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        await _transactionService.TransferFunds(request.Amount, request.RecieverId);
        return Ok(Result.Success());
    }

    /// <summary>
    /// Deposits a specified amount into the authenticated user's account.
    /// </summary>
    /// <param name="request">The deposit request containing the amount to be deposited.</param>
    /// <returns>A success response if the deposit is processed successfully.</returns>
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequest request)
    {
        await _transactionService.Deposit(request.Amount);
        return Ok(Result.Success());
    }
}