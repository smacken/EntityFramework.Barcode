using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFramework.Barcode.Test
{
    public class ScannableTest : IClassFixture<DbFixture>
    {
        private ServiceProvider _serviceProvider;

        public ScannableTest(DbFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public void Scannable_writes_to_database()
        {
            using (var context = _serviceProvider.GetService<Context>())
            {
                var product = new Product{Name="Pencil", BarcodeEntry=Generator.Generate(11)};
                context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = _serviceProvider.GetService<Context>())
            {
                context.Products.Count().Should().Be(1);
                context.Products.Single().Name.Should().Be("Pencil");
            }
        }
        
        [Fact]
        public void Scannable_CanInsertWithBarcode()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            using (var context = _serviceProvider.GetService<Context>())
            {
                var product = new Product{Name="Pencil", BarcodeEntry=Generator.Generate(11)};
                context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = _serviceProvider.GetService<Context>())
            {
                var prod = context.Products.Single(p => p.Name == "Pencil");
                prod.BarcodeImage.Should().NotBeNullOrEmpty();
                prod.Barcode.Should().BeOfType(typeof(BarcodeLib.Barcode));
            }
        }

        [Fact]
        public void Scannable_CanInsertWithBarcodeStringSet()
        {

        }

        [Fact]
        public void Scannable_CanReadBarcodeFromSaved()
        {

        }

        [Fact]
        public void Scannable_CanUpdateWithSetBarcode()
        {

        }
    }
}
