using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Co_Banking_System.Models
{
  public class TransactionStatus
  {
    [Key]
    public int StatusId { get; set; }

    [Required]
    public string? StatusCode { get; set; }

    public string? Description { get; set; }

    // Navigation property where Each status can be associated with multiple transactions
    public ICollection<Transaction>? Transactions { get; set; }
  }
}
