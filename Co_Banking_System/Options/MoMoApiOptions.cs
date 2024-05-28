namespace Co_Banking_System.Options
{
  // Class to hold configuration options for the MoMo API
  public class MoMoApiOptions
  {
    // Property to store the API user identifier, can be null
    public string? ApiUser { get; set; }
    public string? ApiKey { get; set; }
    public string? AccessToken { get; set; }
    public string? BaseUrl { get; set; }
  }
}
