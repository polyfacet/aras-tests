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
        public void Admin_ShouldFindADocument() {
            // Act
            Item document = AdminInn.newItem("Document", "get");
            document.setAttribute("maxRecords", "1");
            document = document.apply();

            // Assert
            Assert.False(document.isError());
        }
    }
}
