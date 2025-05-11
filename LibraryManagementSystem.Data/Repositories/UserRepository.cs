using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Enums;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddAsync(User user)
        {
            if (await ExistsByEmailAsync(user.Email))
            {
                throw new InvalidOperationException($"User with email {user.Email} already exists.");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            // Check if user exists
            var existingUser = await GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with ID {user.Id} not found.");
            }

            // If email is being changed, check if new email already exists
            if (existingUser.Email.ToLower() != user.Email.ToLower() &&
                await ExistsByEmailAsync(user.Email))
            {
                throw new InvalidOperationException($"User with email {user.Email} already exists.");
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {

                var hasActiveBorrowings = await _context.BorrowingRecords
                    .AnyAsync(b => b.UserId == id && !b.IsReturned);

                if (hasActiveBorrowings)
                {
                    throw new InvalidOperationException(
                        "Cannot delete user with active book borrowings. Please return all books first.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<bool> HasActiveBorrowingsAsync(int userId)
        {
            return await _context.BorrowingRecords
                .AnyAsync(b => b.UserId == userId && !b.IsReturned);
        }

        public async Task<int> GetActiveBorrowingsCountAsync(int userId)
        {
            return await _context.BorrowingRecords
                .CountAsync(b => b.UserId == userId && !b.IsReturned);
        }



    }
}
