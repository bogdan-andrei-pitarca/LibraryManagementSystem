using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Models
{
    public class BorrowingRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }  // Add this
        public DateTime? ReturnDate { get; set; }  // Make nullable
        public bool IsReturned { get; set; }

        public BorrowingRecord(int userId, int bookId, DateTime borrowDate)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
            if (bookId <= 0)
                throw new ArgumentException("Book ID must be greater than zero.", nameof(bookId));
            if (borrowDate == default)
                throw new ArgumentException("Borrow date cannot be default value.", nameof(borrowDate));

            UserId = userId;
            BookId = bookId;
            BorrowDate = borrowDate;
            DueDate = borrowDate.AddDays(14);  // Set default due date to 14 days
            IsReturned = false;
        }

        public BorrowingRecord()
        {
            IsReturned = false;
        }

        public bool IsOverdue()
        {
            return !IsReturned && DateTime.Now > DueDate;
        }

        public int GetDaysOverdue()
        {
            if (!IsOverdue())
                return 0;
            return (DateTime.Now - DueDate).Days;
        }

        public void Return()
        {
            if (IsReturned)
                throw new InvalidOperationException("Book has already been returned.");

            ReturnDate = DateTime.Now;
            IsReturned = true;
        }

        public override string ToString()
        {
            return $"Book ID: {BookId}, User ID: {UserId}, Borrowed: {BorrowDate}, Due: {DueDate}, Returned: {ReturnDate?.ToString() ?? "Not returned"}";
        }
    }
}
