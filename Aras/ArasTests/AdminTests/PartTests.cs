using ArasTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Innovator.Client.IOM;
using ArasTests.Mycronic;

namespace ArasTests.AdminTests
{
    
    public class PartTests : ArasTestBase
    {
        public PartTests(ArasFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "Part")]
        public void Admin_ShouldFindAPart() {
            // Act
            Item part = AdminInn.newItem("Part", "get");
            part.setAttribute("maxRecords", "1");
            part = part.apply();

            // Assert
            Assert.False(part.isError());
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Part", "Create")]
        public void Admin_ShouldBeAbleToCreatePart() {
            // Act
            Item part = AdminInn.newItem("Part", "add");
            part = part.apply();

            // Assert
            AssertItem(part).IsNotError();
            
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Part", "Create")]
        public void ShouldBeAbleToCreateDesignPart() {
            // Arrange
            MycronicBase myc = new MycronicBase(AdminInn);
            
            // Act
            Item part = myc.Part.Create.NewDesignedPart();

            // Assert
            AssertItem(part).IsNotError();

        }
    }
}
