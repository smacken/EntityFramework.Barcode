using System;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFramework.Barcode.Test
{
    public class Context : DbContextWithTriggers
    {
        public DbSet<Product> Products { get; set; }

        // public Context()
        // { }

        public Context(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public Context(IServiceProvider serviceProvider, DbContextOptions options) : base(serviceProvider, options) {}

        // public Context(DbContextOptions<Context> options)
        //     : base(options)
        // { }
    }
}