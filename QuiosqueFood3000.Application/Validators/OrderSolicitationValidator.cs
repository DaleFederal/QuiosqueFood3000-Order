using FluentValidation;
using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Api.Validators;
public class OrderSolicitationValidator : AbstractValidator<OrderSolicitation>
{
    public OrderSolicitationValidator()
    {
        RuleFor(orderSolicitation => orderSolicitation.TypeOfIdentification).NotNull().IsInEnum().WithMessage("O tipo de identificação não é válido para a solicitação de pedido");
        RuleFor(orderSolicitation => orderSolicitation.OrderSolicitationStatus).NotNull().IsInEnum().WithMessage("O status da solicitação de pedido não é válido para a solicitação de pedido");
        RuleFor(orderSolicitation => orderSolicitation.TotalValue).NotNull().NotEmpty().Must(totalValue => totalValue >= 0).WithMessage("O pedido deve possuir o valor igual ou maior que 0");
    }
}