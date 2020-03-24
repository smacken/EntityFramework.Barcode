using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFramework.Barcode.Test
{
    public class ScannableTest
    {
        [Fact]
        public void Scannable_writes_to_database()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            using (var context = new Context(options))
            {
                var product = new Product{Name="Pencil", BarcodeEntry=Generator.Generate(11)};
                context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = new Context(options))
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

            using (var context = new Context(options))
            {
                var product = new Product{Name="Pencil", BarcodeEntry=Generator.Generate(11)};
                context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = new Context(options))
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
