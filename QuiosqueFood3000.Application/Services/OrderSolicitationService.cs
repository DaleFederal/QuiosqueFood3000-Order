using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Api.Services;

public class OrderSolicitationService(IOrderSolicitationRepository OrderSolicitationRepository, IOrderService OrderService, IProductRepository ProductRepository) : IOrderSolicitationService
{
    public async Task<OrderSolicitationDto?> GetOrderSolicitationById(int id)
    {
        var orderSolicitation = await OrderSolicitationRepository.GetOrderSolicitationbyId(id);

        return orderSolicitation == null
            ? null
            : new OrderSolicitationDto()
            {
                Id = orderSolicitation.Id.ToString(),
                TypeOfIdentification = this.GetTreatedTypeOfIdentification(orderSolicitation.Customer),
                Customer = orderSolicitation.Customer,
                AnonymousIdentification = orderSolicitation.AnonymousIdentification,
                OrderSolicitationStatus = orderSolicitation.OrderSolicitationStatus,
                OrderItemsList = orderSolicitation.OrderItemsList,
                TotalValue = orderSolicitation.TotalValue,
                InitialDate = orderSolicitation.InitialDate,
                EndDate = orderSolicitation.EndDate,

            };
    }
    private TypeOfIdentification GetTreatedTypeOfIdentification(Customer? customer)
    {
        if (customer is null)
        {
            return TypeOfIdentification.Anonymous;
        }
        return TypeOfIdentification.CpfIdentification;
    }
    public OrderSolicitationDto? AssociateCustomerToOrderSolicitation(CustomerDto customerDto, OrderSolicitationDto orderSolicitationDto)
    {
        ArgumentNullException.ThrowIfNull(customerDto);
        ArgumentNullException.ThrowIfNull(orderSolicitationDto);

        if (string.IsNullOrEmpty(customerDto.Id))
        {
            throw new ArgumentException("O cliente deve ser informado");
        }

        if (orderSolicitationDto.TypeOfIdentification == TypeOfIdentification.CpfIdentification)
        {
            throw new InvalidOperationException("Solicitação de pedido já está associada à um cliente.");
        }
        if (orderSolicitationDto.OrderSolicitationStatus != OrderSolicitationStatus.InIdentification)
        {
            throw new InvalidOperationException("Solicitação de pedido já está associada à uma identificação.");
        }

        OrderSolicitation orderSolicitation = new OrderSolicitation()
        {
            Id = int.Parse(orderSolicitationDto.Id),
            TypeOfIdentification = TypeOfIdentification.CpfIdentification,
            Customer = orderSolicitationDto.Customer,
            AnonymousIdentification = orderSolicitationDto.AnonymousIdentification,
            OrderSolicitationStatus = OrderSolicitationStatus.InProgress,
            OrderItemsList = orderSolicitationDto.OrderItemsList,
            TotalValue = orderSolicitationDto.TotalValue ?? throw new ArgumentNullException(nameof(orderSolicitationDto.TotalValue)),
            InitialDate = orderSolicitationDto.InitialDate ?? throw new ArgumentNullException(nameof(orderSolicitationDto.InitialDate)),
            EndDate = orderSolicitationDto.EndDate,
        };

        Customer customer = new Customer()
        {
            Id = int.Parse(customerDto.Id),
            Cpf = customerDto.Cpf,
            Name = customerDto.Name,
            Email = customerDto.Email
        };

        orderSolicitation.Customer = customer;
        orderSolicitation = OrderSolicitationRepository.UpdateOrderSolicitation(orderSolicitation);

        return new OrderSolicitationDto()
        {
            Id = orderSolicitation.Id.ToString(),
            TypeOfIdentification = this.GetTreatedTypeOfIdentification(orderSolicitation.Customer),
            Customer = orderSolicitation.Customer,
            AnonymousIdentification = orderSolicitation.AnonymousIdentification,
            OrderSolicitationStatus = orderSolicitation.OrderSolicitationStatus,
            OrderItemsList = orderSolicitation.OrderItemsList,
            TotalValue = orderSolicitation.TotalValue,
            InitialDate = orderSolicitation.InitialDate,
            EndDate = orderSolicitation.EndDate,
        };
    }

