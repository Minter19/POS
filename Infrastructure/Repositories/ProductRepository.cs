using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTO.Product;
using System;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            var productResponses = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
            }).ToList();

            return productResponses;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var productById = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return productById;
        }

        public async Task AddAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                _logger.LogDebug("Success add product {productName}", product.Name);
            }catch(Exception ex)
            {
                _logger.LogError("Error:: {msg}", ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var rowsAffected = await _context.Products.Where(x => x.Id == id).ExecuteDeleteAsync();
                if (rowsAffected == 0)
                {
                    _logger.LogWarning("Product with Uuid {Uuid} not found for deletion.", id);
                }
                else
                {
                    _logger.LogDebug("Successfully deleted product with Uuid {Uuid}.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:: {msg}", ex.Message);
                throw;
            }
        }
    }
}
