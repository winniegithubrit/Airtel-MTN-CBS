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

        [HttpPost("create-api-user")]
        public async Task<IActionResult> CreateApiUser()
        {
            try
            {
                var result = await _mtnMomoService.CreateApiUserAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the API user: {ex.Message}");
            }
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

        [HttpGet("request-to-pay-status/{requestId}")]
        public async Task<IActionResult> GetRequestToPayStatus(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                return BadRequest("Request ID is required.");
            }

            try
            {
                var result = await _mtnMomoService.GetRequestToPayStatusAsync(requestId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching payment status: {ex.Message}");
            }
        }
    }
}
