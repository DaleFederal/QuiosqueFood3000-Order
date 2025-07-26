using Microsoft.EntityFrameworkCore;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Infraestructure.Persistence;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Infraestructure.Repositories;

public class OrderSolicitationRepository(ApplicationDbContext context) : IOrderSolicitationRepository
{
    public async Task<OrderSolicitation?> GetOrderSolicitationbyId(int id) => await context.OrderSolicitation
        .Include(o => o.Customer)
        .Include(o => o.OrderItemsList!)
        .ThenInclude(oi => oi.Product)
        .FirstOrDefaultAsync(o => o.Id == id);

    public OrderSolicitation RegisterOrderSolicitation(OrderSolicitation orderSolicitation)
    {
        context.OrderSolicitation.Add(orderSolicitation);
        context.SaveChanges();
        return orderSolicitation;
    }

    public OrderSolicitation UpdateOrderSolicitation(OrderSolicitation orderSolicitation)
    {
        ArgumentNullException.ThrowIfNull(orderSolicitation);

        var existingOrderSolicitation = context.OrderSolicitation
            .Include(o => o.Customer)
            .Include(o => o.OrderItemsList!)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefault(o => o.Id == orderSolicitation.Id) ?? throw new InvalidOperationException("Solicitação de pedido não encontrada.");

        context.Entry(existingOrderSolicitation).State = EntityState.Detached;
        if (orderSolicitation.Customer is not null)
        {
            var existingCustomer = context.Customer.Find(orderSolicitation.Customer.Id);
            if (existingCustomer != null)
            {
                context.Entry(existingCustomer).State = EntityState.Detached;
            }
        }

        foreach (var orderItem in existingOrderSolicitation.OrderItemsList!)
        {
            var existingOrderItem = context.OrderItem.Find(orderItem.Id);
            if (existingOrderItem != null)
            {
                context.Entry(existingOrderItem).State = EntityState.Detached;
                context.OrderItem.Update(orderItem);
            }

        }

        context.OrderSolicitation.Update(orderSolicitation);

        context.SaveChanges();
        return orderSolicitation;
    }
}
