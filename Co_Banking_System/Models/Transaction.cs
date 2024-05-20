using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Co_Banking_System.Models
{
  public class Transaction
  {
    [Key]
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }

    [Required]
    public string? Reference { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User? User { get; set; }

    public TransactionStatus? Status { get; set; }

    public ICollection<AdditionalInfo>? AdditionalInfos { get; set; }
  }
}
