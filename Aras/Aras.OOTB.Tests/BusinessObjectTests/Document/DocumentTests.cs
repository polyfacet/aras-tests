using Innovator.Client.IOM;
using Aras.Core.Tests;

namespace Aras.OOTB.Tests.BusinessObjectTests.Document
{
    public class DocumentTests : OOTBTestBase
    {
        public DocumentTests(Aras.Core.Tests.Setup.ArasCollectionFixture fixture) : base(fixture) { }

        private const string ITEM_TYPE = "Document";

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "Documents")]
        [Trait("SmokeTest", "1")]
        public void Admin_can_find_a_Document()
        {
            // Act
            Item document = AdminInn.newItem(ITEM_TYPE, "get");
            document.setAttribute("maxRecords", "1");
            document = document.apply();

            // Assert
            Assert.False(document.isError());
        }

        [Fact]
        [Trait("Domain", "Documents")]
        [Trait("Document", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_can_create_Document()
        {
            // Act
            Item document = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            document.setProperty("item_number", itemNumber);
            document.setProperty("name", TEST_NAME);

            document = document.apply();

            // Assert
            AssertItem.IsNotError(document);

        }

        [Fact]
        [Trait("Domain", "Documents")]
        [Trait("Document", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_can_not_create_Document_without_an_item_number()
        {
            // Act
            Item document = AdminInn.newItem(ITEM_TYPE, "add");
            string itemNumber = GetNewId();
            document = document.apply();

            // Assert
            AssertItem.IsError(document);


        }
    }
}
