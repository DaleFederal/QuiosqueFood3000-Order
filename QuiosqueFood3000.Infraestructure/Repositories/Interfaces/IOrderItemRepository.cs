using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Infraestructure.Repositories.Interfaces
{
    public  interface IOrderItemRepository
    {
        Task<OrderItem?> GetOrderItemById(int id);
        void RegisterOrderItem(OrderItem orderItem);
        void RemoveOrderItem(OrderItem orderItem);
        void UpdateOrderItem(OrderItem orderItem);
    }
}
