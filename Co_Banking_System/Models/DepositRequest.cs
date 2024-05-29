namespace Co_Banking_System.Models
{
  public class DepositRequest
  {
    public string? Amount { get; set; }
    public string? Currency { get; set; }
    public string? ExternalId { get; set; }
    public Party? Payee { get; set; }
    public string? PayerMessage { get; set; }
    public string? PayeeNote { get; set; }
  }

  public class Party
  {
    public string? PartyIdType { get; set; }
    public string? PartyId { get; set; }
  }

  public class DepositResponse
  {
    public string? ReferenceId { get; set; }
  }
}
