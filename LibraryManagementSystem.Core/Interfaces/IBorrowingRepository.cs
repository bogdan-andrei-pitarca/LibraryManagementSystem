using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Interfaces
{
    public interface IBorrowingRepository
    {
        Task<BorrowingRecord> GetByIdAsync(int id);
        Task<IEnumerable<BorrowingRecord>> GetAllAsync();
        Task<IEnumerable<BorrowingRecord>> GetByUserIdAsync(int userId);
        Task<IEnumerable<BorrowingRecord>> GetByBookIdAsync(int bookId);
        Task<IEnumerable<BorrowingRecord>> GetOverdueRecordsAsync();
        Task<BorrowingRecord> AddAsync(BorrowingRecord record);
        Task<BorrowingRecord> UpdateAsync(BorrowingRecord record);
        Task DeleteAsync(int id);
        Task<bool> HasActiveBorrowingAsync(int userId, int bookId);
        Task<int> GetActiveBorrowingsCountAsync(int userId);
    }
}
