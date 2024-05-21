using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Co_Banking_System.Services;
using Co_Banking_System.Options;

namespace Co_Banking_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirtelController : ControllerBase
    {
        private readonly AirtelApiClient _airtelApiClient;

        public AirtelController(IOptions<AirtelApiOptions> options)
        {
            _airtelApiClient = new AirtelApiClient(options);
        }

        [HttpPost("cashin")]
        public async Task<IActionResult> CashIn([FromBody] CashInRequest request)
        {
            try
            {
                if (request is null)
                {
                    return BadRequest("Request body cannot be null");
                }

                string endpoint = "standard/v2/cashin/";
                string response = await _airtelApiClient.MakeCashInRequest(endpoint, request);
                return Ok(response);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }

    public class CashInRequest
    {
        public Subscriber? subscriber { get; set; }
        public Transaction? transaction { get; set; }
        public AdditionalInfo[]? additional_info { get; set; }
        public string? reference { get; set; }
        public string? pin { get; set; }
    }

    public class Subscriber
    {
        public string? msisdn { get; set; }
    }

    public class Transaction
    {
        public string? amount { get; set; }
        public string? id { get; set; }
    }

    public class AdditionalInfo
    {
        public string? key { get; set; }
        public string? value { get; set; }
    }
}
