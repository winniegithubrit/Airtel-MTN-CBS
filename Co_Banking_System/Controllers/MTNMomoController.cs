using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Co_Banking_System.Services;
using Co_Banking_System.Models;

namespace Co_Banking_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MTNMomoController : ControllerBase
    {
        private readonly MtnMomoService _mtnMomoService;

        public MTNMomoController(MtnMomoService mtnMomoService)
        {
            _mtnMomoService = mtnMomoService ?? throw new ArgumentNullException(nameof(mtnMomoService));
        }



        [HttpPost("request-to-pay")]
        public async Task<IActionResult> RequestToPay([FromBody] RequestToPayModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.ExternalId) || string.IsNullOrEmpty(model.Payer) || string.IsNullOrEmpty(model.Currency) || string.IsNullOrEmpty(model.PayerMessage) || string.IsNullOrEmpty(model.PayeeNote))
            {
                return BadRequest("Invalid request parameters.");
            }

            // Convert nullable decimal to non-nullable decimal
            decimal amount = model.Amount ?? 0;

            try
            {
                var result = await _mtnMomoService.RequestToPayAsync(model.ExternalId, model.Payer, amount, model.Currency, model.PayerMessage, model.PayeeNote);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the payment request: {ex.Message}");
            }
        }


    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentModel model)
    {
      if (model == null || string.IsNullOrEmpty(model.Amount.ToString()) || string.IsNullOrEmpty(model.Currency) || string.IsNullOrEmpty(model.CustomerReference) || string.IsNullOrEmpty(model.ServiceProviderUserName))
      {
        return BadRequest("Invalid request parameters.");
      }

      try
      {
        var result = await _mtnMomoService.CreatePaymentAsync(model);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An error occurred while creating the payment: {ex.Message}");
      }
    }

    [HttpGet("account-balance")]
    public async Task<IActionResult> GetAccountBalance()
    {
      try
      {
        // Replace 'accessToken' and 'targetEnvironment' with actual values
        string accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSMjU2In0.eyJjbGllbnRJZCI6IjRhYmQ5Y2YzLTk4YTUtNDhmMy1iMmZhLWJkNmIzMjZjYjYzNSIsImV4cGlyZXMiOiIyMDI0LTA1LTI5VDIwOjM2OjUzLjgyNyIsInNlc3Npb25JZCI6IjcxZjcyMGE2LWQwMTEtNDM4Yi1hNTdlLTIzOTlkMzg0Mzk3MCJ9.L5HEN6aW8wY9gfd80_AVclmyate2KAaZAOah-Icchfmbsk5ii4D__JSFRHwBRvoZVTjnKXxfwnyKwyFG3vq-ShtvOyuE9qrw4W7Tzlfm3WZ-uxVHDEEu7nB0ucTljUSn_XfOem_g70ZMQWkLuyAzKSUpif209dDDSyHuEgYtZykbrmuEjA3NhqosX5obWmzdC1kz-d1dUJLkwHFSsp2neV4I-RS5hFIXMBEpaU5-ePjwE8SH7xZqdWMWHVrSQS9s1afjK-bggBhzvp_PmAOZOuQIO4pSp9nkxJUnYP04bwOuUIhdFXp9NIYtxUT1jWDxIajqe4OLmGUqMNuptVJ2-A";
        string targetEnvironment = "sandbox";

        // Call the service to get the account balance
        var result = await _mtnMomoService.GetAccountBalanceAsync(accessToken, targetEnvironment);

        // Return the account balance as JSON
        return Ok(result);
      }
      catch (Exception ex)
      {
        // Handle any errors and return an error response
        return StatusCode(500, $"An error occurred while fetching account balance: {ex.Message}");
      }
    }
    
  }
}
