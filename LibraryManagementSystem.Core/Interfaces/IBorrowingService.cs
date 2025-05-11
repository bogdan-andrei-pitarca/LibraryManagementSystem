using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Interfaces
{
    public interface IBorrowingService
    {
        Task<BorrowingRecord> GetBorrowingByIdAsync(int id);
        Task<IEnumerable<BorrowingRecord>> GetAllBorrowingsAsync();
        Task<IEnumerable<BorrowingRecord>> GetBorrowingsByUserIdAsync(int userId);
        Task<IEnumerable<BorrowingRecord>> GetBorrowingsByBookIdAsync(int bookId);
        Task<IEnumerable<BorrowingRecord>> GetOverdueBorrowingsAsync();
        Task<BorrowingRecord> CreateBorrowingAsync(int userId, int bookId);
        Task<BorrowingRecord> ReturnBookAsync(int borrowingId);
        Task DeleteBorrowingAsync(int id);
        Task<bool> IsBookBorrowedByUserAsync(int bookId, int userId);
        Task<int> GetActiveBorrowingsCountAsync(int userId);
    }
}
