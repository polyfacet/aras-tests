using Xunit.Abstractions;
using Innovator.Client.IOM;
using Aras.Core.Tests.Arranging;
using Aras.Core.Tests;
using Aras.OOTB.Tests.Fixture;
using Aras.Core.Tests.ArasExtensions;

namespace Aras.OOTB.Tests.BusinessObjectTests.Part
{

    public class PartTests : OOTBTest
    {
        public PartTests(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            Inn = AdminInn;
        }

        private const string ITEM_TYPE = "Part";
        private const string RELEASED = "Released";

        protected Innovator.Client.IOM.Innovator Inn { get; private set; }
        private void SetInn(string user) {
            Inn = GetInnovatorBySessionName(user);
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("CM")]
        [Trait("Domain", "Part")]
        [Trait("SmokeTest", "1")]
        public void User_Can_Find_A_Part(string user)
        {
            // Arrange
            SetInn(user);
          
            // Act
            Item part = Inn.newItem(ITEM_TYPE, "get");
            part.setAttribute("maxRecords", "1");
            part = part.apply();

            // Assert
            Assert.False(part.isError());
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("CM")]
        [Trait("Domain", "Part")]
        [Trait("Part", "Create")]
        public void User_Can_Create_Part(string user)
        {
            // Arrange
            SetInn(user);
          
            // Act
            Item part = Inn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            part.setProperty("item_number", itemNumber);
            part.setProperty("name", TEST_NAME);

            part = part.apply();

            // Assert
            AssertItem.IsNotError(part);

        }

        [Theory]
        [InlineData("admin")]
        [InlineData("CM")]
        [Trait("Domain", "Part")]
        [Trait("Part", "Release")]
        public void User_Can_Manually_Release_Part(string user) {
            // Arrange
            SetInn(user);
            UserCan_Manually_Release_Part(Inn);
        }

        private void UserCan_Manually_Release_Part(Innovator.Client.IOM.Innovator inn)
        {
            // Arrange
            Arrange arrange = NewArrange(inn);
            Item part = arrange.CreateDefault(ITEM_TYPE);

            // Act
            Item result = part.apply("PE_ManualRelease");

            // Assert
            AssertItem.IsNotError(result);
            // Reload part after release
            part = inn.getItemById(ITEM_TYPE, part.getID(), "state");
            AssertItem.IsInState(part, RELEASED);
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("CM")]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void User_Can_Delete_Their_New_Parts(string user)
        {
            // Arrange
            SetInn(user);
            Arrange arrange = NewArrange(Inn);
            Item part = arrange.CreateDefault(ITEM_TYPE);

            // Act
            Item result = part.apply("delete");

            // Assert
            AssertItem.IsNotError(result);

        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void CM_Can_NOT_Delete_a_Released_Part()
        {
            // Arrange
            Arrange arrange = NewArrange(CMInn);
            Item part = arrange.CreateDefaultApproved(ITEM_TYPE);

            // Act
            Item result = part.apply("delete");

            // Assert
            AssertItem.IsError(result);
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void User_Can_NOT_Edit_Part_When_Locked_By_Another_User()
        {
            // Arrange
            Arrange arrange = NewArrange(AdminInn);
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

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void CM_Can_NOT_Edit_a_Released_Part()
        {
            // Arrange
            Arrange arrange = NewArrange(CMInn);
            Item part = arrange.CreateDefaultApproved(ITEM_TYPE);

            // Act
            part.setAction("edit");
            part.setProperty("css", "red");
            Item result = part.apply();

            // Assert
            AssertItem.IsError(result);
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void CM_Can_Create_New_Revision_Of_a_Released_Part()
        {
            // Arrange
            Arrange arrange = NewArrange(CMInn);
            Item part = arrange.CreateDefaultApproved(ITEM_TYPE);

            // Act
            Item newRevision = part.apply("PE_CreateNewRevision");

            // Assert
            AssertItem.IsNotError(newRevision);
            string expectedRevision = "B";
            Assert.Equal(expectedRevision, newRevision.getProperty("major_rev"));
        }

    }
}
