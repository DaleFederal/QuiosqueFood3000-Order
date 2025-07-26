
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Controllers;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;

namespace QuiosqueFood3000.Order.UnitTests.Controllers
{
    public class OrderSolicitationControllerTests
    {
        private readonly Mock<IOrderSolicitationService> _orderSolicitationServiceMock;
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly OrderSolicitationController _orderSolicitationController;

        public OrderSolicitationControllerTests()
        {
            _orderSolicitationServiceMock = new Mock<IOrderSolicitationService>();
            _customerServiceMock = new Mock<ICustomerService>();
            _orderSolicitationController = new OrderSolicitationController(_orderSolicitationServiceMock.Object, _customerServiceMock.Object);
        }

        [Fact]
        public async Task GetOrderSolicitationById_WhenOrderSolicitationExists_ReturnsOkResultWithOrderSolicitationDto()
        {
            // Arrange
            var id = 1;
            var orderSolicitationDto = new OrderSolicitationDto { Id = id.ToString() };
            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(id)).ReturnsAsync(orderSolicitationDto);

            // Act
            var result = await _orderSolicitationController.GetOrderSolicitationById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderSolicitationDto>(okResult.Value);
            Assert.Equal(id.ToString(), returnValue.Id);
        }

        [Fact]
        public async Task GetOrderSolicitationById_WhenOrderSolicitationDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var id = 1;
            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(id)).ReturnsAsync((OrderSolicitationDto)null);

            // Act
            var result = await _orderSolicitationController.GetOrderSolicitationById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderSolicitationById_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var id = 1;
            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(id)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _orderSolicitationController.GetOrderSolicitationById(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task AssociateCustomerToOrderSolicitation_WhenDataIsValid_ReturnsOkResultWithOrderSolicitationDto()
        {
            // Arrange
            var orderSolicitationId = 1;
            var customerCpf = "11144477735";
            var orderSolicitationDto = new OrderSolicitationDto { Id = orderSolicitationId.ToString() };
            var customerDto = new CustomerDto { Cpf = customerCpf };

            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(orderSolicitationId)).ReturnsAsync(orderSolicitationDto);
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerCpf)).ReturnsAsync(customerDto);
            _orderSolicitationServiceMock.Setup(x => x.AssociateCustomerToOrderSolicitation(customerDto, orderSolicitationDto)).Returns(orderSolicitationDto);

            // Act
            var result = await _orderSolicitationController.AssociateCustomerToOrderSolicitation(orderSolicitationId, customerCpf);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderSolicitationDto>(okResult.Value);
            Assert.Equal(orderSolicitationId.ToString(), returnValue.Id);
        }

        [Fact]
        public async Task AssociateCustomerToOrderSolicitation_WhenOrderSolicitationDoesNotExist_ReturnsBadRequestResult()
        {
            // Arrange
            var orderSolicitationId = 1;
            var customerCpf = "11144477735";
            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(orderSolicitationId)).ReturnsAsync((OrderSolicitationDto)null);

            // Act
            var result = await _orderSolicitationController.AssociateCustomerToOrderSolicitation(orderSolicitationId, customerCpf);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Solicitação de pedido não encontrada.", badRequestResult.Value);
        }

        [Fact]
        public async Task AssociateCustomerToOrderSolicitation_WhenCustomerDoesNotExist_ReturnsBadRequestResult()
        {
            // Arrange
            var orderSolicitationId = 1;
            var customerCpf = "11144477735";
            var orderSolicitationDto = new OrderSolicitationDto { Id = orderSolicitationId.ToString() };

            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(orderSolicitationId)).ReturnsAsync(orderSolicitationDto);
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerCpf)).ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _orderSolicitationController.AssociateCustomerToOrderSolicitation(orderSolicitationId, customerCpf);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cliente não encontrado.", badRequestResult.Value);
        }

        [Fact]
        public async Task AssociateCustomerToOrderSolicitation_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var orderSolicitationId = 1;
            var customerCpf = "11144477735";
            var orderSolicitationDto = new OrderSolicitationDto { Id = orderSolicitationId.ToString() };
            var customerDto = new CustomerDto { Cpf = customerCpf };

            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(orderSolicitationId)).ReturnsAsync(orderSolicitationDto);
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerCpf)).ReturnsAsync(customerDto);
            _orderSolicitationServiceMock.Setup(x => x.AssociateCustomerToOrderSolicitation(customerDto, orderSolicitationDto)).Throws(new Exception("Service error"));

            // Act
            var result = await _orderSolicitationController.AssociateCustomerToOrderSolicitation(orderSolicitationId, customerCpf);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task AssociateAnnonymousIdentificationToOrderSolicitation_WhenDataIsValid_ReturnsOkResultWithOrderSolicitationDto()
        {
            // Arrange
            var id = 1;
            var orderSolicitationDto = new OrderSolicitationDto { Id = id.ToString() };
            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(id)).ReturnsAsync(orderSolicitationDto);
            _orderSolicitationServiceMock.Setup(x => x.AssociateAnnonymousIdentificationToOrderSolicitation(orderSolicitationDto)).ReturnsAsync(orderSolicitationDto);

            // Act
            var result = await _orderSolicitationController.AssociateAnnonymousIdentificationToOrderSolicitation(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderSolicitationDto>(okResult.Value);
            Assert.Equal(id.ToString(), returnValue.Id);
        }

        [Fact]
        public async Task AssociateAnnonymousIdentificationToOrderSolicitation_WhenOrderSolicitationDoesNotExist_ReturnsBadRequestResult()
        {
            // Arrange
            var id = 1;
            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(id)).ReturnsAsync((OrderSolicitationDto)null);

            // Act
            var result = await _orderSolicitationController.AssociateAnnonymousIdentificationToOrderSolicitation(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Solicitação de pedido não encontrada.", badRequestResult.Value);
        }

        [Fact]
        public async Task AssociateAnnonymousIdentificationToOrderSolicitation_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var id = 1;
            var orderSolicitationDto = new OrderSolicitationDto { Id = id.ToString() };
            _orderSolicitationServiceMock.Setup(x => x.GetOrderSolicitationById(id)).ReturnsAsync(orderSolicitationDto);
            _orderSolicitationServiceMock.Setup(x => x.AssociateAnnonymousIdentificationToOrderSolicitation(orderSolicitationDto)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _orderSolicitationController.AssociateAnnonymousIdentificationToOrderSolicitation(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task AddOrderItemToOrder_WhenDataIsValid_ReturnsOkResultWithOrderSolicitationDto()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var observations = "Test";
            var orderSolicitationDto = new OrderSolicitationDto { Id = orderSolicitationId.ToString() };

            _orderSolicitationServiceMock.Setup(x => x.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations)).ReturnsAsync(orderSolicitationDto);

            // Act
            var result = await _orderSolicitationController.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderSolicitationDto>(okResult.Value);
            Assert.Equal(orderSolicitationId.ToString(), returnValue.Id);
        }

        [Fact]
        public async Task AddOrderItemToOrder_WhenServiceReturnsNull_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var observations = "Test";

            _orderSolicitationServiceMock.Setup(x => x.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations)).ReturnsAsync((OrderSolicitationDto)null);

            // Act
            var result = await _orderSolicitationController.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Não foi possível associar o cliente à solicitação de pedido.", badRequestResult.Value);
        }

        [Fact]
        public async Task AddOrderItemToOrder_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var observations = "Test";

            _orderSolicitationServiceMock.Setup(x => x.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _orderSolicitationController.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task RemoveOrderItemToOrder_WhenDataIsValid_ReturnsOkResultWithOrderSolicitationDto()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var orderSolicitationDto = new OrderSolicitationDto { Id = orderSolicitationId.ToString() };

            _orderSolicitationServiceMock.Setup(x => x.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId)).ReturnsAsync(orderSolicitationDto);

            // Act
            var result = await _orderSolicitationController.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderSolicitationDto>(okResult.Value);
            Assert.Equal(orderSolicitationId.ToString(), returnValue.Id);
        }

        [Fact]
        public async Task RemoveOrderItemToOrder_WhenServiceReturnsNull_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;

            _orderSolicitationServiceMock.Setup(x => x.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId)).ReturnsAsync((OrderSolicitationDto)null);

            // Act
            var result = await _orderSolicitationController.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Não foi possível associar o cliente à solicitação de pedido.", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveOrderItemToOrder_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;

            _orderSolicitationServiceMock.Setup(x => x.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _orderSolicitationController.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public void InitiateOrderSolicitation_WhenCalled_ReturnsOkResultWithOrderSolicitationDto()
        {
            // Arrange
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1" };
            _orderSolicitationServiceMock.Setup(x => x.InitiateOrderSolicitation()).Returns(orderSolicitationDto);

            // Act
            var result = _orderSolicitationController.InitiateOrderSolicitation();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderSolicitationDto>(okResult.Value);
            Assert.Equal("1", returnValue.Id);
        }

        [Fact]
        public void InitiateOrderSolicitation_WhenServiceReturnsNull_ReturnsBadRequestResult()
        {
            // Arrange
            _orderSolicitationServiceMock.Setup(x => x.InitiateOrderSolicitation()).Returns((OrderSolicitationDto)null);

            // Act
            var result = _orderSolicitationController.InitiateOrderSolicitation();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao iniciar a solicitação de pedido.", badRequestResult.Value);
        }

        [Fact]
        public void InitiateOrderSolicitation_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            _orderSolicitationServiceMock.Setup(x => x.InitiateOrderSolicitation()).Throws(new Exception("Service error"));

            // Act
            var result = _orderSolicitationController.InitiateOrderSolicitation();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }
    }
}
