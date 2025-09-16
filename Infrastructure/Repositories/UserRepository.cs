using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.AsNoTracking().ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:: {msg}", ex.Message);
                throw;
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            try
            {
                var userById = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                return userById;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:: {msg}", ex.Message);
                throw;
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            try
            {
                var userByUsername = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
                return userByUsername;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:: {msg}", ex.Message);
                throw;
            }
        }

        public async Task AddAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogDebug("Success add product {productName}", user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:: {msg}", ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var rowsAffected = await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
                if (rowsAffected == 0)
                {
                    _logger.LogWarning("User with Uuid {Uuid} not found for deletion.", id);
                }
                else
                {
                    _logger.LogDebug("Successfully deleted User with Uuid {Uuid}.", id);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error:: {msg}", ex.Message);
                throw;
            }
        }
    }
}
