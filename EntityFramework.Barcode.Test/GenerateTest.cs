using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace EntityFramework.Barcode.Test
{
    public class GenerateTest
    {

        [Fact]
        public void Generate_random()
        {
            var text = Generator.Generate();
            text.Should().NotBeNullOrEmpty();
            text.Length.Should().Be(10);
        }
    }
}
