namespace Co_Banking_System.Models
{
  public class AdditionalInfo
  {
    public int AdditionalInfoId { get; set; }
    public int TransactionId { get; set; }
    public string InfoKey { get; set; }
    public string InfoValue { get; set; }

    // Navigation property
    public Transaction Transaction { get; set; }
  }
}
