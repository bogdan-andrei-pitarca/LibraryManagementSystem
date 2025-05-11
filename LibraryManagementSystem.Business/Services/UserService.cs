using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.Core.Enums;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (await _userRepository.ExistsByEmailAsync(user.Email))
                throw new InvalidOperationException($"User with email {user.Email} already exists");

            return await _userRepository.AddAsync(user);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {user.Id} not found");

            if (await _userRepository.ExistsByEmailAsync(user.Email) &&
                existingUser.Email != user.Email)
                throw new InvalidOperationException($"User with email {user.Email} already exists");

            return await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            if (await _userRepository.HasActiveBorrowingsAsync(id))
                throw new InvalidOperationException("Cannot delete user with active borrowings");

            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> CanBorrowBookAsync(int userId)
        {
            // Check if user exists and is active
            if (!await IsUserActiveAsync(userId))
                return false;

            // Check if user has reached maximum borrowings
            var activeBorrowingsCount = await _userRepository.GetActiveBorrowingsCountAsync(userId);
            return activeBorrowingsCount < 5; // Assuming max 5 books per user
        }

        public async Task<bool> IsUserActiveAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            // Since we don't have an IsActive property, we'll consider a user active if they exist
            // and have a valid role
            return user != null && Enum.IsDefined(typeof(UserRole), user.Role);
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            // Since we don't have password authentication in the current model,
            // we'll just check if the user exists with the given email
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

    }
}
