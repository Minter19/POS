using Domain.Entities;
using Domain.Interfaces;
using Shared.DTO.User;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetUserById(Guid id);
        Task AddUser(User user);
        Task DeleteUser(Guid id);
        Task<object?> Login(LoginRequest loginRequest);

    }

    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<User>> GetAllUsers() {
            return _userRepository.GetAllAsync();
        }

        public Task<User?> GetUserById(Guid id) {
            return _userRepository.GetByIdAsync(id);
        }
        public Task AddUser(User user)
        {
            return _userRepository.AddAsync(user);
        }
        public Task DeleteUser(Guid id)
        {
            return _userRepository.DeleteAsync(id);
        }

        public async Task<object?> Login(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetByUsernameAsync(loginRequest.Username);
            if (user != null && user.PasswordHash != null)
            {
                var verifyPassword = PasswordHelper.VerifyPassword(loginRequest.Password, user.PasswordHash);
                if (verifyPassword)
                {
                    return new
                    {
                        Result = "Login Berhasil"
                    };
                }
            }

            return new
            {
                Result = "Login Gagal"
            };
        }
    }
}
