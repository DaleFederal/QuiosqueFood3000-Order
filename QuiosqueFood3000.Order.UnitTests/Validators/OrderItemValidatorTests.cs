
using FluentValidation.TestHelper;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using Xunit;

namespace QuiosqueFood3000.Order.UnitTests.Validators
{
    public class OrderItemValidatorTests
    {
        private readonly OrderItemValidator _validator;

        public OrderItemValidatorTests()
        {
            _validator = new OrderItemValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenTotalValueIsNegative()
        {
            var orderItem = new OrderItem { Product = new Product(), TotalValue = -10, Quantity = 1 };
            var result = _validator.TestValidate(orderItem);
            result.ShouldHaveValidationErrorFor(orderItem => orderItem.TotalValue).WithErrorMessage("O item de pedido deve possuir o valor igual ou maior que 0");
        }

        [Fact]
        public void ShouldHaveErrorWhenQuantityIsZeroOrNegative()
        {
            var orderItem = new OrderItem { Product = new Product(), TotalValue = 10, Quantity = 0 };
            var result = _validator.TestValidate(orderItem);
            result.ShouldHaveValidationErrorFor(orderItem => orderItem.Quantity).WithErrorMessage("A quantidade de item de pedido deve ao menos 1");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenOrderItemIsValid()
        {
            var orderItem = new OrderItem { Product = new Product(), TotalValue = 10, Quantity = 1 };
            var result = _validator.TestValidate(orderItem);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
