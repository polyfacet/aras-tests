using ArasTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Innovator.Client.IOM;

namespace ArasTests.AdminTests
{
    public class DocumentTests : ArasTestBase
    {
        public DocumentTests(ArasFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Domain", "Documents")]
        [Trait("SmokeTest", "1")]
        public void Admin_ShouldFindADocument() {
            // Act
            Item document = AdminInn.newItem("Document", "get");
            document.setAttribute("maxRecords", "1");
            document = document.apply();

            // Assert
            Assert.False(document.isError());
        }

        [Fact]
        [Trait("Domain", "Part")]
        [Trait("Document", "Create")]
        [Trait("Business", "OOTB")]
        public void Admin_ShouldBeAbleToCreateDocument() {
            // Act
            Item part = AdminInn.newItem("Document", "add");
            string itemNumber = Guid.NewGuid().ToString().Substring(0,8);
            part.setProperty("item_number", itemNumber);
            part.setProperty("name", "AutoTest");

            part = part.apply();

            // Assert
            AssertItem(part).IsNotError();
            
        }
    }
}
