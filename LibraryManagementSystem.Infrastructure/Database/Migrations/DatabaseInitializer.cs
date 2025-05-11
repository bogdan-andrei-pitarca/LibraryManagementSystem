using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Infrastructure.Database.Migrations
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            try
            {
                context.Database.Migrate();
                SeedData(context);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while initializing the database.", ex);
            }
        }

        private static void SeedData(LibraryDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new Core.Models.User(
                    "Admin",
                    "admin@library.com",
                    new DateTime(1990, 1, 1),
                    Core.Enums.UserRole.Admin
                ));
                context.SaveChanges();
            }
        }
    }
}
