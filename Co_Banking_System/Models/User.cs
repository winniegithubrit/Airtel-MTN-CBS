using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Co_Banking_System.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? PasswordHash { get; set; }

    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property for relationships whereby One user can have multiple transactions
    public ICollection<Transaction>? Transactions { get; set; }
  }
}
