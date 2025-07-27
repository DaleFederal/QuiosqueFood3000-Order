using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public WebhookController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost("payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus([FromBody] PaymentDto paymentDto)
        {
            await _paymentService.ProcessPayment(paymentDto);

            if(paymentDto.PaymentStatus == PaymentStatus.Payed)
            {
                await _orderService.SendOrderToKitchenQueue(paymentDto.OrderId);
            }

            return Ok();
        }
    }
}
