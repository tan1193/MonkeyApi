using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public DbSet<MonkeyApi.Model.Monkey> Monkey { get; set; } = default!;
    }
}
