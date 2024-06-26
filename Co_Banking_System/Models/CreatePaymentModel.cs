namespace Co_Banking_System.Models
{
  public class CreatePaymentModel
  {
    public string? ExternalTransactionId { get; set; }
    public MoneyModel? Money { get; set; }
    public string? CustomerReference { get; set; }
    public string? ServiceProviderUserName { get; set; }
    public string? ReceiverMessage { get; set; }
    public string? SenderNote { get; set; }
  }

  public class MoneyModel
  {
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
  }
}
