using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Api.Helpers
{
    public class OrderSolicitationHelper
    {
        public Order GenerateOrderByOrderSolicitation(OrderSolicitation orderSolicitation)
        {
            if (!ValidateOrderSolicitationDataForConfirmation(orderSolicitation))
            {
                throw new InvalidOperationException("A solicitação de pedido deve possuir ao menos 1 produto para a confirmação");
            }
            ;
            Order order = new Order()
            {
                OrderSolicitation = orderSolicitation,
                OrderStatus = OrderStatus.Received,
                InitialDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.PendingPayment,
                TotalValue = orderSolicitation.TotalValue,
                TypeOfIdentification = orderSolicitation.TypeOfIdentification,
                OrderItemsList = orderSolicitation.OrderItemsList
            };

            if (order.TypeOfIdentification == TypeOfIdentification.CpfIdentification)
            {
                order.Customer = orderSolicitation.Customer;
            }
            else if (order.TypeOfIdentification == TypeOfIdentification.Anonymous)
            {
                order.AnonymousIdentification = orderSolicitation.AnonymousIdentification;
            }

            return order;
        }
        public static bool ValidateOrderSolicitationDataForConfirmation(OrderSolicitation orderSolicitation) => !(orderSolicitation == null || orderSolicitation.OrderItemsList?.Count < 1);
    }
}
