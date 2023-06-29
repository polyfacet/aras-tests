using Xunit.Abstractions;
using Innovator.Client.IOM;
using Aras.Core.Tests;
using Aras.OOTB.Tests.Fixture;

namespace Aras.OOTB.Tests.BusinessObjectTests.ECO
{

    public class ECOTests : OOTBTest
    {
        public ECOTests(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        private const string ITEM_TYPE = "Express ECO";

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "ECO")]
        [Trait("SmokeTest", "1")]
        public void Admin_ShouldFindAnECO()
        {
            // Act
            Item eco = AdminInn.newItem(ITEM_TYPE, "get");
            eco.setAttribute("maxRecords", "1");
            eco = eco.apply();

            // Assert
            Assert.False(eco.isError());
        }

        [Fact]
        [Trait("Domain", "ECO")]
        [Trait("ECO", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_ShouldBeAbleToCreateAnECO()
        {
            // Act
            Item eco = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            eco.setProperty("item_number", itemNumber);
            eco.setProperty("title", TEST_NAME);
            eco = eco.apply();

            // Assert
            AssertItem.IsNotError(eco);

        }

        [Fact]
        [Trait("Domain", "ECO")]
        [Trait("ECO", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_ShouldNotBeAbleToCreateAnECO_WhenTitleIsNotSet()
        {
            // Arrange/Act
            Item eco = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            eco.setProperty("item_number", itemNumber);
            eco = eco.apply();

            // Assert
            AssertItem.IsError(eco);
        }

    }
}
