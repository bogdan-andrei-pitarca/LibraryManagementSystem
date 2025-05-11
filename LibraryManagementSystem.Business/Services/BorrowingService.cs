using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Business.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BorrowingService> _logger;

        public BorrowingService(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            ILogger<BorrowingService> logger)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BorrowingRecord> GetBorrowingByIdAsync(int id)
        {
            return await _borrowingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BorrowingRecord>> GetAllBorrowingsAsync()
        {
            return await _borrowingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<BorrowingRecord>> GetBorrowingsByUserIdAsync(int userId)
        {
            return await _borrowingRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<BorrowingRecord>> GetBorrowingsByBookIdAsync(int bookId)
        {
            return await _borrowingRepository.GetByBookIdAsync(bookId);
        }

        public async Task<IEnumerable<BorrowingRecord>> GetOverdueBorrowingsAsync()
        {
            var borrowings = await _borrowingRepository.GetAllAsync();
            return borrowings.Where(b => b.IsOverdue());
        }

        public async Task<BorrowingRecord> CreateBorrowingAsync(int userId, int bookId)
        {
            // Validate user exists and can borrow
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            if (!user.CanBorrowBook())
                throw new InvalidOperationException("User is not allowed to borrow books");

            // Validate book exists and is available
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new KeyNotFoundException($"Book with ID {bookId} not found");

            if (!await _bookRepository.IsBookAvailableAsync(bookId))
                throw new InvalidOperationException($"Book with ID {bookId} is not available");

            // Check if user has too many active borrowings
            var activeBorrowingsCount = await _borrowingRepository.GetActiveBorrowingsCountAsync(userId);
            if (activeBorrowingsCount >= 5) // Assuming max 5 books per user
                throw new InvalidOperationException("User has reached maximum number of active borrowings");

            // Create new borrowing record
            var borrowing = new BorrowingRecord(userId, bookId, DateTime.UtcNow);
            return await _borrowingRepository.AddAsync(borrowing);
        }

        public async Task<BorrowingRecord> ReturnBookAsync(int borrowingId)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId);
            if (borrowing == null)
                throw new KeyNotFoundException($"Borrowing record with ID {borrowingId} not found");

            if (borrowing.IsReturned)
                throw new InvalidOperationException("Book has already been returned");

            borrowing.Return();
            return await _borrowingRepository.UpdateAsync(borrowing);
        }

        public async Task DeleteBorrowingAsync(int id)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(id);
            if (borrowing == null)
                throw new KeyNotFoundException($"Borrowing record with ID {id} not found");

            await _borrowingRepository.DeleteAsync(borrowing.Id);
        }

        public async Task<bool> IsBookBorrowedByUserAsync(int bookId, int userId)
        {
            return await _borrowingRepository.HasActiveBorrowingAsync(bookId, userId);
        }

        public async Task<int> GetActiveBorrowingsCountAsync(int userId)
        {
            return await _borrowingRepository.GetActiveBorrowingsCountAsync(userId);
        }
    }
}
