namespace Co_Banking_System.Models
{
  // Model class to represent the request payload for the RequestToPay action
  public class RequestToPayModel
  {
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? ExternalId { get; set; }
    public string? Payer { get; set; }
    public string? PayerMessage { get; set; }
    public string? PayeeNote { get; set; }
  }

}
