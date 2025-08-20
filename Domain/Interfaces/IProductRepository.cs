using Domain.Entities;
using Shared.DTO.Product;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<ProductResponse>> GetAllAsync();
        Task AddAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
