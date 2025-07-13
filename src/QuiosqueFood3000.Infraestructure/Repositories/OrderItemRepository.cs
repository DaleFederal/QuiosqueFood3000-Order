using Microsoft.EntityFrameworkCore;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Infraestructure.Persistence;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Infraestructure.Repositories
{
    public class OrderItemRepository(ApplicationDbContext context) : IOrderItemRepository
    {
        public async Task<OrderItem?> GetOrderItemById(int id) => await context.OrderItem.FindAsync(id);

        public void RegisterOrderItem(OrderItem orderItem)
        {
            context.OrderItem.Add(orderItem);
            context.SaveChanges();
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            var existingOrderItem = context.Customer.Find(orderItem.Id);
            if (existingOrderItem != null)
            {
                context.Entry(existingOrderItem).State = EntityState.Detached;
            }
            context.OrderItem.Remove(orderItem);
            context.SaveChanges();
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            var existingOrderItem = context.OrderItem.Find(orderItem.Id);

            if (existingOrderItem is null)
            {
                throw new InvalidOperationException("Pedido ou cliente não encontrado.");
            }
            context.Entry(existingOrderItem).State = EntityState.Detached;
            context.OrderItem.Update(orderItem);

            context.SaveChanges();
        }
    }
}
