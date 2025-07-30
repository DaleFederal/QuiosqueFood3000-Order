using System.Net.Http;
using System.Text;
using System.Text.Json;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Api.Services;

public class PaymentService(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory) : IPaymentService
{
    public async Task ProcessPayment(PaymentDto paymentDto)
    {
        if (paymentDto.PaymentId == Guid.Empty)
            throw new ApplicationException("Identificador de Pagamento invalido");

        if (paymentDto.PaymentStatus == PaymentStatus.PendingPayment)
            return;

        var order = await orderRepository.GetOrderbyId(paymentDto.OrderId);

        if (order == null)
        {
            throw new InvalidOperationException("Pedido não encontrado");
        }
        order.PaymentStatus = paymentDto.PaymentStatus;

        orderRepository.UpdateOrder(order);
    }

    public async Task RequestPayment(OrderDto order)
    {
        var httpClient = httpClientFactory.CreateClient();
        var paymentUrl = "http://qp-prod-alb-2011577136.us-east-1.elb.amazonaws.com/api/Remittances/generate";

        var paymentRequest = new
        {
            OrderId = order.Id,
            Amount = order.TotalValue,
            WebhookUrl = "http://vd0s-prod-alb-968033309.us-east-1.elb.amazonaws.com/api/Webhook/payment-status"
        };

        var json = JsonSerializer.Serialize(paymentRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(paymentUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException("Erro ao solicitar pagamento: " + response.ReasonPhrase);
        }
    }
}