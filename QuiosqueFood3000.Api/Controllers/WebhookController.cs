using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services;
using QuiosqueFood3000.Api.Services.Interfaces;

namespace QuiosqueFood3000.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class WebhookController(IPaymentService paymentService) : ControllerBase
{
    
    /// <summary>
    /// Webhook para processar o pagamento
    /// </summary>
    /// <param name="paymentDto"></param>
    /// <returns></returns>
    [HttpPost("PaymentWebhook/")]
    public async Task<IActionResult> PaymentWebhook(PaymentDto paymentDto)
    {
        try
        {
            await paymentService.ProcessPayment(paymentDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
        return Ok();
    }
}