    public async Task<OrderSolicitationDto?> AssociateAnnonymousIdentificationToOrderSolicitation(OrderSolicitationDto orderSolicitationDto)
    {
        ArgumentNullException.ThrowIfNull(orderSolicitationDto);

        if (orderSolicitationDto.TypeOfIdentification == TypeOfIdentification.CpfIdentification)
        {
            throw new InvalidOperationException("Solicitação de pedido já está associada à um cliente.");
        }
        if (orderSolicitationDto.OrderSolicitationStatus != OrderSolicitationStatus.InIdentification)
        {
            throw new InvalidOperationException("Solicitação de pedido já está associada à uma identificação.");
        }

        var orderSolicitation = await OrderSolicitationRepository.GetOrderSolicitationbyId(int.Parse(orderSolicitationDto.Id))
            ?? throw new KeyNotFoundException("Solicitação de pedido não encontrada");

        orderSolicitation.AnonymousIdentification = Guid.NewGuid();
        orderSolicitation.OrderSolicitationStatus = OrderSolicitationStatus.InProgress;
        orderSolicitation = OrderSolicitationRepository.UpdateOrderSolicitation(orderSolicitation);

        return new OrderSolicitationDto()
        {
            Id = orderSolicitation.Id.ToString(),
            TypeOfIdentification = orderSolicitation.TypeOfIdentification,
            Customer = orderSolicitation.Customer,
            AnonymousIdentification = orderSolicitation.AnonymousIdentification,
            OrderSolicitationStatus = orderSolicitation.OrderSolicitationStatus,
            OrderItemsList = orderSolicitation.OrderItemsList,
            TotalValue = orderSolicitation.TotalValue,
            InitialDate = orderSolicitation.InitialDate,
            EndDate = orderSolicitation.EndDate,
        };
    }

    public async Task<OrderSolicitationDto?> AddOrderItemToOrder(int productId, int quantity, int orderSolicitationId, string? observations)
    {
        ArgumentNullException.ThrowIfNull(productId);
        ArgumentNullException.ThrowIfNull(orderSolicitationId);

        var orderSolicitation = await OrderSolicitationRepository.GetOrderSolicitationbyId(orderSolicitationId) ?? throw new InvalidOperationException("Solicitação de pedido não encontrada.");

        var product = await ProductRepository.GetProductbyId(productId) ?? throw new InvalidOperationException("Produto não encontrado.");

        if (orderSolicitation.OrderItemsList == null)
        {
            orderSolicitation.OrderItemsList = new List<OrderItem>();
        }

        if (orderSolicitation.OrderSolicitationStatus != OrderSolicitationStatus.InProgress)
        {
            throw new InvalidOperationException("Solicitação de pedido não está em andamento.");
        }


        var orderItem = new OrderItem()
        {
            Product = product,
            Quantity = quantity,
            TotalValue = product.Value * quantity,
            Observations = observations,
        };
        OrderItemValidator orderItemValidator = new OrderItemValidator();
        var resultOrderItem = orderItemValidator.Validate(orderItem);

        if (!resultOrderItem.IsValid)
        {
            throw new InvalidDataException(resultOrderItem.ToString());
        }

        if (orderSolicitation.OrderItemsList?.Count < 1)
        {
            orderSolicitation.OrderItemsList.Add(orderItem);
            orderSolicitation.TotalValue = this.CalculateTotalValue(orderSolicitation);
            OrderSolicitationRepository.UpdateOrderSolicitation(orderSolicitation);
        }
        else
        {
            int indexToChange = (int)(orderSolicitation.OrderItemsList?.FindIndex(orderItem => orderItem.Product.Id == productId));

            if (indexToChange == -1)
            {
                orderSolicitation.OrderItemsList.Add(orderItem);

            }
            else
            {
                orderSolicitation.OrderItemsList[indexToChange].Quantity += quantity;
                resultOrderItem = orderItemValidator.Validate(orderSolicitation.OrderItemsList[indexToChange]);

                if (!resultOrderItem.IsValid)
                {
                    throw new InvalidOperationException(resultOrderItem.ToString());
                }
            }
            resultOrderItem = orderItemValidator.Validate(orderItem);

            if (!resultOrderItem.IsValid)
            {
                throw new InvalidDataException(resultOrderItem.ToString());
            }
            orderSolicitation.TotalValue = this.CalculateTotalValue(orderSolicitation);
            orderSolicitation = OrderSolicitationRepository.UpdateOrderSolicitation(orderSolicitation);
        }

        return new OrderSolicitationDto()
        {
            Id = orderSolicitation.Id.ToString(),
            TypeOfIdentification = this.GetTreatedTypeOfIdentification(orderSolicitation.Customer),
            Customer = orderSolicitation.Customer,
            AnonymousIdentification = orderSolicitation.AnonymousIdentification,
            OrderSolicitationStatus = orderSolicitation.OrderSolicitationStatus,
            OrderItemsList = orderSolicitation.OrderItemsList,
            TotalValue = orderSolicitation.TotalValue,
            InitialDate = orderSolicitation.InitialDate,
            EndDate = orderSolicitation.EndDate,
        };
    }

