
using Moq;
using Moq.Protected;
using System.Net.Http;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Order.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _paymentServiceMock = new Mock<IPaymentService>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _orderService = new OrderService(_orderRepositoryMock.Object, _paymentServiceMock.Object, _httpClientFactoryMock.Object);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json")
                });

            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(_httpMessageHandlerMock.Object));
        }

        [Fact]
        public async Task GetOrderById_WhenOrderExists_ReturnsOrderDto()
        {
            // Arrange
            var orderId = 1;
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = orderId, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Emitted, TotalValue = 10 };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderById(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id.ToString(), result.Id);
        }

        [Fact]
        public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var orderId = 1;
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(orderId)).ReturnsAsync((QuiosqueFood3000.Domain.Entities.Order)null);

            // Act
            var result = await _orderService.GetOrderById(orderId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrdersByStatus_WhenOrdersExist_ReturnsOrderDtoList()
        {
            // Arrange
            var orderStatus = OrderStatus.Emitted;
            var orders = new List<QuiosqueFood3000.Domain.Entities.Order> { new QuiosqueFood3000.Domain.Entities.Order { Id = 1, OrderSolicitation = new OrderSolicitation(), OrderStatus = orderStatus, TotalValue = 10 } };
            _orderRepositoryMock.Setup(x => x.GetOrdersByStatus(orderStatus)).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetOrdersByStatus(orderStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(orders[0].Id.ToString(), result[0].Id);
        }

        [Fact]
        public async Task GetOrdersByStatus_WhenOrdersDoNotExist_ReturnsNull()
        {
            // Arrange
            var orderStatus = OrderStatus.Emitted;
            _orderRepositoryMock.Setup(x => x.GetOrdersByStatus(orderStatus)).ReturnsAsync((List<QuiosqueFood3000.Domain.Entities.Order>)null);

            // Act
            var result = await _orderService.GetOrdersByStatus(orderStatus);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrders_WhenOrdersExist_ReturnsOrderDtoList()
        {
            // Arrange
            var orders = new List<QuiosqueFood3000.Domain.Entities.Order> { new QuiosqueFood3000.Domain.Entities.Order { Id = 1, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Emitted, TotalValue = 10 } };
            _orderRepositoryMock.Setup(x => x.GetOrders()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(orders[0].Id.ToString(), result[0].Id);
        }

        [Fact]
        public async Task GetOrders_WhenOrdersDoNotExist_ReturnsNull()
        {
            // Arrange
            _orderRepositoryMock.Setup(x => x.GetOrders()).ReturnsAsync((List<QuiosqueFood3000.Domain.Entities.Order>)null);

            // Act
            var result = await _orderService.GetOrders();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateOrder_WhenOrderDtoIsValid_CallsUpdateOrder()
        {
            // Arrange
            var orderDto = new OrderDto { Id = "1", TypeOfIdentification = TypeOfIdentification.CpfIdentification, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Emitted, TotalValue = 10, InitialDate = DateTime.Now, PaymentStatus = PaymentStatus.Payed };

            // Act
            _orderService.UpdateOrder(orderDto);

            // Assert
            _orderRepositoryMock.Verify(x => x.UpdateOrder(It.IsAny<QuiosqueFood3000.Domain.Entities.Order>()), Times.Once);
        }

        [Fact]
        public void UpdateOrder_WhenOrderDtoIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _orderService.UpdateOrder(null));
        }

        [Fact]
        public void UpdateOrder_WhenOrderIdIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var orderDto = new OrderDto();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _orderService.UpdateOrder(orderDto));
        }

        [Fact]
        public async Task SendOrderToKitchenQueue_WhenOrderIsValid_ReturnsOrderDto()
        {
            // Arrange
            var orderId = 1;
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = orderId, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Emitted, PaymentStatus = PaymentStatus.Payed, TotalValue = 10, OrderItemsList = new List<OrderItem>() };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.SendOrderToKitchenQueue(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Received, result.OrderStatus);
        }

        [Fact]
        public async Task SendOrderToKitchenQueue_WhenOrderStatusIsNotEmitted_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderId = 1;
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = orderId, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Received, PaymentStatus = PaymentStatus.Payed, TotalValue = 10 };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(orderId)).ReturnsAsync(order);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.SendOrderToKitchenQueue(orderId));
        }

        [Fact]
        public async Task SendOrderToKitchenQueue_WhenPaymentStatusIsNotPayed_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderId = 1;
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = orderId, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Emitted, PaymentStatus = PaymentStatus.NotPayed, TotalValue = 10 };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(orderId)).ReturnsAsync(order);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.SendOrderToKitchenQueue(orderId));
        }

        [Fact]
        public void RegisterOrder_WhenOrderDtoIsValid_ReturnsOrderDto()
        {
            // Arrange
            var orderDto = new OrderDto { TypeOfIdentification = TypeOfIdentification.CpfIdentification, OrderSolicitation = new OrderSolicitation(), TotalValue = 10, OrderItemsList = new List<OrderItem> { new OrderItem { Product = new Product() } } };
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = 1, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Emitted, TotalValue = 10 };
            _orderRepositoryMock.Setup(x => x.RegisterOrder(It.IsAny<QuiosqueFood3000.Domain.Entities.Order>())).Returns(order);

            // Act
            var result = _orderService.RegisterOrder(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id.ToString(), result.Id);
        }

        [Fact]
        public void RegisterOrder_WhenOrderDtoIsInvalid_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderDto = new OrderDto { TypeOfIdentification = TypeOfIdentification.CpfIdentification, OrderSolicitation = new OrderSolicitation(), TotalValue = 10 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _orderService.RegisterOrder(orderDto));
        }

        [Fact]
        public async Task GetCurrentOrders_WhenOrdersExist_ReturnsOrderDtoList()
        {
            // Arrange
            var orders = new List<QuiosqueFood3000.Domain.Entities.Order>
            {
                new QuiosqueFood3000.Domain.Entities.Order { Id = 1, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Ready, TotalValue = 10 },
                new QuiosqueFood3000.Domain.Entities.Order { Id = 2, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.InProgress, TotalValue = 20 },
                new QuiosqueFood3000.Domain.Entities.Order { Id = 3, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Received, TotalValue = 30 }
            };
            _orderRepositoryMock.Setup(x => x.GetCurrentOrders()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetCurrentOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(orders[0].Id.ToString(), result[0].Id);
            Assert.Equal(orders[1].Id.ToString(), result[1].Id);
            Assert.Equal(orders[2].Id.ToString(), result[2].Id);
        }

        [Fact]
        public async Task GetCurrentOrders_WhenNoOrdersExist_ReturnsNull()
        {
            // Arrange
            _orderRepositoryMock.Setup(x => x.GetCurrentOrders()).ReturnsAsync(new List<QuiosqueFood3000.Domain.Entities.Order>());

            // Act
            var result = await _orderService.GetCurrentOrders();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ChangeOrderStatus_WhenOrderExists_CallsUpdateOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new QuiosqueFood3000.Domain.Entities.Order { Id = orderId, OrderSolicitation = new OrderSolicitation(), OrderStatus = OrderStatus.Emitted, TotalValue = 10 };
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(orderId)).ReturnsAsync(order);

            // Act
            await _orderService.ChangeOrderStatus(orderId, OrderStatus.InProgress);

            // Assert
            _orderRepositoryMock.Verify(x => x.UpdateOrder(It.Is<QuiosqueFood3000.Domain.Entities.Order>(o => o.OrderStatus == OrderStatus.InProgress)), Times.Once);
        }

        [Fact]
        public async Task ChangeOrderStatus_WhenOrderDoesNotExist_ThrowsNullReferenceException()
        {
            // Arrange
            var orderId = 1;
            _orderRepositoryMock.Setup(x => x.GetOrderbyId(orderId)).ReturnsAsync((QuiosqueFood3000.Domain.Entities.Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _orderService.ChangeOrderStatus(orderId, OrderStatus.InProgress));
        }
    }
}
