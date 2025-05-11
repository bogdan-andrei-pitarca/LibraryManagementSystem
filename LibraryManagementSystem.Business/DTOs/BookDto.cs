﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Enums;

namespace LibraryManagementSystem.Business.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }

        public BookStatus Status { get; set; }
    }

    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
        public BookStatus Status { get; set; }

    }
}
