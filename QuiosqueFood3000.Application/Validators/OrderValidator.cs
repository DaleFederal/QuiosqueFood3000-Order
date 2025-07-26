using FluentValidation;
using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Api.Validators;
public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(order => order.TypeOfIdentification).NotNull().IsInEnum().WithMessage("O tipo de identificação não é válido para o pedido");
        RuleFor(order => order.PaymentStatus).NotNull().IsInEnum().WithMessage("O status de pagamento não é válido para o pedido");
        RuleFor(order => order.OrderStatus).NotNull().IsInEnum().WithMessage("O status do pedido não é válido para o pedido");
        RuleFor(order => order.OrderSolicitation).NotNull().NotEmpty().WithMessage("O pedido deve possuir uma solicitação de pedido valida");
        RuleFor(order => order.OrderItemsList).NotNull().NotEmpty().WithMessage("O pedido deve possuir ao menos 1 produto");
        RuleFor(order => order.TotalValue).NotNull().NotEmpty().Must(totalValue => totalValue >= 0).WithMessage("O pedido deve possuir o valor igual ou maior que 0");
    }
}