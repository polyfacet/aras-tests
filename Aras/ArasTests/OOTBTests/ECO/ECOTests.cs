using ArasTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Innovator.Client.IOM;


namespace ArasTests.AdminTests.ECO
{
    
    public class ECOTests : ArasTestBase
    {
        public ECOTests(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        private const string ITEM_TYPE = "Express ECO";

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "ECO")]
        [Trait("SmokeTest", "1")]
        public void Admin_ShouldFindAnECO() {
            // Act
            Item part = AdminInn.newItem(ITEM_TYPE, "get");
            part.setAttribute("maxRecords", "1");
            part = part.apply();

            // Assert
            Assert.False(part.isError());
        }

        [Fact]
        [Trait("Domain", "ECO")]
        [Trait("ECO", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_ShouldBeAbleToCreateAnECO() {
            // Act
            Item part = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            part.setProperty("item_number", itemNumber);
            part.setProperty("title", TEST_NAME);

            part = part.apply();

            // Assert
            AssertItem(part).IsNotError();
            
        }

        
    }
}
