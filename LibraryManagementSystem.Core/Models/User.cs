using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Enums;

namespace LibraryManagementSystem.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserRole Role { get; set; }
        public User(string name, string email, DateTime dateOfBirth, UserRole userRole)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (dateOfBirth == default)
                throw new ArgumentException("Date of Birth cannot be default value.", nameof(dateOfBirth));
            if (!Enum.IsDefined(typeof(UserRole), userRole))
                throw new ArgumentException("Invalid user role.", nameof(userRole));
            Name = name;
            Email = email;
            DateOfBirth = dateOfBirth;
            Role = userRole;
        }

        public User()
        {
            Role = UserRole.User;
        }

        public User(int id, string name, string email, DateTime dateOfBirth, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (!IsValidEmail(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (dateOfBirth == default)
                throw new ArgumentException("Date of Birth cannot be default value.", nameof(dateOfBirth));
            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("Invalid user role.", nameof(role));

            Id = id;
            Name = name;
            Email = email;
            DateOfBirth = dateOfBirth;
            Role = role;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool CanBorrowBook()
        {
            return Role == UserRole.User;
        }

        public bool CanManageBooks()
        {
            return Role == UserRole.Admin;
        }

        public override string ToString()
        {
            return $"{Name} ({Role})";
        }
    }
}
