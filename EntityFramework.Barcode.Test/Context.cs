using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Barcode.Test
{
    public class Context : DbContextWithTriggers
    {
        public DbSet<Product> Products { get; set; }

        public Context()
        { }

        public Context(DbContextOptions<Context> options)
            : base(options)
        { }
    }
}