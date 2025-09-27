using Application.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using Shared.Common.Error;
using Shared.DTO.Product;
using System.ComponentModel.DataAnnotations;

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

            productGroup.MapGet("/{id}", async static (IProductService productService, string id) =>
            {
                //// Handle invalid GUID format first
                if (!Guid.TryParse(id, out var guid))
                {
                    return Results.BadRequest(new
                    {
                        Title = "Validation Error",
                        Status = 400,
                        Detail = $"The provided 'id' is not a valid GUID format."
                    });
                }

                try
                {
                    var product = await productService.GetProductById(guid);
                    if (product == null)
                    {
                        // Correctly return 404 Not Found
                        return Results.NotFound(new
                        {
                            Title = "Not Found",
                            Status = 404,
                            Detail = $"Product with Id '{id}' not found."
                        });
                    }

                    return Results.Ok(product);
                }
                catch (ValidationException ex)
                {
                    // Handle validation errors with a 400 Bad Request
                    return Results.BadRequest(new
                    {
                        Title = "Validation Error",
                        Status = 400,
                        Detail = ex.Message
                    });
                }
                catch (DbUpdateException)
                {
                    // Handle database errors with a 500 Internal Server Error
                    // It's safer not to expose the internal exception message
                    return Results.Problem(
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Database Error",
                        detail: "An internal server error occurred while retrieving the product."
                    );
                }
                catch (Exception)
                {
                    // Catch all other unexpected errors with a generic 500
                    return Results.Problem(
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "An unexpected error occurred",
                        detail: "An internal server error occurred. Please try again later."
                    );
                }
            });

            productGroup.MapPost("/", async (IProductService productService, ProductRequest newProduct) =>
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    Stock = newProduct.Stock
                };

                try
                {
                    await productService.AddProduct(product);
                    return Results.Created($"/api/products/{product.Id}", product);
                }
                catch (ValidationException ex)
                {
                    var errorReponse = new ErrorBodyResponse
                    {
                        Title = "Validation Error",
                        StatusCode = 400,
                        Detail = ex.Message
                    };

                    return Results.BadRequest(errorReponse);
                }
                catch (DbUpdateException ex)
                {
                    var errorReponse = new ErrorBodyResponse
                    {
                        Title = "Database Error",
                        StatusCode = 409,
                        Detail = ex.Message
                    };

                    return Results.Conflict(errorReponse);
                }
                catch (Exception ex)
                {
                    var errorReponse = new ErrorBodyResponse
                    {
                        Title = "An unexpected error occurred",
                        StatusCode = 500,
                        Detail = ex.Message
                    };

                    return Results.InternalServerError(errorReponse);
                }
            });

            productGroup.MapDelete("/{id}", async (IProductService productService, string id) =>
            {
                // Handle invalid GUID format first
                if (!Guid.TryParse(id, out var guid))
                {
                    return Results.BadRequest(new
                    {
                        Title = "Validation Error",
                        Status = 400,
                        Detail = $"The provided 'id' is not a valid GUID format."
                    });
                }

                try
                {
                    await productService.DeleteProduct(guid);
                    return Results.NoContent();
                }
                catch (ValidationException ex)
                {
                    var errorReponse = new ErrorBodyResponse
                    {
                        Title = "Validation Error",
                        StatusCode = 400,
                        Detail = ex.Message
                    };

                    return Results.BadRequest(errorReponse);
                }
                catch (DbUpdateException ex)
                {
                    var errorReponse = new ErrorBodyResponse
                    {
                        Title = "Database Error",
                        StatusCode = 409,
                        Detail = ex.Message
                    };

                    return Results.Conflict(errorReponse);
                }
                catch (Exception ex)
                {
                    var errorReponse = new ErrorBodyResponse
                    {
                        Title = "An unexpected error occurred",
                        StatusCode = 500,
                        Detail = ex.Message
                    };

                    return Results.InternalServerError(errorReponse);
                }
                
            });
        }

    }
}
