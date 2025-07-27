using System.Net.Http;
using System.Text.Json;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Api.Services;

public class OrderService(IOrderRepository orderRepository, IPaymentService paymentService) : IOrderService
{
    public async Task<OrderDto?> GetOrderById(int id)
    {
        var order = await orderRepository.GetOrderbyId(id);

        return order == null
            ? null
            : new OrderDto()
            {
                Id = order.Id.ToString(),
                TypeOfIdentification = order.TypeOfIdentification,
                Customer = order.Customer,
                AnonymousIdentification = order.AnonymousIdentification,
                OrderSolicitation = order.OrderSolicitation,
                OrderStatus = order.OrderStatus,
                OrderItemsList = order.OrderItemsList,
                TotalValue = order.TotalValue,
                InitialDate = order.InitialDate,
                EndDate = order.EndDate ?? null,
                PaymentStatus = order.PaymentStatus,
            };
    }
    public async Task<List<OrderDto>?> GetOrdersByStatus(OrderStatus orderStatus)
    {
        var orders = await orderRepository.GetOrdersByStatus(orderStatus);
        return orders?.Select(order => new OrderDto()
        {
            Id = order.Id.ToString(),
            TypeOfIdentification = order.TypeOfIdentification,
            Customer = order.Customer,
            AnonymousIdentification = order.AnonymousIdentification,
            OrderSolicitation = order.OrderSolicitation,
            OrderStatus = order.OrderStatus,
            OrderItemsList = order.OrderItemsList,
            TotalValue = order.TotalValue,
            InitialDate = order.InitialDate,
            EndDate = order.EndDate ?? null,
            PaymentStatus = order.PaymentStatus,
        }).ToList();
    }



    public async Task<List<OrderDto>?> GetOrders()
    {
        var orders = await orderRepository.GetOrders();

        if (orders == null)
        {
            return null;
        }
        List<OrderDto> ordersDto = new List<OrderDto>();
        foreach (var order in orders)
        {
            OrderDto orderDto = new OrderDto()
            {
                Id = order.Id.ToString(),
                TypeOfIdentification = order.TypeOfIdentification,
                Customer = order.Customer,
                AnonymousIdentification = order.AnonymousIdentification,
                OrderSolicitation = order.OrderSolicitation,
                OrderStatus = order.OrderStatus,
                OrderItemsList = order.OrderItemsList,
                TotalValue = order.TotalValue,
                InitialDate = order.InitialDate,
                EndDate = order.EndDate ?? null,
                PaymentStatus = order.PaymentStatus,
            };
            ordersDto.Add(orderDto);
        }
        return ordersDto;
    }
    public void UpdateOrder(OrderDto orderDto)
    {
        if (orderDto == null)
        {
            throw new ArgumentNullException("Pedido deve ser informado");
        }
        if (string.IsNullOrEmpty(orderDto.Id))
        {
            throw new ArgumentNullException("Pedido deve ser informado");
        }
        Order order = new Order()
        {
            Id = int.Parse(orderDto.Id),
            TypeOfIdentification = orderDto.TypeOfIdentification ?? throw new ArgumentNullException(nameof(orderDto.TypeOfIdentification)),
            Customer = orderDto.Customer,
            AnonymousIdentification = orderDto.AnonymousIdentification,
            OrderSolicitation = orderDto.OrderSolicitation ?? throw new ArgumentNullException(nameof(orderDto.OrderSolicitation)),
            OrderStatus = orderDto.OrderStatus ?? throw new ArgumentNullException(nameof(orderDto.OrderStatus)),
            OrderItemsList = orderDto.OrderItemsList,
            TotalValue = orderDto.TotalValue ?? throw new ArgumentNullException(nameof(orderDto.TotalValue)),
            InitialDate = orderDto.InitialDate ?? throw new ArgumentNullException(nameof(orderDto.InitialDate)),
            EndDate = orderDto.EndDate,
            PaymentStatus = orderDto.PaymentStatus ?? throw new ArgumentNullException(nameof(orderDto.PaymentStatus)),
        };
        orderRepository.UpdateOrder(order);
    }
    public async Task<OrderDto> SendOrderToKitchenQueue(int id)
    {
        var order = await orderRepository.GetOrderbyId(id);


        if (order.OrderStatus != OrderStatus.Emitted)
        {
            throw new InvalidOperationException("Pedido já foi enviado para a cozinha");
        }

        if (order.PaymentStatus != PaymentStatus.Payed)
        {
            throw new InvalidOperationException("Pedido deve estar pago para ser enviado para a cozinha");
        }

        order.OrderStatus = OrderStatus.Received;

        orderRepository.UpdateOrder(order);

        // Send order to kitchen
        var httpClient = new HttpClient();
        var kitchenUrl = "http://quiosquekitchen:5002/OrderSolicitation";

        var kitchenOrder = new KitchenOrderSolicitationDto
        {
            Id = Guid.Parse(order.Id.ToString()),
            Status = OrderStatus.Received.ToString(),
            GenerateDate = order.InitialDate,
            CustomerId = order.Customer != null ? Guid.Parse(order.Customer.Id.ToString()) : (Guid?)null,
            AnonymousIdentification = order.AnonymousIdentification.ToString(),
            Products = order.OrderItemsList.Select(item => new KitchenProductDto
            {
                Id = Guid.NewGuid(),
                Name = item.Product.Name,
                Description = item.Observations
            }).ToList()
        };

        var json = System.Text.Json.JsonSerializer.Serialize(kitchenOrder);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(kitchenUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Failed to send order to kitchen: {response.ReasonPhrase}");
        }

        return new OrderDto()
        {
            Id = order.Id.ToString(),
            TypeOfIdentification = order.TypeOfIdentification,
            Customer = order.Customer,
            AnonymousIdentification = order.AnonymousIdentification,
            OrderSolicitation = order.OrderSolicitation,
            OrderStatus = order.OrderStatus,
            OrderItemsList = order.OrderItemsList,
            TotalValue = order.TotalValue,
            InitialDate = order.InitialDate,
            EndDate = null,
            PaymentStatus = order.PaymentStatus
        };
    }
    
