using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Retorna um produto pelo ID
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <returns>Retorna o produto correspondente ao ID fornecido</returns>
    [HttpGet("getProductById/{id}")]
    public async Task<IActionResult> ProductById(int id)
    {
        ProductDto? productDto;

        try
        {
            productDto = await productService.GetProductById(id);

            if (productDto is null)
            {
                return NotFound($"Produto com o id: {id} não encontrado");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(productDto);
    }

    /// <summary>
    /// Registra um novo produto
    /// </summary>
    /// <param name="productDto">Dados do produto a ser registrado</param>
    /// <returns>Retorna os dados do produto registrado</returns>
    [HttpPost("registerProduct")]
    public IActionResult RegisterProduct(ProductDto productDto)
    {
        try
        {
            productDto = productService.RegisterProduct(productDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(productDto);
    }

    /// <summary>
    /// Remove um produto pelo ID
    /// </summary>
    /// <param name="id">ID do produto a ser removido</param>
    /// <returns>Retorna uma mensagem de sucesso ou erro</returns>
    [HttpDelete("removeProduct/{id}")]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        try
        {
            var productSearchForSameId = await productService.GetProductById(id);

            if (productSearchForSameId == null)
            {
                return BadRequest($"Produto com o Id: {id} não está cadastrado");
            }

            productService.RemoveProduct(productSearchForSameId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok("Produto removido com sucesso");
    }

    /// <summary>
    /// Atualiza os dados de um produto
    /// </summary>
    /// <param name="productDto">Dados do produto a serem atualizados</param>
    /// <returns>Retorna os dados do produto atualizado</returns>
    [HttpPut("updateProduct")]
    public async Task<IActionResult> UpdateProduct(ProductDto productDto)
    {
        try
        {
            if (string.IsNullOrEmpty(productDto.Id))
            {
                return BadRequest("Id do produto não pode ser nulo ou vazio");
            }

            var productSearchForSameCpf = await productService.GetProductById(int.Parse(productDto.Id));

            if (productSearchForSameCpf == null)
            {
                return BadRequest($"Produto com o Id: {productDto.Id} não está cadastrado");
            }
            productDto = productService.UpdateProduct(productDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(productDto);
    }

    /// <summary>
    /// Retorna produtos pela categoria
    /// </summary>
    /// <param name="productCategory">Categoria do produto</param>
    /// <returns>Retorna uma lista de produtos com a categoria fornecida</returns>
    [HttpGet("getProductsByCategory/{productCategory}")]
    public async Task<IActionResult> GetProductsByCategory(ProductCategory productCategory)
    {
        List<ProductDto>? products;
        try
        {
            products = await productService.GetProductsByCategory(productCategory);

            if (products is null)
            {
                return NotFound($"Não foram encontrados produtos com a categoria: {productCategory}");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(ex.Message);
        }
        return Ok(products);
    }
}