    public async Task<OrderSolicitationDto?> RemoveOrderItemToOrder(int productId, int quantity, int orderSolicitationId)
    {
        ArgumentNullException.ThrowIfNull(productId);
        ArgumentNullException.ThrowIfNull(orderSolicitationId);

        var orderSolicitation = await OrderSolicitationRepository.GetOrderSolicitationbyId(orderSolicitationId) ?? throw new InvalidOperationException("Solicitação de pedido não encontrada.");

        var product = await ProductRepository.GetProductbyId(productId) ?? throw new InvalidOperationException("Produto não encontrado.");

        if (orderSolicitation.OrderItemsList == null)
        {
            orderSolicitation.OrderItemsList = new List<OrderItem>();
        }

        if (orderSolicitation.OrderSolicitationStatus != OrderSolicitationStatus.InProgress)
        {
            throw new InvalidOperationException("Solicitação de pedido não está em andamento.");
        }

        var orderItem = new OrderItem()
        {
            Product = product,
            Quantity = quantity,
            TotalValue = product.Value * quantity,
        };
        OrderItemValidator orderItemValidator = new OrderItemValidator();
        var resultOrderItem = orderItemValidator.Validate(orderItem);

        if (!resultOrderItem.IsValid)
        {
            throw new InvalidDataException(resultOrderItem.ToString());
        }
        if (orderSolicitation.OrderItemsList == null || !orderSolicitation.OrderItemsList.Any())
        {
            throw new InvalidOperationException("A solicitação de pedido não contém itens para remover.");
        }
        else
        {
            int indexToChange = (int)(orderSolicitation.OrderItemsList?.FindIndex(orderItem => orderItem.Product.Id == productId));

            if (indexToChange == -1)
            {
                throw new InvalidOperationException("O produto não está na solicitação de pedido, não pode ser removido");
            }
            else
            {
                orderSolicitation.OrderItemsList[indexToChange].Quantity -= quantity;

                if (orderSolicitation.OrderItemsList[indexToChange].Quantity == 0)
                {
                    orderSolicitation.OrderItemsList.RemoveAt(indexToChange);
                }
                else
                {
                    resultOrderItem = orderItemValidator.Validate(orderSolicitation.OrderItemsList[indexToChange]);

                    if (!resultOrderItem.IsValid)
                    {
                        throw new InvalidOperationException(resultOrderItem.ToString());
                    }
                }
            }

            orderSolicitation.TotalValue = this.CalculateTotalValue(orderSolicitation);
            orderSolicitation = OrderSolicitationRepository.UpdateOrderSolicitation(orderSolicitation);
        }

        return new OrderSolicitationDto()
        {
            Id = orderSolicitation.Id.ToString(),
            TypeOfIdentification = this.GetTreatedTypeOfIdentification(orderSolicitation.Customer),
            Customer = orderSolicitation.Customer,
            AnonymousIdentification = orderSolicitation.AnonymousIdentification,
            OrderSolicitationStatus = orderSolicitation.OrderSolicitationStatus,
            OrderItemsList = orderSolicitation.OrderItemsList,
            TotalValue = orderSolicitation.TotalValue,
            InitialDate = orderSolicitation.InitialDate,
            EndDate = orderSolicitation.EndDate,
        };
    }
    public decimal CalculateTotalValue(OrderSolicitation orderSolicitation)
    {
        ArgumentNullException.ThrowIfNull(orderSolicitation);

        decimal totalValue = 0;

        if (orderSolicitation.OrderItemsList != null)
        {
            OrderItemValidator orderItemValidator = new OrderItemValidator();
            foreach (var orderItem in orderSolicitation.OrderItemsList)
            {
                var resultOrderItem = orderItemValidator.Validate(orderItem);

                if (!resultOrderItem.IsValid)
                {
                    throw new InvalidDataException(resultOrderItem.ToString());
                }
                totalValue += orderItem.Product.Value * orderItem.Quantity;
            }
        }

        return totalValue;
    }

    public OrderSolicitationDto? InitiateOrderSolicitation()
    {
        var orderSolicitation = new OrderSolicitation()
        {
            InitialDate = DateTime.UtcNow,
            OrderSolicitationStatus = OrderSolicitationStatus.InIdentification,
            TotalValue = 0
        };

        orderSolicitation = OrderSolicitationRepository.RegisterOrderSolicitation(orderSolicitation);
        var orderSolicitationDto = new OrderSolicitationDto()
        {
            Id = orderSolicitation.Id.ToString(),
            InitialDate = orderSolicitation.InitialDate,
            OrderSolicitationStatus = orderSolicitation.OrderSolicitationStatus,
            TotalValue = orderSolicitation.TotalValue
        };

        return orderSolicitationDto;
    }    
}
