using System;
using System.Collections.Generic;

namespace Co_Banking_System.Models
{
  public class User
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property for relationships
    public ICollection<Transaction> Transactions { get; set; }
  }
}
