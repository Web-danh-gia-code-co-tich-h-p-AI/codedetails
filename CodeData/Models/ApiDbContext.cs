using Microsoft.EntityFrameworkCore;

namespace CodeData.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Coders> Coders { get; set; }
    }
}
