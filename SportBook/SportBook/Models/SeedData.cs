using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportBook.Data;

namespace SportBook.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Data.SportBookContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SportBookContext>>()))
            {
                // Look for any movies.
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.AddRange(
                    new User
                    {
                        Firstname = "Arnas",
                        Lastname = "Kvedaras",
                        Username = "Worll",
                        BirthDate = DateTime.Now,
                        Password = "stronkpassword",
                        Email = "emailas1@gmail.com"
                    },
                    new User
                    {
                        Firstname = "Robertas",
                        Lastname = "Strazdauskas",
                        Username = "Elevengarden",
                        BirthDate = DateTime.Now,
                        Password = "stronkpasswordas",
                        Email = "emailas2@gmail.com"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
