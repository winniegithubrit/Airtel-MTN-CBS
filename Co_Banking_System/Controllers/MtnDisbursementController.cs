using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Co_Banking_System.Models;
using Co_Banking_System.Services;
using Microsoft.Extensions.Logging;

namespace Co_Banking_System.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MtnDisbursementController : ControllerBase
  {
    private readonly MtnDisbursementService _mtnDisbursementService;
    private readonly ILogger<MtnDisbursementController> _logger;

    // Constructor to initialize the MtnDisbursementService and Logger
    public MtnDisbursementController(MtnDisbursementService mtnDisbursementService, ILogger<MtnDisbursementController> logger)
    {
      _mtnDisbursementService = mtnDisbursementService;
      _logger = logger;
    }

    // HTTP POST endpoint for depositing money
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest depositRequest)
    {
      try
      {
        // authentication token
        var accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSMjU2In0.eyJjbGllbnRJZCI6IjY1NDI0ODg3LWU1MmQtNGFhYS05ODk1LTdhZWQzYTdmMTMyOSIsImV4cGlyZXMiOiIyMDI0LTA1LTMwVDEwOjM1OjMyLjg5OSIsInNlc3Npb25JZCI6Ijk2MTRiNjA3LWU2Y2UtNDI5My04ZjM3LTVhM2Y0NjQyMzYxNSJ9.NzHKX_JZaCsFDEfLMXi4B7043p3ijKPg7iMVXG_jHq1_2kcUNCnJVnYkgyGVbtm4dOxwuRq-VMQQRRdcpwoAiydr4HJNMSi868onJJweA_J63-rUTJb7d719lRRkuLn7J1urqrm_eQk28OOLblFnjjk7RDx2FvIZ_e3NSLxcja6M8a5YD_dHjyB8yldTN4DeFr-PWxWzKePASEIgYPgzsim_QKLiYmIQm6J10tXxjZbIg0gAoyokG7M2Il9O5495LFBBeW1JFxP5QcbZozUbULpoe5-nAKD7j6_lDBnnHsFNyvZ23gNVUsOLV2hOyvi2qEMvKbsvbvXN4AY-QDHa0w";

        // Target environment for the API request
        var targetEnvironment = "sandbox";

        // Calling the DepositAsync method of the MtnDisbursementService to process the deposit
        var result = await _mtnDisbursementService.DepositAsync(accessToken, targetEnvironment, depositRequest);

        // Return a successful response with the result
        return Ok(result);
      }
      catch (Exception ex)
      {
        // Log the error if the deposit fails and return a 500 Internal Server Error response
        _logger.LogError($"Error processing deposit: {ex.Message}");
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }

    [HttpGet("account/balance")]
    public async Task<IActionResult> GetAccountBalance()
    {
      try
      {
        var accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSMjU2In0.eyJjbGllbnRJZCI6IjY1NDI0ODg3LWU1MmQtNGFhYS05ODk1LTdhZWQzYTdmMTMyOSIsImV4cGlyZXMiOiIyMDI0LTA1LTMwVDEwOjM1OjMyLjg5OSIsInNlc3Npb25JZCI6Ijk2MTRiNjA3LWU2Y2UtNDI5My04ZjM3LTVhM2Y0NjQyMzYxNSJ9.NzHKX_JZaCsFDEfLMXi4B7043p3ijKPg7iMVXG_jHq1_2kcUNCnJVnYkgyGVbtm4dOxwuRq-VMQQRRdcpwoAiydr4HJNMSi868onJJweA_J63-rUTJb7d719lRRkuLn7J1urqrm_eQk28OOLblFnjjk7RDx2FvIZ_e3NSLxcja6M8a5YD_dHjyB8yldTN4DeFr-PWxWzKePASEIgYPgzsim_QKLiYmIQm6J10tXxjZbIg0gAoyokG7M2Il9O5495LFBBeW1JFxP5QcbZozUbULpoe5-nAKD7j6_lDBnnHsFNyvZ23gNVUsOLV2hOyvi2qEMvKbsvbvXN4AY-QDHa0w"; 
        var targetEnvironment = "sandbox"; 

        var balanceResponse = await _mtnDisbursementService.GetAccountBalanceAsync(accessToken, targetEnvironment);

        return Ok(balanceResponse);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving account balance: {ex.Message}");
        return StatusCode(500, "An error occurred while retrieving account balance.");
      }
    }
  }
}
