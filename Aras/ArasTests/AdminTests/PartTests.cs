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
        [Trait("SmokeTest", "1")]
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
        [Trait("Business", "OOTB")]
        public void Admin_ShouldBeAbleToCreatePart() {
            // Act
            Item part = AdminInn.newItem("Part", "add");
            string itemNumber = Guid.NewGuid().ToString().Substring(0,8);
            part.setProperty("item_number", itemNumber);
            part.setProperty("name", "AutoTest");

            part = part.apply();

            // Assert
            AssertItem(part).IsNotError();
            
        }

        
    }
}
