using System;
using System.Collections.Generic;

namespace Co_Banking_System.Models
{
  public class Transaction
  {
    public int TransactionId { get; set; }
    public int UserId { get; set; }
    public int StatusId { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public User User { get; set; }
    public TransactionStatus Status { get; set; }
    public ICollection<AdditionalInfo> AdditionalInfos { get; set; }
  }
}
