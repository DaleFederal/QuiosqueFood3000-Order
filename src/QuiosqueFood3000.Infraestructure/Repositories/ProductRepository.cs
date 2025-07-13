using Microsoft.EntityFrameworkCore;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Persistence;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Infraestructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<Product?> GetProductbyId(int id) => await context.Product.FindAsync(id);
    public async Task<List<Product>?> GetProductsByCategory(ProductCategory productCategory) => await context.Product.Where(p => p.ProductCategory == productCategory).ToListAsync();

    public Product RegisterProduct(Product product)
    {
        context.Product.Add(product);
        context.SaveChanges();
        return product;
    }
    public void RemoveProduct(Product product)
    {
        var existingProduct = context.Product.Find(product.Id);
        if (existingProduct != null)
        {
            context.Entry(existingProduct).State = EntityState.Detached;
            context.Product.Remove(product);
            context.SaveChanges();
        }
    }
    public Product UpdateProduct(Product product)
    {
        var existingProduct = context.Product.Find(product.Id);

        if (existingProduct is null)
        {
            throw new InvalidOperationException("Produto não encontrado.");
        }
        context.Entry(existingProduct).State = EntityState.Detached;

        context.Product.Update(product);
        context.SaveChanges();
        return product;
    }
}
