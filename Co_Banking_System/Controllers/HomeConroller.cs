using Microsoft.AspNetCore.Mvc;

namespace Co_Banking_System.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class HomeController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get()
    {
      return Ok("Welcome to the Co Banking System API");
    }
  }
}
