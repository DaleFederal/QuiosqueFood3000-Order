using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Api.Validators;

namespace QuiosqueFood3000.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    /// <summary>
    /// Retorna um cliente pelo CPF
    /// </summary>
    /// <param name="cpf">CPF do cliente</param>
    /// <returns>Retorna o cliente correspondente ao CPF fornecido</returns>
    [HttpGet("getCustomerByCpf/{cpf}")]
    public async Task<IActionResult> CustomerByCpf(string cpf)
    {
        CustomerDto? customer;

        try
        {
            customer = await customerService.GetCustomerByCpf(cpf);

            if (customer is null)
            {
                return NotFound($"Cliente com o CPF: {cpf} não encontrado");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(customer);
    }

    /// <summary>
    /// Registra um novo cliente
    /// </summary>
    /// <param name="customerDto">Dados do cliente a ser registrado</param>
    /// <returns>Retorna os dados do cliente registrado</returns>
    [HttpPost("registerCustomer")]
    public async Task<IActionResult> RegisterCustomer(CustomerDto customerDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerDto.Cpf))
            {
                throw new ArgumentNullException("O CPF deve ser informado");
            }
            var customerSearchForSameCpf = await customerService.GetCustomerByCpf(customerDto.Cpf);

            if (customerSearchForSameCpf != null)
            {
                return BadRequest($"Cliente com o CPF: {customerDto.Cpf} já está cadastrado");
            }
            customerDto = customerService.RegisterCustomer(customerDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(customerDto);
    }

    /// <summary>
    /// Remove um cliente pelo CPF
    /// </summary>
    /// <param name="cpf">CPF do cliente a ser removido</param>
    /// <returns>Retorna uma mensagem de sucesso ou erro</returns>
    [HttpDelete("removeCustomer/{cpf}")]
    public async Task<IActionResult> RemoveCustomer(string cpf)
    {
        try
        {
            CustomerDtoValidator customerDtoValidator = new CustomerDtoValidator();
            var resultCustomerDto = await customerDtoValidator.ValidateAsync(new CustomerDto() { Cpf = cpf }, options =>
            {
                options.IncludeProperties(c => c.Cpf);
            });

            if (!resultCustomerDto.IsValid)
            {
                throw new InvalidDataException(resultCustomerDto.ToString());
            }
            var customerSearchForSameCpf = await customerService.GetCustomerByCpf(cpf);

            if (customerSearchForSameCpf == null)
            {
                return BadRequest($"Cliente com o CPF: {cpf} não está cadastrado");
            }

            customerService.RemoveCustomer(customerSearchForSameCpf);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok("Cliente removido com sucesso");
    }

    /// <summary>
    /// Atualiza os dados de um cliente
    /// </summary>
    /// <param name="customerDto">Dados do cliente a serem atualizados</param>
    /// <returns>Retorna os dados do cliente atualizado</returns>
    [HttpPut("updateCustomer")]
    public async Task<IActionResult> UpdateCustomer(CustomerDto customerDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerDto.Cpf))
            {
                throw new ArgumentNullException("O CPF deve ser informado");
            }
            var customerSearchForSameCpf = await customerService.GetCustomerByCpf(customerDto.Cpf);

            if (customerSearchForSameCpf == null)
            {
                return BadRequest($"Cliente com o CPF: {customerDto.Cpf} não está cadastrado");
            }
            customerDto = customerService.UpdateCustomer(customerDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(customerDto);
    }
}
