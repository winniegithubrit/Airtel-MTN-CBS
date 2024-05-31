namespace Co_Banking_System.Models
{
  public class CreateInvoiceModel
  {
    public string? ExternalId { get; set; }
    public string? Amount { get; set; }
    public string? Currency { get; set; }
    public string? ValidityDuration { get; set; }
    public Party? IntendedPayer { get; set; }
    public Party? Payee { get; set; }
    public string? Description { get; set; }
  }
}
