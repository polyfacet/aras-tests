using Innovator.Client.IOM;
using Aras.Core.Tests;
using Aras.Core.Tests.Arranging;
using Aras.Core.Tests.ArasExtensions;
using Aras.Core.Tests.Setup;
using Microsoft.Extensions.Logging;

namespace Aras.OOTB.Tests.BusinessObjectTests.ECO
{

    public class ECOTests
    {
        private static readonly ILogger Logger = OOTBTestBase.CreateLogger();
        ArasCollectionFixture fixture;
        Innovator.Client.IOM.Innovator AdminInn;
        Innovator.Client.IOM.Innovator CMInn;
        OOTBArranger Arranger;
        public ECOTests(ArasCollectionFixture fixture) {
            this.fixture = fixture;
            AdminInn = fixture.GetAdminInn();
            CMInn = fixture.GetInnovatorBySessionName("CM");
            Arranger = new OOTBArranger();
        }

        private const string ITEM_TYPE = "Express ECO";

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "ECO")]
        [Trait("SmokeTest", "1")]
        public void Admin_can_find_an_ECO()
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
        public void Admin_can_create_an_ECO()
        {
            // Act
            Item eco = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = ArasTestBase.GetNewId();
            eco.setProperty("item_number", itemNumber);
            eco.setProperty("title", ArasTestBase.TEST_NAME);
            eco = eco.apply();

            // Assert
            AssertItem.IsNotError(eco);

        }

        [Fact]
        [Trait("Domain", "ECO")]
        [Trait("ECO", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_can_not_create_ECO_without_a_title()
        {
            // Arrange/Act
            Item eco = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = ArasTestBase.GetNewId();
            eco.setProperty("item_number", itemNumber);
            eco = eco.apply();

            // Assert
            AssertItem.IsError(eco);
        }

           [Theory]
        [InlineData("Part")]
        [InlineData("Document")]
        public void CM_can_Release_an_Item_via_ECO(string itemTypeToRelease) {
            // Arrange
            Arrange arrange = NewArrange(CMInn);
            Item ecoItem = arrange.CreateDefault(ITEM_TYPE);
            string ecoNumber = ecoItem.getProperty("item_number");
            Item itemToRelease = arrange.CreateDefault(itemTypeToRelease);
            Models.ECO eco = new Models.ECO(ecoItem);
            arrange.Run(() =>
            {
                Item ecoAffectedItem = eco.AddAffectedItem(itemToRelease, Models.ECO.AffectedItemAction.Release);
            });

            // Act/(Assert)
            Logger.LogInformation("Signing off ECO {ECOItemNumber} with action {Action}", ecoNumber, "Submit to Planning");
            Item result = eco.SignOff("Submit to Planning");
            AssertItem.IsNotError(result);
            Logger.LogInformation("Signing off ECO {ECOItemNumber} with action {Action}", ecoNumber, "Start Work");
            result = eco.SignOff("Start Work");
            AssertItem.IsNotError(result);
            Logger.LogInformation("Signing off ECO {ECOItemNumber} with action {Action}", ecoNumber, "Submit to Review");
            result = eco.SignOff("Submit to Review");
            AssertItem.IsNotError(result);
            Logger.LogInformation("Signing off ECO {ECOItemNumber} with action {Action}", ecoNumber, "Approve Changes");
            result = eco.SignOff("Approve Changes");
            AssertItem.IsNotError(result);

            // Assert
            Item releasedItem =  CMInn.getItemById(itemTypeToRelease, itemToRelease.getID(), "state");
            AssertItem.IsInState(releasedItem, "Released");
        }

        private Arrange NewArrange(Innovator.Client.IOM.Innovator inn) {
            return new Arrange(inn, Arranger);
        }

    }
}
