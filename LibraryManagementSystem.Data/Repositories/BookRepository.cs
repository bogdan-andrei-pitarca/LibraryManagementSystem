using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
                .Where(b => b.Status == Core.Enums.BookStatus.Available && b.Quantity > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchTerm) ||
                           b.Author.Contains(searchTerm) ||
                           b.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Book> AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsBookAvailableAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }

        public async Task<int> GetAvailableQuantityAsync(int id)
        {
            var book = await GetByIdAsync(id);
            return book?.Quantity ?? 0;
        }
    }
}
