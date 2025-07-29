using QuiosqueFood3000.Api.Helpers;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Order.UnitTests.Helpers;

public class OrderSolicitationHelperTests
{
    [Fact]
    public void GenerateOrderByOrderSolicitation_WithValidSolicitation_ShouldGenerateOrder()
    {
        // Arrange
        var helper = new OrderSolicitationHelper();
        var orderSolicitation = new OrderSolicitation
        {
            OrderItemsList = new List<OrderItem> { new OrderItem() { Product = new Product() } },
            TotalValue = 100,
            TypeOfIdentification = TypeOfIdentification.Anonymous,
            AnonymousIdentification = Guid.NewGuid()
        };

        // Act
        var result = helper.GenerateOrderByOrderSolicitation(orderSolicitation);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(OrderStatus.Received, result.OrderStatus);
        Assert.Equal(PaymentStatus.PendingPayment, result.PaymentStatus);
        Assert.Equal(100, result.TotalValue);
    }

    [Fact]
    public void GenerateOrderByOrderSolicitation_WithInvalidSolicitation_ShouldThrowException()
    {
        // Arrange
        var helper = new OrderSolicitationHelper();
        var orderSolicitation = new OrderSolicitation(); // No items

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => helper.GenerateOrderByOrderSolicitation(orderSolicitation));
    }

    [Fact]
    public void ValidateOrderSolicitationDataForConfirmation_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var orderSolicitation = new OrderSolicitation
        {
            OrderItemsList = new List<OrderItem> { new OrderItem() { Product = new Product() } }
        };

        // Act
        var result = OrderSolicitationHelper.ValidateOrderSolicitationDataForConfirmation(orderSolicitation);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateOrderSolicitationDataForConfirmation_WithInvalidData_ShouldReturnFalse()
    {
        // Arrange
        var orderSolicitation = new OrderSolicitation(); // No items

        // Act
        var result = OrderSolicitationHelper.ValidateOrderSolicitationDataForConfirmation(orderSolicitation);

        // Assert
        Assert.False(result);
    }
}
