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
  }
}
