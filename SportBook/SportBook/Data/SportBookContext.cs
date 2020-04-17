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

        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Sportbook.db");
    }
}
