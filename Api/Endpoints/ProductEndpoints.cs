using Application.Services;
using Domain.Entities;
using Shared.DTO.Product;

namespace Api.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var productGroup = app.MapGroup("/api/products").WithTags("Products");

            productGroup.MapGet("/", async (IProductService productService) =>
            {
                var products = await productService.GetAllProducts();
                return Results.Ok(products);
            });

            productGroup.MapGet("/{id}", async (IProductService productService, Guid id) =>
            {
                var product = await productService.GetProductById(id);
                if (product == null)
                {
                    return Results.NotFound(new
                    {
                        Result = $"Product dengan Id ${id} tidak ditemukan."
                    });
                }

                return Results.Ok(product);
            });

            productGroup.MapPost("/", async(IProductService productService, ProductRequest newProduct) =>
            {
                var product = new Product {
                    Id = Guid.NewGuid(),
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price
                };
                await productService.AddProduct(product);
                return Results.Created($"/api/products/{newProduct.Name}", product);
            });

            productGroup.MapDelete("/{id}", async (IProductService productService, Guid id) =>
            {
                await productService.DeleteProduct(id);
                return Results.Ok();
            });
        }

    }
}
