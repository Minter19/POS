using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Infrastructure.Data.Seed
{
    public class ProductExampleSeed
    {
        public static List<Product> GetProductsExample()
        {
            return [
                new(){
                    Id = Guid.Parse("2c7b5d3a-0e9f-4b1c-8a2d-1f6e9c0b8a7f"),
                    Name = "Product One",
                    Description = "Description of product one",
                    Stock = 1,
                    Price = 1000
                },
                new(){
                    Id = Guid.Parse("3d7b5d3a-0e9f-4b1c-8a2d-1f6e9c0b8a6f"),
                    Name = "Product Two",
                    Description = "Description of product tow",
                    Stock = 2,
                    Price = 2000
                },
                new(){
                    Id = Guid.Parse("1aeb5d3a-0e9f-4b1c-8a2d-1f6e9c0b8a9a"),
                    Name = "Product Three",
                    Description = "Description of product three",
                    Stock = 3,
                    Price = 3000
                }
            ];
        }
    }
}