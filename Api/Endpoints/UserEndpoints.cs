using Application.Services;
using Domain.Entities;
using Shared.DTO.User;
using Shared.Helper;

namespace Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var userGroup = app.MapGroup("/api/users").WithTags("Users");

            userGroup.MapGet("/", async(IUserService userService) =>
            {
                var users = await userService.GetAllUsers();
                return Results.Ok(users);
            });

            userGroup.MapGet("/{id}", async (IUserService userService, Guid id) =>
            {
                var user = await userService.GetUserById(id);
                if(user == null)
                {
                    return Results.NotFound(new { 
                        Results = $"User dengan Id ${id} tidak ditemukan."
                    });
                }
                return Results.Ok(user);
            });

            userGroup.MapPost("/", async (IUserService userService, User user) =>
            {
                user.Id = new Guid();
                user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
                await userService.AddUser(user);
                return Results.Created();
            });

            userGroup.MapPost("/login", async (IUserService userService, LoginRequest loginRequest) =>
            {
                var result = await userService.Login(loginRequest: loginRequest);
                return Results.Ok(result);
            });

            userGroup.MapDelete("/{id}", async (IUserService userService, Guid id) =>
            {
                await userService.DeleteUser(id);
                return Results.Ok();
            });

            
        }
    }
}
