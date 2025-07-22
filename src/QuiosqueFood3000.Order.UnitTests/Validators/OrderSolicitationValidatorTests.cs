
using FluentValidation.TestHelper;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using Xunit;

namespace QuiosqueFood3000.Order.UnitTests.Validators
{
    public class OrderSolicitationValidatorTests
    {
        private readonly OrderSolicitationValidator _validator;

        public OrderSolicitationValidatorTests()
        {
            _validator = new OrderSolicitationValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenTotalValueIsNegative()
        {
            var orderSolicitation = new OrderSolicitation { TypeOfIdentification = TypeOfIdentification.Anonymous, OrderSolicitationStatus = OrderSolicitationStatus.InIdentification, TotalValue = -10 };
            var result = _validator.TestValidate(orderSolicitation);
            result.ShouldHaveValidationErrorFor(os => os.TotalValue).WithErrorMessage("O pedido deve possuir o valor igual ou maior que 0");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenOrderSolicitationIsValid()
        {
            var orderSolicitation = new OrderSolicitation { TypeOfIdentification = TypeOfIdentification.Anonymous, OrderSolicitationStatus = OrderSolicitationStatus.InIdentification, TotalValue = 10 };
            var result = _validator.TestValidate(orderSolicitation);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
