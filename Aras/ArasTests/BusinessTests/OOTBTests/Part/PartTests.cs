using ArasTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Innovator.Client.IOM;
using ArasTests.Arranging;

namespace ArasTests.BusinessTests.OOTBTests.Part
{

    public class PartTests : OOTBTest
    {
        public PartTests(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        private const string ITEM_TYPE = "Part";

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "Part")]
        [Trait("SmokeTest", "1")]
        public void Admin_ShouldFindAPart()
        {
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
        public void Admin_ShouldBeAbleToCreatePart()
        {
            // Act
            Item part = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            part.setProperty("item_number", itemNumber);
            part.setProperty("name", TEST_NAME);

            part = part.apply();

            // Assert
            AssertItem.IsNotError(part);

        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Part", "Release")]
        [Trait("Business", "OOTB")]
        public void Admin_ShouldBeAbleToManuallyReleasePart()
        {
            User_ShouldBeAbleToManuallyReleasePart(AdminInn);
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Part", "Release")]
        [Trait("Business", "OOTB")]
        public void CM_ShouldBeAbleToManuallyReleasePart() {
            User_ShouldBeAbleToManuallyReleasePart(CMInn);
        }

        private void User_ShouldBeAbleToManuallyReleasePart(Innovator.Client.IOM.Innovator inn) {
            // Arrange
            Arrange arrange = new Arrange(inn);
            Item part = arrange.CreateDefault(ITEM_TYPE);

            // Act
            part = part.apply("PE_ManualRelease");

            // Assert
            AssertItem.IsNotError(part);
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void CM_ShouldBeAbleToDeletePart_WhenNew() {
            // Arrange
            Arrange arrange = new Arrange(CMInn);
            Item part = arrange.CreateDefault(ITEM_TYPE);

            // Act
            Item result = part.apply("delete");

            // Assert
            AssertItem.IsNotError(result);

        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void CM_ShouldNotBeAbleToDeletePart_WhenReleased() {
            // Arrange
            Arrange arrange = new Arrange(CMInn);
            Item part = arrange.CreateDefaultApproved(ITEM_TYPE);

            // Act
            Item result = part.apply("delete");

            // Assert
            AssertItem.IsError(result);
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void User_ShouldNotBeAbleToEditPart_WhenLockedByAnotherUser() {
            // Arrange
            Arrange arrange = new Arrange(AdminInn);
            Item part = arrange.CreateDefault(ITEM_TYPE);
            Item lockedPart = CMInn.newError("Temp");
            arrange.Run(() =>
            {
                part.apply("lock");
                string id = part.getID();
                lockedPart = CMInn.newItem(ITEM_TYPE, "get");
                lockedPart.setAttribute("select", "id");
                lockedPart.setID(id);
                lockedPart = lockedPart.apply();
            });


            // Act
            lockedPart.setAction("edit");
            lockedPart.setProperty("css", "red");
            Item result = lockedPart.apply();

            // Assert
            AssertItem.IsError(result);
        }

    }
}
