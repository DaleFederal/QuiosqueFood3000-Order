using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Api.Services;

public class PaymentService(IOrderService orderService, IOrderRepository orderRepository) : IPaymentService
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
        if(paymentDto.PaymentStatus == PaymentStatus.Payed) 
            await orderService.SendOrderToKitchenQueue(order.Id);
    }
}