
using Moq;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Order.UnitTests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _paymentService = new PaymentService(_orderRepositoryMock.Object, _httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentIdIsEmpty_ThrowsApplicationException()
        {
            // Arrange
            var paymentDto = new PaymentDto { PaymentId = Guid.Empty };

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _paymentService.ProcessPayment(paymentDto));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentStatusIsPending_DoesNothing()
        {
            // Arrange
            var paymentDto = new PaymentDto { PaymentId = Guid.NewGuid(), PaymentStatus = PaymentStatus.PendingPayment };

            // Act
            await _paymentService.ProcessPayment(paymentDto);

            // Assert
            _orderRepositoryMock.Verify(x => x.GetOrderbyId(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task ProcessPayment_WhenOrderNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var paymentDto = new PaymentDto { PaymentId = Guid.NewGuid(), PaymentStatus = PaymentStatus.Payed, OrderId = 1 };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(It.IsAny<int>())).ReturnsAsync((QuiosqueFood3000.Domain.Entities.Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _paymentService.ProcessPayment(paymentDto));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentStatusIsPayed_UpdatesOrderAndSendsToKitchenQueue()
        {
            // Arrange
            var paymentDto = new PaymentDto { PaymentId = Guid.NewGuid(), PaymentStatus = PaymentStatus.Payed, OrderId = 1 };
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = 1, OrderSolicitation = new OrderSolicitation() };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(It.IsAny<int>())).ReturnsAsync(order);

            // Act
            await _paymentService.ProcessPayment(paymentDto);

            // Assert
            _orderRepositoryMock.Verify(x => x.UpdateOrder(It.Is<QuiosqueFood3000.Domain.Entities.Order>(o => o.PaymentStatus == PaymentStatus.Payed)), Times.Once);
            
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentStatusIsNotPayed_UpdatesOrderAndDoesNotSendToKitchenQueue()
        {
            // Arrange
            var paymentDto = new PaymentDto { PaymentId = Guid.NewGuid(), PaymentStatus = PaymentStatus.NotPayed, OrderId = 1 };
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = 1, OrderSolicitation = new OrderSolicitation() };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(It.IsAny<int>())).ReturnsAsync(order);

            // Act
            await _paymentService.ProcessPayment(paymentDto);

            // Assert
            _orderRepositoryMock.Verify(x => x.UpdateOrder(It.Is<QuiosqueFood3000.Domain.Entities.Order>(o => o.PaymentStatus == PaymentStatus.NotPayed)), Times.Once);
            
        }
    }
}
