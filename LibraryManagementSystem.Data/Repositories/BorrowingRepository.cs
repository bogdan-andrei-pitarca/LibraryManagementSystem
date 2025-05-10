using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data.Context;

namespace LibraryManagementSystem.Data.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {

        private readonly LibraryDbContext _context;

        public BorrowingRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<BorrowingRecord> AddAsync(BorrowingRecord record)
        {
            await _context.BorrowingRecords.AddAsync(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task DeleteAsync(int id)
        {
            var record = await GetByIdAsync(id);
            if (record != null)
            {
                _context.BorrowingRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetActiveBorrowingsCountAsync(int userId)
        {
            return await _context.BorrowingRecords
                .CountAsync(b => b.UserId == userId && !b.IsReturned);
        }

        public async Task<IEnumerable<BorrowingRecord>> GetAllAsync()
        {
            return await _context.BorrowingRecords.ToListAsync();
        }

        public async Task<IEnumerable<BorrowingRecord>> GetByBookIdAsync(int bookId)
        {
            return await _context.BorrowingRecords
                .Where(b => b.BookId == bookId)
                .ToListAsync();
        }

        public async Task<BorrowingRecord> GetByIdAsync(int id)
        {
            return await _context.BorrowingRecords.FindAsync(id);
        }

        public async Task<IEnumerable<BorrowingRecord>> GetByUserIdAsync(int userId)
        {
            return await _context.BorrowingRecords
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowingRecord>> GetOverdueRecordsAsync()
        {
            return await _context.BorrowingRecords
                .Where(b => !b.IsReturned && b.DueDate < DateTime.Now)
                .ToListAsync();
        }

        public async Task<bool> HasActiveBorrowingAsync(int userId, int bookId)
        {
            return await _context.BorrowingRecords
                .AnyAsync(b => b.UserId == userId &&
                             b.BookId == bookId &&
                             !b.IsReturned);
        }

        public async Task<BorrowingRecord> UpdateAsync(BorrowingRecord record)
        {
            _context.BorrowingRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }
    }
}
