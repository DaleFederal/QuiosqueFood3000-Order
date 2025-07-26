
using FluentValidation.TestHelper;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using Xunit;

namespace QuiosqueFood3000.Order.UnitTests.Validators
{
    public class OrderValidatorTests
    {
        private readonly OrderValidator _validator;

        public OrderValidatorTests()
        {
            _validator = new OrderValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenOrderItemsListIsEmpty()
        {
            var order = new QuiosqueFood3000.Domain.Entities.Order { TypeOfIdentification = TypeOfIdentification.Anonymous, PaymentStatus = PaymentStatus.Payed, OrderStatus = OrderStatus.Emitted, OrderSolicitation = new OrderSolicitation(), OrderItemsList = new List<OrderItem>(), TotalValue = 10 };
            var result = _validator.TestValidate(order);
            result.ShouldHaveValidationErrorFor(o => o.OrderItemsList);
        }

        [Fact]
        public void ShouldHaveErrorWhenTotalValueIsNegative()
        {
            var order = new QuiosqueFood3000.Domain.Entities.Order { TypeOfIdentification = TypeOfIdentification.Anonymous, PaymentStatus = PaymentStatus.Payed, OrderStatus = OrderStatus.Emitted, OrderSolicitation = new OrderSolicitation(), OrderItemsList = new List<OrderItem> { new OrderItem { Product = new Product() } }, TotalValue = -10 };
            var result = _validator.TestValidate(order);
            result.ShouldHaveValidationErrorFor(o => o.TotalValue).WithErrorMessage("O pedido deve possuir o valor igual ou maior que 0");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenOrderIsValid()
        {
            var order = new QuiosqueFood3000.Domain.Entities.Order { TypeOfIdentification = TypeOfIdentification.Anonymous, PaymentStatus = PaymentStatus.Payed, OrderStatus = OrderStatus.Emitted, OrderSolicitation = new OrderSolicitation(), OrderItemsList = new List<OrderItem> { new OrderItem { Product = new Product() } }, TotalValue = 10 };
            var result = _validator.TestValidate(order);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

