using Microsoft.EntityFrameworkCore;
using SportBook.Models;


namespace SportBook.Data
{
    public class SportBookContext : DbContext
    {
        public SportBookContext(DbContextOptions<SportBookContext> options)
           : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