    public OrderDto RegisterOrder(OrderDto orderDto)
    {
        Order order = new Order()
        {
            TypeOfIdentification = orderDto.TypeOfIdentification ?? throw new ArgumentNullException(nameof(orderDto.TypeOfIdentification)),
            Customer = orderDto.Customer,
            AnonymousIdentification = orderDto.AnonymousIdentification,
            OrderStatus = OrderStatus.Emitted,
            OrderItemsList = orderDto.OrderItemsList,
            TotalValue = orderDto.TotalValue ?? throw new ArgumentNullException(nameof(orderDto)),
            InitialDate = DateTime.UtcNow,
            OrderSolicitation = orderDto.OrderSolicitation ?? throw new ArgumentNullException(nameof(orderDto.OrderSolicitation)),
            PaymentStatus = PaymentStatus.NotPayed
        };
        OrderValidator orderValidator = new OrderValidator();
        var resultOrder = orderValidator.Validate(order);

        if (!resultOrder.IsValid)
        {
            throw new InvalidOperationException(resultOrder.ToString());
        }
        order = orderRepository.RegisterOrder(order);

        orderDto.Id = order.Id.ToString();
        paymentService.RequestPayment(orderDto);
        return orderDto;
    }

    public async Task<List<OrderDto>?> GetCurrentOrders()
    {
        var orders = await orderRepository.GetCurrentOrders();

        if (orders == null || orders.Count == 0)
            return null;
        
        var readyOrders = orders.Where(x => x.OrderStatus == OrderStatus.Ready);
        var preparingOrders = orders.Where(x => x.OrderStatus == OrderStatus.InProgress);
        var receivedOrders = orders.Where(x => x.OrderStatus == OrderStatus.Received);
        
        var orderEntityList = new List<Order>();
        orderEntityList.AddRange(readyOrders);
        orderEntityList.AddRange(preparingOrders);
        orderEntityList.AddRange(receivedOrders);

        var orderDtoList = new List<OrderDto>();
        foreach (var order in orderEntityList)
        {
            orderDtoList.Add(MappingOrderDto(order));
        }
        
        return orderDtoList;
    }

    public async Task ChangeOrderStatus(int id, OrderStatus status)
    {
        var order = await orderRepository.GetOrderbyId(id);

        if (order is null)
            throw new NullReferenceException("Pedido não encontrado");
        
        order.OrderStatus = status;
        orderRepository.UpdateOrder(order);
    }

    private OrderDto MappingOrderDto(Order order)
    {
        return new OrderDto()
        {
            Id = order.Id.ToString(),
            TypeOfIdentification = order.TypeOfIdentification,
            Customer = order.Customer,
            AnonymousIdentification = order.AnonymousIdentification,
            OrderSolicitation = order.OrderSolicitation,
            OrderStatus = order.OrderStatus,
            OrderItemsList = order.OrderItemsList,
            TotalValue = order.TotalValue,
            InitialDate = order.InitialDate,
            EndDate = null,
            PaymentStatus = order.PaymentStatus
        };
    }
}