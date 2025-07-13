using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Api.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<ProductDto?> GetProductById(int id)
    {
        var product = await productRepository.GetProductbyId(id);

        return product == null
            ? null
            : new ProductDto()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Available = product.Available,
                ProductCategory = product.ProductCategory,
                Value = product.Value,
                Description = product.Description,
                Image = product.Image
            };
    }
    public ProductDto RegisterProduct(ProductDto productDto)
    {
        ArgumentNullException.ThrowIfNull(productDto);

        ProductDtoValidator productDtoValidator = new ProductDtoValidator();
        var resultProductDto = productDtoValidator.Validate(productDto);

        if (!resultProductDto.IsValid)
        {
            throw new InvalidDataException(resultProductDto.ToString());
        }

        Product product = new Product()
        {
            Name = productDto.Name,
            Available = productDto.Available ?? false,
            ProductCategory = productDto.ProductCategory ?? throw new ArgumentNullException(nameof(productDto.ProductCategory)),
            Value = productDto.Value ?? throw new ArgumentNullException(nameof(productDto.Value)),
            Description = productDto.Description,
            Image = productDto.Image
        };

        ProductValidator productValidator = new ProductValidator();
        var resultProduct = productValidator.Validate(product);

        if (!resultProduct.IsValid)
        {
            throw new InvalidDataException(resultProduct.ToString());
        }

        product = productRepository.RegisterProduct(product);
        return new ProductDto()
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Available = product.Available,
            ProductCategory = product.ProductCategory,
            Value = product.Value,
            Description = product.Description,
            Image = product.Image
        };
    }
    public void RemoveProduct(ProductDto productDto)
    {
        ArgumentNullException.ThrowIfNull(productDto);
        if (string.IsNullOrWhiteSpace(productDto.Id))
        {
            throw new ArgumentNullException("O id do produto deve ser informado");
        }
        Product product = new Product()
        {
            Id = int.Parse(productDto.Id)
        };
        productRepository.RemoveProduct(product);
    }
    public ProductDto UpdateProduct(ProductDto productDto)
    {

        if (productDto == null)
        {
            throw new ArgumentNullException("O produto deve ser informado");
        }

        ProductDtoValidator productDtoValidator = new ProductDtoValidator();
        var resultProductDto = productDtoValidator.Validate(productDto);

        if (!resultProductDto.IsValid)
        {
            throw new InvalidDataException(resultProductDto.ToString());
        }

        Product product = new Product()
        {
            Id = int.Parse(productDto.Id ?? throw new ArgumentNullException(nameof(productDto.Id))),
            Name = productDto.Name,
            Available = productDto.Available ?? false,
            ProductCategory = productDto.ProductCategory ?? throw new ArgumentNullException(nameof(productDto.ProductCategory)),
            Value = productDto.Value ?? throw new ArgumentNullException(nameof(productDto.Value)),
            Description = productDto.Description,
            Image = productDto.Image
        };
        ProductValidator productValidator = new ProductValidator();
        var resultProduct = productValidator.Validate(product);

        if (!resultProduct.IsValid)
        {
            throw new InvalidDataException(resultProduct.ToString());
        }
        product = productRepository.UpdateProduct(product);
        return new ProductDto()
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Available = product.Available,
            ProductCategory = product.ProductCategory,
            Value = product.Value,
            Description = product.Description,
            Image = product.Image
        };
    }
    public async Task<List<ProductDto>?> GetProductsByCategory(ProductCategory productCategory)
    {
        var products = await productRepository.GetProductsByCategory(productCategory);

        if (products == null)
        {
            throw new ArgumentNullException();
        }

        return products.Select(product => new ProductDto
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Available = product.Available,
            ProductCategory = product.ProductCategory,
            Value = product.Value,
            Description = product.Description,
            Image = product.Image
        }).ToList();
    }
}
