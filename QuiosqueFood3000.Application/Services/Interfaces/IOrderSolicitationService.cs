using QuiosqueFood3000.Api.DTOs;

namespace QuiosqueFood3000.Api.Services.Interfaces;

public interface IOrderSolicitationService
{
    Task<OrderSolicitationDto?> GetOrderSolicitationById(int id);
    OrderSolicitationDto? AssociateCustomerToOrderSolicitation(CustomerDto customerDto, OrderSolicitationDto orderSolicitationDto);
    Task<OrderSolicitationDto?> AssociateAnnonymousIdentificationToOrderSolicitation(OrderSolicitationDto orderSolicitationDto);
    Task<OrderSolicitationDto?> AddOrderItemToOrder(int productId, int quantity, int orderSolicitationId, string observations);
    Task<OrderSolicitationDto?> RemoveOrderItemToOrder(int productId, int quantity, int orderSolicitationId);
    OrderSolicitationDto? InitiateOrderSolicitation();
}