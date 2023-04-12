using ArasTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Innovator.Client.IOM;

namespace ArasTests.BusinessTests.OOTBTests.Document
{
    public class DocumentTests : ArasTestBase
    {
        public DocumentTests(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        private const string ITEM_TYPE = "Document";

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "Documents")]
        [Trait("SmokeTest", "1")]
        public void Admin_ShouldFindADocument()
        {
            // Act
            Item document = AdminInn.newItem(ITEM_TYPE, "get");
            document.setAttribute("maxRecords", "1");
            document = document.apply();

            // Assert
            Assert.False(document.isError());
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Document", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_ShouldBeAbleToCreateDocument()
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
    }
}
