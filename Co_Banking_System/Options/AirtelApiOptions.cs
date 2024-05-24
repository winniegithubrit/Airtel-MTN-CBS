namespace Co_Banking_System.Options
{
    public class AirtelApiOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string TokenUrl { get; set; } = string.Empty;

        // Parameterless constructor for configuration binding
        public AirtelApiOptions() { }

        // Constructor to ensure all properties are set
        public AirtelApiOptions(string apiKey, string clientId, string clientSecret, string baseUrl, string tokenUrl)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            TokenUrl = tokenUrl ?? throw new ArgumentNullException(nameof(tokenUrl));
        }
    }
}
