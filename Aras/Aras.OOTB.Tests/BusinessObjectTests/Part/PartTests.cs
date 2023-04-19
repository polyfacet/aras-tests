﻿using Xunit.Abstractions;
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
        }

        private const string ITEM_TYPE = "Part";
        private const string RELEASED = "Released";

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

        [Theory]
        [InlineData("admin")]
        [InlineData("CM")]
        [Trait("Domain", "Part")]
        [Trait("Part", "Release")]
        private void Users_Should_Be_Able_To_Manually_Release_Part(string user) {
            Innovator.Client.IOM.Innovator inn = GetInnovatorBySessionName(user);
            User_Should_Be_Able_To_Manually_Release_Part(inn);
        }

        private void User_Should_Be_Able_To_Manually_Release_Part(Innovator.Client.IOM.Innovator inn)
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

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void CM_ShouldBeAbleToDeletePart_WhenNew()
        {
            // Arrange
            Arrange arrange = NewArrange(CMInn);
            Item part = arrange.CreateDefault(ITEM_TYPE);

            // Act
            Item result = part.apply("delete");

            // Assert
            AssertItem.IsNotError(result);

        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Business", "OOTB")]
        public void CM_ShouldNotBeAbleToDeletePart_WhenReleased()
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
        public void User_ShouldNotBeAbleToEditPart_WhenLockedByAnotherUser()
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
        public void CM_ShouldNotBeAbleToEditPart_WhenReleased()
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
        public void CM_ShouldBeAbleToCreateNewRevisionOfPart_WhenReleased()
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
