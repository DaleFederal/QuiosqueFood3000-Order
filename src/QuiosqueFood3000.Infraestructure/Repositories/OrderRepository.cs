using Microsoft.EntityFrameworkCore;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Persistence;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Infraestructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetOrderbyId(int id)
    {
        return await context.Order
            .Include(o => o.OrderSolicitation)
            .Include(o => o.OrderItemsList!)
            .ThenInclude(p => p.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    public async Task<List<Order>?> GetOrders() => await context.Order
        .Include(o => o.OrderSolicitation)
        .Include(o => o.OrderItemsList!)
        .ThenInclude(p => p.Product)
        .Include(o => o.Customer).ToListAsync();
    public async Task<List<Order>?> GetOrdersByStatus(OrderStatus orderStatus) => await context.Order
            .Include(o => o.OrderSolicitation)
            .Include(o => o.OrderItemsList!)
            .ThenInclude(p => p.Product)
            .Include(o => o.Customer).Where(o => o.OrderStatus == orderStatus).ToListAsync();

    public Order RegisterOrder(Order order)
    {
        context.Order.Add(order);
        context.SaveChanges();
        return order;
    }

    public Order UpdateOrder(Order order)
    {
        var existingOrder = context.Order.Find(order.Id);
        if (order.Customer is not null)
        {
            var existingCustomer = context.Customer.Find(order.Customer?.Id);
            if (existingCustomer is null)
            {
                throw new InvalidOperationException("Cliente n�o encontrado.");
            }
            context.Entry(existingCustomer).State = EntityState.Detached;
        }

        if (existingOrder is null)
        {
            throw new InvalidOperationException("Pedido n�o encontrado.");
        }
        context.Entry(existingOrder).State = EntityState.Detached;
        context.Order.Update(order);

        context.SaveChanges();
        return order;
    }

    public async Task<List<Order>?> GetCurrentOrders() => await context.Order
        .Where(x => x.OrderStatus != OrderStatus.Finished || x.OrderStatus != OrderStatus.Cancelled)
        .OrderBy(x => x.InitialDate).ToListAsync();
}
