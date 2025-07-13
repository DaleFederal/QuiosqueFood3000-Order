using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

public interface IOrderSolicitationRepository
{
    Task<OrderSolicitation?> GetOrderSolicitationbyId(int id);
    OrderSolicitation RegisterOrderSolicitation(OrderSolicitation orderSolicitation);
    OrderSolicitation UpdateOrderSolicitation(OrderSolicitation orderSolicitation);
}