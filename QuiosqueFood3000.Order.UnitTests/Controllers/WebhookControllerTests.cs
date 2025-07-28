using Microsoft.AspNetCore.Mvc;
using Moq;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Controllers;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Order.UnitTests.Controllers;

public class WebhookControllerTests
{
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly WebhookController _controller;

    public WebhookControllerTests()
    {
        _paymentServiceMock = new Mock<IPaymentService>();
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new WebhookController(_paymentServiceMock.Object, _orderServiceMock.Object);
    }

    [Fact]
    public async Task UpdatePaymentStatus_WhenPaymentIsPayed_ShouldProcessPaymentAndSendToKitchen()
    {
        // Arrange
        var paymentDto = new PaymentDto { OrderId = 1, PaymentStatus = PaymentStatus.Payed };

        // Act
        var result = await _controller.UpdatePaymentStatus(paymentDto);

        // Assert
        _paymentServiceMock.Verify(s => s.ProcessPayment(paymentDto), Times.Once);
        _orderServiceMock.Verify(s => s.SendOrderToKitchenQueue(paymentDto.OrderId), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdatePaymentStatus_WhenPaymentIsNotPayed_ShouldOnlyProcessPayment()
    {
        // Arrange
        var paymentDto = new PaymentDto { OrderId = 1, PaymentStatus = PaymentStatus.PendingPayment };

        // Act
        var result = await _controller.UpdatePaymentStatus(paymentDto);

        // Assert
        _paymentServiceMock.Verify(s => s.ProcessPayment(paymentDto), Times.Once);
        _orderServiceMock.Verify(s => s.SendOrderToKitchenQueue(It.IsAny<int>()), Times.Never);
        Assert.IsType<OkResult>(result);
    }
}
