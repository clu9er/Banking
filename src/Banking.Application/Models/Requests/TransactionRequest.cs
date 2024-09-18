using System.ComponentModel.DataAnnotations;

namespace Banking.Application.Models.Requests;

public class TransactionRequest
{
    [Range(0.01, double.MaxValue, ErrorMessage = "The amount must be greater than 0.")]
    public decimal Amount { get; set; }
}

public class TransferRequest : TransactionRequest
{
    public required string RecieverId { get; set; }
}