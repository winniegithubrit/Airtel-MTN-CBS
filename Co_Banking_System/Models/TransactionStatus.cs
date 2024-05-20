using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Co_Banking_System.Models
{
  public class TransactionStatus
  {
    public int StatusId { get; set; }

    [Required]
    public string StatusCode { get; set; }

    public string Description { get; set; }

    // Navigation property
    public ICollection<Transaction> Transactions { get; set; }
  }
}
