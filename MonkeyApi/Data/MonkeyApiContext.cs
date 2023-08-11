using Microsoft.EntityFrameworkCore;
using MonkeyApi.Model;

namespace MonkeyApi.Data
{
    public class MonkeyApiContext : DbContext
    {
        public MonkeyApiContext (DbContextOptions<MonkeyApiContext> options)
            : base(options)
        {
        }

        public DbSet<Monkey> Monkey { get; set; } = default!;
    }
}
