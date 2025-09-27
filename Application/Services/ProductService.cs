using Domain.Entities;
using Domain.Interfaces;
using Shared.DTO.Product;
using System.ComponentModel.DataAnnotations;

namespace Application.Services
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProducts();
        Task<Product?> GetProductById(Guid id);
        Task AddProduct(Product product);
        Task DeleteProduct(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<List<ProductResponse>> GetAllProducts()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product?> GetProductById(Guid id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public Task AddProduct(Product product)
        {
            var context = new ValidationContext(product);
            Validator.ValidateObject(product, context, validateAllProperties: true);

            return _productRepository.AddAsync(product);
        }

        public Task DeleteProduct(Guid id)
        {
            return _productRepository.DeleteAsync(id);
        }
    }
}
