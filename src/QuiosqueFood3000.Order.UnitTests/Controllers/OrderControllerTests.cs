
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Controllers;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Order.UnitTests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderController = new OrderController(_orderServiceMock.Object);
        }

        [Fact]
        public async Task OrderById_WhenOrderExists_ReturnsOkResultWithOrderDto()
        {
            // Arrange
            var orderId = 1;
            var orderDto = new OrderDto { Id = orderId.ToString() };
            _orderServiceMock.Setup(x => x.GetOrderById(orderId)).ReturnsAsync(orderDto);

            // Act
            var result = await _orderController.OrderById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(orderId.ToString(), returnValue.Id);
        }

        [Fact]
        public async Task OrderById_WhenOrderDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            _orderServiceMock.Setup(x => x.GetOrderById(orderId)).ReturnsAsync((OrderDto)null);

            // Act
            var result = await _orderController.OrderById(orderId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async Task GetOrders_WhenOrdersExist_ReturnsOkResultWithOrderDtoList()
        {
            // Arrange
            var ordersDto = new List<OrderDto> { new OrderDto { Id = "1" } };
            _orderServiceMock.Setup(x => x.GetOrders()).ReturnsAsync(ordersDto);

            // Act
            var result = await _orderController.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetOrders_WhenOrdersDoNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            _orderServiceMock.Setup(x => x.GetOrders()).ReturnsAsync((List<OrderDto>)null);

            // Act
            var result = await _orderController.GetOrders();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }        

        [Fact]
        public async Task GetOrdersByStatus_WhenOrdersExist_ReturnsOkResultWithOrderDtoList()
        {
            // Arrange
            var orderStatus = OrderStatus.Emitted;
            var ordersDto = new List<OrderDto> { new OrderDto { Id = "1", OrderStatus = orderStatus } };
            _orderServiceMock.Setup(x => x.GetOrdersByStatus(orderStatus)).ReturnsAsync(ordersDto);

            // Act
            var result = await _orderController.GetOrdersByStatus(orderStatus);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetOrdersByStatus_WhenOrdersDoNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var orderStatus = OrderStatus.Emitted;
            _orderServiceMock.Setup(x => x.GetOrdersByStatus(orderStatus)).ReturnsAsync((List<OrderDto>)null);

            // Act
            var result = await _orderController.GetOrdersByStatus(orderStatus);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async Task GetPaymentOrdersStatusById_WhenOrderExists_ReturnsOkResultWithPaymentStatus()
        {
            // Arrange
            var orderId = 1;
            var orderDto = new OrderDto { Id = orderId.ToString(), PaymentStatus = PaymentStatus.Payed };
            _orderServiceMock.Setup(x => x.GetOrderById(orderId)).ReturnsAsync(orderDto);

            // Act
            var result = await _orderController.GetPaymentOrdersStatusById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(PaymentStatus.Payed, okResult.Value);
        }

        [Fact]
        public async Task GetPaymentOrdersStatusById_WhenOrderDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            _orderServiceMock.Setup(x => x.GetOrderById(orderId)).ReturnsAsync((OrderDto)null);

            // Act
            var result = await _orderController.GetPaymentOrdersStatusById(orderId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }        

        [Fact]
        public async Task GetCurrentOrders_WhenOrdersExist_ReturnsOkResultWithOrderDtoList()
        {
            // Arrange
            var ordersDto = new List<OrderDto> { new OrderDto { Id = "1" } };
            _orderServiceMock.Setup(x => x.GetCurrentOrders()).ReturnsAsync(ordersDto);

            // Act
            var result = await _orderController.GetCurrentOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetCurrentOrders_WhenOrdersDoNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            _orderServiceMock.Setup(x => x.GetCurrentOrders()).ReturnsAsync((List<OrderDto>)null);

            // Act
            var result = await _orderController.GetCurrentOrders();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }        

        [Fact]
        public async Task OrderChangeStatus_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var newOrderStatus = OrderStatus.InProgress;
            _orderServiceMock.Setup(x => x.ChangeOrderStatus(orderId, newOrderStatus)).Returns(Task.CompletedTask);

            // Act
            var result = await _orderController.OrderChangeStatus(orderId, newOrderStatus);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Pedido de identificação: {orderId} Status novo: {newOrderStatus}", okResult.Value);
            _orderServiceMock.Verify(x => x.ChangeOrderStatus(orderId, newOrderStatus), Times.Once);
        }        
    }
}
