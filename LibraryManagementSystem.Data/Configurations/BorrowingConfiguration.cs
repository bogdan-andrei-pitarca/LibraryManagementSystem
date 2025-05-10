using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configurations
{
    public class BorrowingConfiguration : IEntityTypeConfiguration<BorrowingRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowingRecord> builder)
        {

            builder.HasKey(br => br.Id);

            builder.Property(br => br.BorrowDate)
                .IsRequired();

            builder.Property(br => br.ReturnDate)
                .IsRequired(false);

            builder.Property(br => br.DueDate)
                .IsRequired();
            
            builder.Property(br => br.IsReturned)
                .IsRequired();


            builder.HasOne<Book>()
                .WithMany()
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(br => br.BookId);
            builder.HasIndex(br => br.UserId);
            builder.HasIndex(br => br.BorrowDate);

        }
    }
    {
    }
}
