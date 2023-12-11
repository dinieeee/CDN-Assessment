using Microsoft.EntityFrameworkCore;

namespace Assessment.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) :base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
