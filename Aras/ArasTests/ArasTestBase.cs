using ArasTests.Setup;
using Innovator.Client.IOM;
using Xunit;
using Xunit.Abstractions;

namespace ArasTests
{
    [Collection("Aras setup")]
    public abstract class ArasTestBase
    {
        protected readonly ITestOutputHelper output;
        protected readonly ArasFixture fixture;
        protected readonly Innovator.Client.IOM.Innovator AdminInn;

        public ArasTestBase(ArasFixture fixture, ITestOutputHelper output) {
            this.fixture = fixture;
            this.output = output;
            AdminInn = fixture.GetAdminInn();
        }

        protected AssertItem AssertItem(Item item) {
            return new AssertItem(item);
        }
       
    }

   

}