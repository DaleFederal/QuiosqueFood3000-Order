using QuiosqueFood3000.Api.DTOs;

namespace QuiosqueFood3000.Api.Services;

public interface IPaymentService
{
    Task ProcessPayment(PaymentDto paymentDto);
    Task RequestPayment(OrderDto order);
}