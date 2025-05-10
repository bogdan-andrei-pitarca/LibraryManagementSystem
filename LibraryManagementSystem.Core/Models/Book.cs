using LibraryManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
        public BookStatus Status { get; set; }

        public Book()
        {
            Status = BookStatus.Available;
            Quantity = 0;
        }

        public Book(string name, string title, string author, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be null or empty.", nameof(author));
            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

            Name = name;
            Title = title;
            Author = author;
            Quantity = quantity;
            Status = quantity > 0 ? BookStatus.Available : BookStatus.Borrowed;
        }

        public bool IsAvailable()
        {
            return Status == BookStatus.Available && Quantity > 0;
        }

        public void BorrowBook()
        {
            if (Status == BookStatus.Available && Quantity > 0)
            {
                Quantity--;
                if (Quantity == 0)
                {
                    Status = BookStatus.Borrowed;
                }
            }
            else
            {
                throw new InvalidOperationException("Book is not available for borrowing.");
            }
        }

        public void ReturnBook()
        {
            if (Status == BookStatus.Borrowed)
            {
                Quantity++;
                if (Quantity > 0)
                {
                    Status = BookStatus.Available;
                }
            }
            else
            {
                throw new InvalidOperationException("Book is not borrowed.");
            }
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity cannot be negative.");
            Quantity = newQuantity;
            Status = newQuantity > 0 ? BookStatus.Available : BookStatus.Borrowed;
        }

        public override string ToString()
        {
            return $"Book: {Name}, Title: {Title}, Author: {Author}, Quantity: {Quantity}, Status: {Status}";
        }

    }

}
