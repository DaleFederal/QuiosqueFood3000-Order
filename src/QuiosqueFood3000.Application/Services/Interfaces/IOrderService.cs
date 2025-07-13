using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Api.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto?> GetOrderById(int id);
    Task<List<OrderDto>?> GetOrders();
    Task<List<OrderDto>?> GetOrdersByStatus(OrderStatus orderStatus);
    Task<OrderDto> SendOrderToKitchenQueue(int id);
    void UpdateOrder(OrderDto orderDto);
    OrderDto RegisterOrder(OrderDto orderDto);
    Task<List<OrderDto>?> GetCurrentOrders();
    Task ChangeOrderStatus(int id, OrderStatus status);
}