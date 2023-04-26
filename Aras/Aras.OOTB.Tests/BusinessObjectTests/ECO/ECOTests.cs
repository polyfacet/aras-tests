using Xunit.Abstractions;
using Innovator.Client.IOM;
using Aras.Core.Tests;
using Aras.OOTB.Tests.Fixture;
using Aras.Core.Tests.Arranging;
using Aras.Core.Tests.ArasExtensions;

namespace Aras.OOTB.Tests.BusinessObjectTests.ECO
{
    public class ECOFixture : Aras.Core.Tests.Setup.ArasCollectionFixture, IDisposable
    {
        
        public ECOFixture()
        {
            
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }



    public class ECOTests : IClassFixture<ECOFixture>
    {
        ECOFixture fixture;
        Innovator.Client.IOM.Innovator AdminInn;
        Innovator.Client.IOM.Innovator CMInn;
        OOTBArranger Arranger;
        public ECOTests(ECOFixture fixture) {
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
        public void Admin_Can_Find_an_ECO()
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
        public void Admin_Can_Create_an_ECO()
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
        public void Admin_Can_NOT_Create_an_ECO_Without_Title()
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
        //[InlineData("Document")]
        public void CM_Can_Release_an_Item_via_ECO(string itemTypeToRelease) {
            // Arrange
            Arrange arrange = NewArrange(CMInn);
            Item ecoItem = arrange.CreateDefault(ITEM_TYPE);
            Item itemToRelease = arrange.CreateDefault(itemTypeToRelease);
            Models.ECO eco = new Models.ECO(ecoItem);
            arrange.Run(() =>
            {
                Item ecoAffectedItem = eco.AddAffectedItem(itemToRelease, Models.ECO.AffectedItemAction.Release);
            });

            // Act/(Assert)
            Item result = eco.SignOff("Submit to Planning");
            AssertItem.IsNotError(result);
            result = eco.SignOff("Start Work");
            AssertItem.IsNotError(result);
            result = eco.SignOff("Submit to Review");
            AssertItem.IsNotError(result);
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
