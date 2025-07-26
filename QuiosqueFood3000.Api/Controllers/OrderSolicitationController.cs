using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;

namespace QuiosqueFood3000.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class OrderSolicitationController(IOrderSolicitationService OrderSolicitationService, ICustomerService CustomerService) : ControllerBase
{
    /// <summary>
    /// Retorna uma solicitação de pedido pelo ID
    /// </summary>
    /// <param name="id">ID da solicitação de pedido</param>
    /// <returns>Retorna a solicitação de pedido correspondente ao ID fornecido</returns>
    [HttpGet("getOrderSolicitationById/{id}")]
    public async Task<IActionResult> GetOrderSolicitationById(int id)
    {
        OrderSolicitationDto? orderSolicitation;

        try
        {
            orderSolicitation = await OrderSolicitationService.GetOrderSolicitationById(id);

            if (orderSolicitation is null)
            {
                return NotFound($"Solicitação com o id: {id} não encontrada");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(orderSolicitation);
    }

    /// <summary>
    /// Associa um cliente a uma solicitação de pedido
    /// </summary>
    /// <param name="orderSolicitationId">ID da solicitação de pedido</param>
    /// <param name="customerCpf">CPF do cliente</param>
    /// <returns>Retorna a solicitação de pedido atualizada</returns>
    [HttpPost("associateCustomerToOrderSolicitation")]
    public async Task<IActionResult> AssociateCustomerToOrderSolicitation(int orderSolicitationId, string customerCpf)
    {
        OrderSolicitationDto? orderSolicitationDto;
        try
        {
            orderSolicitationDto = await OrderSolicitationService.GetOrderSolicitationById(orderSolicitationId);
            if (orderSolicitationDto is null)
            {
                return BadRequest("Solicitação de pedido não encontrada.");
            }

            CustomerDto? CustomerDto = await CustomerService.GetCustomerByCpf(customerCpf);
            if (CustomerDto is null)
            {
                return BadRequest("Cliente não encontrado.");
            }

            orderSolicitationDto = OrderSolicitationService.AssociateCustomerToOrderSolicitation(CustomerDto, orderSolicitationDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }

        return Ok(orderSolicitationDto);
    }

    /// <summary>
    /// Associa uma identificação anônima a uma solicitação de pedido
    /// </summary>
    /// <param name="id">ID da solicitação de pedido</param>
    /// <returns>Retorna a solicitação de pedido atualizada</returns>
    [HttpPost("associateAnnonymousIdentificationToOrderSolicitation")]
    public async Task<IActionResult> AssociateAnnonymousIdentificationToOrderSolicitation(int id)
    {
        OrderSolicitationDto? orderSolicitationDto;
        try
        {
            orderSolicitationDto = await OrderSolicitationService.GetOrderSolicitationById(id);
            if (orderSolicitationDto is null)
            {
                return BadRequest("Solicitação de pedido não encontrada.");
            }

            orderSolicitationDto = await OrderSolicitationService.AssociateAnnonymousIdentificationToOrderSolicitation(orderSolicitationDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(orderSolicitationDto);
    }

    /// <summary>
    /// Adiciona um item ao pedido
    /// </summary>
    /// <param name="productId">ID do produto</param>
    /// <param name="quantity">Quantidade do produto</param>
    /// <param name="orderSolicitationId">ID da solicitação de pedido</param>
    /// <param name="observations">Observações sobre o item</param>
    /// <returns>Retorna a solicitação de pedido atualizada</returns>
    [HttpPost("addOrderItemToOrder")]
    public async Task<IActionResult> AddOrderItemToOrder(int productId, int quantity, int orderSolicitationId, string? observations)
    {
        OrderSolicitationDto? orderSolicitation;
        try
        {
            orderSolicitation = await OrderSolicitationService.AddOrderItemToOrder(productId, quantity, orderSolicitationId, observations);

            if (orderSolicitation is null)
            {
                return BadRequest("Não foi possível associar o cliente à solicitação de pedido.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(orderSolicitation);
    }

    /// <summary>
    /// Remove um item do pedido
    /// </summary>
    /// <param name="productId">ID do produto</param>
    /// <param name="quantity">Quantidade do produto</param>
    /// <param name="orderSolicitationId">ID da solicitação de pedido</param>
    /// <returns>Retorna a solicitação de pedido atualizada</returns>
    [HttpPost("removeOrderItemToOrder")]
    public async Task<IActionResult> RemoveOrderItemToOrder(int productId, int quantity, int orderSolicitationId)
    {
        OrderSolicitationDto? orderSolicitation;
        try
        {
            orderSolicitation = await OrderSolicitationService.RemoveOrderItemToOrder(productId, quantity, orderSolicitationId);

            if (orderSolicitation is null)
            {
                return BadRequest("Não foi possível associar o cliente à solicitação de pedido.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }

        return Ok(orderSolicitation);
    }

    /// <summary>
    /// Inicia uma nova solicitação de pedido
    /// </summary>
    /// <returns>Retorna a solicitação de pedido iniciada</returns>
    [HttpPost("initiateOrderSolicitation")]
    public IActionResult InitiateOrderSolicitation()
    {
        OrderSolicitationDto? orderSolicitationDto;
        try
        {
            orderSolicitationDto = OrderSolicitationService.InitiateOrderSolicitation();

            if (orderSolicitationDto is null)
            {
                return BadRequest("Erro ao iniciar a solicitação de pedido.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(orderSolicitationDto);
    }
}