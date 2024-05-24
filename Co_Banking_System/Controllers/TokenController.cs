using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Co_Banking_System.Services;

namespace Co_Banking_System.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class TokenController : ControllerBase
  {
    private readonly AirtelApiClient _airtelApiClient;

    public TokenController(AirtelApiClient airtelApiClient)
    {
      _airtelApiClient = airtelApiClient;
    }

    [HttpGet("access-token")]
    public async Task<IActionResult> GetAccessToken()
    {
      try
      {
        var accessToken = await _airtelApiClient.GetAccessToken();
        return Ok(new { accessToken });
      }
      catch (HttpRequestException ex)
      {
        return StatusCode(500, new { message = ex.Message });
      }
    }
  }
}
