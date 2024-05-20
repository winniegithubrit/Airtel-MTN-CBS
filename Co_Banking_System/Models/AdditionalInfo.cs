using System.ComponentModel.DataAnnotations;

namespace Co_Banking_System.Models
{
  public class AdditionalInfo
  {
    [Key]
    public int AdditionalInfoId { get; set; }

    public int TransactionId { get; set; }

    [Required]
    public string? InfoKey { get; set; }

    [Required]
    public string? InfoValue { get; set; }

    // Navigation property
    public Transaction? Transaction { get; set; }
  }
}
