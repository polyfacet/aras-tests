using ArasTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Innovator.Client.IOM;

namespace ArasTests.AdminTests.Part
{
    
    public class PartTests : ArasTestBase
    {
        public PartTests(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        private const string ITEM_TYPE = "Part";

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "Part")]
        [Trait("SmokeTest", "1")]
        public void Admin_ShouldFindAPart() {
            // Act
            Item part = AdminInn.newItem(ITEM_TYPE, "get");
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
            Item part = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            part.setProperty("item_number", itemNumber);
            part.setProperty("name", TEST_NAME);

            part = part.apply();

            // Assert
            AssertItem(part).IsNotError();
            
        }
       
    }
}
