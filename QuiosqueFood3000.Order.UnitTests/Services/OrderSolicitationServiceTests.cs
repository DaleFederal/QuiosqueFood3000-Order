
using Moq;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Order.UnitTests.Services
{
    public class OrderSolicitationServiceTests
    {
        private readonly Mock<IOrderSolicitationRepository> _orderSolicitationRepositoryMock;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly OrderSolicitationService _orderSolicitationService;

        public OrderSolicitationServiceTests()
        {
            _orderSolicitationRepositoryMock = new Mock<IOrderSolicitationRepository>();
            _orderServiceMock = new Mock<IOrderService>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _orderSolicitationService = new OrderSolicitationService(_orderSolicitationRepositoryMock.Object, _orderServiceMock.Object, _productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetOrderSolicitationById_WhenOrderSolicitationExists_ReturnsOrderSolicitationDto()
        {
            // Arrange
            var orderSolicitationId = 1;
            var orderSolicitation = new OrderSolicitation { Id = orderSolicitationId };
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync(orderSolicitation);

            // Act
            var result = await _orderSolicitationService.GetOrderSolicitationById(orderSolicitationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderSolicitation.Id.ToString(), result.Id);
        }

        [Fact]
        public async Task GetOrderSolicitationById_WhenOrderSolicitationDoesNotExist_ReturnsNull()
        {
            // Arrange
            var orderSolicitationId = 1;
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync((OrderSolicitation)null);

            // Act
            var result = await _orderSolicitationService.GetOrderSolicitationById(orderSolicitationId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void AssociateCustomerToOrderSolicitation_WhenDataIsValid_ReturnsOrderSolicitationDto()
        {
            // Arrange
            var customerDto = new CustomerDto { Id = "1" };
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1", TypeOfIdentification = TypeOfIdentification.Anonymous, OrderSolicitationStatus = OrderSolicitationStatus.InIdentification, TotalValue = 0, InitialDate = DateTime.Now };
            var orderSolicitation = new OrderSolicitation { Id = 1 };
            _orderSolicitationRepositoryMock.Setup(x => x.UpdateOrderSolicitation(It.IsAny<OrderSolicitation>())).Returns(orderSolicitation);

            // Act
            var result = _orderSolicitationService.AssociateCustomerToOrderSolicitation(customerDto, orderSolicitationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderSolicitation.Id.ToString(), result.Id);
        }

        [Fact]
        public void AssociateCustomerToOrderSolicitation_WhenCustomerIdIsNull_ThrowsArgumentException()
        {
            // Arrange
            var customerDto = new CustomerDto();
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1", TypeOfIdentification = TypeOfIdentification.Anonymous, OrderSolicitationStatus = OrderSolicitationStatus.InIdentification };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orderSolicitationService.AssociateCustomerToOrderSolicitation(customerDto, orderSolicitationDto));
        }

        [Fact]
        public void AssociateCustomerToOrderSolicitation_WhenAlreadyIdentified_ThrowsInvalidOperationException()
        {
            // Arrange
            var customerDto = new CustomerDto { Id = "1" };
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1", TypeOfIdentification = TypeOfIdentification.CpfIdentification, OrderSolicitationStatus = OrderSolicitationStatus.InIdentification };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _orderSolicitationService.AssociateCustomerToOrderSolicitation(customerDto, orderSolicitationDto));
        }

        [Fact]
        public void AssociateCustomerToOrderSolicitation_WhenNotInIdentificationStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var customerDto = new CustomerDto { Id = "1" };
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1", TypeOfIdentification = TypeOfIdentification.Anonymous, OrderSolicitationStatus = OrderSolicitationStatus.InProgress };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _orderSolicitationService.AssociateCustomerToOrderSolicitation(customerDto, orderSolicitationDto));
        }

        [Fact]
        public async Task AssociateAnnonymousIdentificationToOrderSolicitation_WhenDataIsValid_ReturnsOrderSolicitationDto()
        {
            // Arrange
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1", TypeOfIdentification = TypeOfIdentification.Anonymous, OrderSolicitationStatus = OrderSolicitationStatus.InIdentification };
            var orderSolicitation = new OrderSolicitation { Id = 1 };
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(1)).ReturnsAsync(orderSolicitation);
            _orderSolicitationRepositoryMock.Setup(x => x.UpdateOrderSolicitation(It.IsAny<OrderSolicitation>())).Returns(orderSolicitation);

            // Act
            var result = await _orderSolicitationService.AssociateAnnonymousIdentificationToOrderSolicitation(orderSolicitationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderSolicitation.Id.ToString(), result.Id);
        }

        [Fact]
        public async Task AssociateAnnonymousIdentificationToOrderSolicitation_WhenAlreadyIdentified_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1", TypeOfIdentification = TypeOfIdentification.CpfIdentification, OrderSolicitationStatus = OrderSolicitationStatus.InIdentification };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderSolicitationService.AssociateAnnonymousIdentificationToOrderSolicitation(orderSolicitationDto));
        }

        [Fact]
        public async Task AssociateAnnonymousIdentificationToOrderSolicitation_WhenNotInIdentificationStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderSolicitationDto = new OrderSolicitationDto { Id = "1", TypeOfIdentification = TypeOfIdentification.Anonymous, OrderSolicitationStatus = OrderSolicitationStatus.InProgress };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderSolicitationService.AssociateAnnonymousIdentificationToOrderSolicitation(orderSolicitationDto));
        }

        [Fact]
        public async Task AddOrderItemToOrder_WhenDataIsValid_ReturnsOrderSolicitationDto()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var observations = "Test";
            var product = new Product { Id = productId, Value = 10 };
            var orderSolicitation = new OrderSolicitation { Id = orderSolicitationId, OrderSolicitationStatus = OrderSolicitationStatus.InProgress, OrderItemsList = new List<OrderItem>() };
            _productRepositoryMock.Setup(x => x.GetProductbyId(productId)).ReturnsAsync(product);
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync(orderSolicitation);
            _orderSolicitationRepositoryMock.Setup(x => x.UpdateOrderSolicitation(It.IsAny<OrderSolicitation>())).Returns(orderSolicitation);

            // Act
            var result = await _orderSolicitationService.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.OrderItemsList);
        }

        [Fact]
        public async Task AddOrderItemToOrder_WhenOrderSolicitationNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var observations = "Test";
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync((OrderSolicitation)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderSolicitationService.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations));
        }

        [Fact]
        public async Task AddOrderItemToOrder_WhenProductNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var observations = "Test";
            var orderSolicitation = new OrderSolicitation { Id = orderSolicitationId, OrderSolicitationStatus = OrderSolicitationStatus.InProgress };
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync(orderSolicitation);
            _productRepositoryMock.Setup(x => x.GetProductbyId(productId)).ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderSolicitationService.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations));
        }

        [Fact]
        public async Task AddOrderItemToOrder_WhenNotInProgressStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var observations = "Test";
            var product = new Product { Id = productId, Value = 10 };
            var orderSolicitation = new OrderSolicitation { Id = orderSolicitationId, OrderSolicitationStatus = OrderSolicitationStatus.Finished };
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync(orderSolicitation);
            _productRepositoryMock.Setup(x => x.GetProductbyId(productId)).ReturnsAsync(product);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderSolicitationService.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations));
        }

        [Fact]
        public async Task RemoveOrderItemToOrder_WhenDataIsValid_ReturnsOrderSolicitationDto()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var product = new Product { Id = productId, Value = 10 };
            var orderItem = new OrderItem { Product = product, Quantity = 2 };
            var orderSolicitation = new OrderSolicitation { Id = orderSolicitationId, OrderSolicitationStatus = OrderSolicitationStatus.InProgress, OrderItemsList = new List<OrderItem> { orderItem } };
            _productRepositoryMock.Setup(x => x.GetProductbyId(productId)).ReturnsAsync(product);
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync(orderSolicitation);
            _orderSolicitationRepositoryMock.Setup(x => x.UpdateOrderSolicitation(It.IsAny<OrderSolicitation>())).Returns(orderSolicitation);

            // Act
            var result = await _orderSolicitationService.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderItemsList[0].Quantity);
        }

        [Fact]
        public async Task RemoveOrderItemToOrder_WhenProductNotInList_ThrowsInvalidOperationException()
        {
            // Arrange
            var productId = 1;
            var quantity = 1;
            var orderSolicitationId = 1;
            var product = new Product { Id = productId, Value = 10 };
            var orderSolicitation = new OrderSolicitation { Id = orderSolicitationId, OrderSolicitationStatus = OrderSolicitationStatus.InProgress, OrderItemsList = new List<OrderItem>() };
            _productRepositoryMock.Setup(x => x.GetProductbyId(productId)).ReturnsAsync(product);
            _orderSolicitationRepositoryMock.Setup(x => x.GetOrderSolicitationbyId(orderSolicitationId)).ReturnsAsync(orderSolicitation);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderSolicitationService.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId));
        }

        [Fact]
        public void InitiateOrderSolicitation_WhenCalled_ReturnsOrderSolicitationDto()
        {
            // Arrange
            var orderSolicitation = new OrderSolicitation { Id = 1 };
            _orderSolicitationRepositoryMock.Setup(x => x.RegisterOrderSolicitation(It.IsAny<OrderSolicitation>())).Returns(orderSolicitation);

            // Act
            var result = _orderSolicitationService.InitiateOrderSolicitation();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderSolicitation.Id.ToString(), result.Id);
        }
    }
}
