using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class OrderController(IOrderService orderService) : ControllerBase
{
    /// <summary>
    /// Retorna um pedido pelo ID
    /// </summary>
    /// <param name="id">ID do pedido</param>
    /// <returns>Retorna o pedido correspondente ao ID fornecido</returns>
    [HttpGet("getOrderById/{id}")]
    public async Task<IActionResult> OrderById(int id)
    {
        OrderDto? orderDto;

        try
        {
            orderDto = await orderService.GetOrderById(id);

            if (orderDto is null)
            {
                return NotFound($"Pedido com o id: {id} não encontrado");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(orderDto);
    }

    /// <summary>
    /// Retorna todos os pedidos
    /// </summary>
    /// <returns>Retorna uma lista de todos os pedidos</returns>
    [HttpGet("getOrders/")]
    public async Task<IActionResult> GetOrders()
    {
        List<OrderDto>? ordersDto;
        try
        {
            ordersDto = await orderService.GetOrders();

            if (ordersDto is null)
            {
                return NotFound($"Não foram encontrados pedidos");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(ordersDto);
    }

    /// <summary>
    /// Retorna pedidos pelo status
    /// </summary>
    /// <param name="orderStatus">Status do pedido</param>
    /// <returns>Retorna uma lista de pedidos com o status fornecido</returns>
    [HttpGet("getOrdersByStatus/{orderStatus}")]
    public async Task<IActionResult> GetOrdersByStatus(OrderStatus orderStatus)
    {
        List<OrderDto>? orderDto;

        try
        {
            orderDto = await orderService.GetOrdersByStatus(orderStatus);

            if (orderDto is null || orderDto.Count == 0)
            {
                return NotFound($"Nenhum pedido com o status {orderStatus} foi encontrado");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(orderDto);
    }
    
    /// <summary>
    /// Retorna o status de um pagamento de um pedido pelo id
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    [HttpGet("getPaymentOrderStatus/{orderId}")]
    public async Task<IActionResult> GetPaymentOrdersStatusById(int orderId)
    {
        OrderDto? orderDto;
        try
        {
            orderDto = await orderService.GetOrderById(orderId);

            if (orderDto is null)
            {
                return NotFound($"Nenhum Pedido encontrado com o id: {orderId}");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(orderDto.PaymentStatus);
    }
    
    /// <summary>
    /// Retorna os pedidos que estão atualmente ativos
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    [HttpGet("getCurrentOrders")]
    public async Task<IActionResult> GetCurrentOrders()
    {
        List<OrderDto>? orderDto;
        try
        {
            orderDto = await orderService.GetCurrentOrders();

            if (orderDto is null)
            {
                return NotFound($"Nenhum Pedido encontrado");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(orderDto);
    }
    
    /// <summary>
    /// Troca o status do pedido para o status desejado
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="newOrderStatus"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    [HttpGet("changeStatusOrder/{orderId}")]
    public async Task<IActionResult> OrderChangeStatus(int orderId, [FromQuery] OrderStatus newOrderStatus)
    {
        try
        {
           await orderService.ChangeOrderStatus(orderId, newOrderStatus);
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok($"Pedido de identificação: {orderId} Status novo: {newOrderStatus}");
    }
}