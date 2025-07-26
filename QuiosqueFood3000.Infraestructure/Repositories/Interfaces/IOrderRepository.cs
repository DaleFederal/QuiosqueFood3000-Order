using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetOrderbyId(int id);
    Task<List<Order>?> GetOrders();
    Task<List<Order>?> GetOrdersByStatus(OrderStatus orderStatus);
    Order UpdateOrder(Order order);
    Order RegisterOrder(Order order);
    Task<List<Order>?> GetCurrentOrders();
}