using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetProductbyId(int id);
    Task<List<Product>?> GetProductsByCategory(ProductCategory productCategory);
    Product RegisterProduct(Product product);
    void RemoveProduct(Product product);
    Product UpdateProduct(Product product);
}