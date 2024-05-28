namespace Co_Banking_System.Models
{
  // Model class to represent the request payload for the RequestToPay action
  public class RequestToPayModel
  {
    // Properties representing the fields in the request payload
    public string? ExternalId { get; set; }
    public string? PayerId { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? PayerMessage { get; set; }
    public string? PayeeNote { get; set; }
  }
}
