using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Api.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetProductById(int id);
    ProductDto RegisterProduct(ProductDto productDto);
    void RemoveProduct(ProductDto productDto);
    ProductDto UpdateProduct(ProductDto productDto);
    Task<List<ProductDto>?> GetProductsByCategory(ProductCategory productCategory);
}