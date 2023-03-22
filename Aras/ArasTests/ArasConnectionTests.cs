using ArasTests.Setup;
using Xunit;
using Xunit.Abstractions;

namespace ArasTests
{
    public class ArasConnectionTests : ArasTestBase
    {
        public ArasConnectionTests(ArasFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        [Fact]
        [Trait("Category", "Core")]
        [Trait("Core", "Login")]
        public void LoginWithFixture_ShouldHaveALoggedInUser() {
            string loggedInUserId = fixture.GetAdminInn().getUserID();
            Assert.False(string.IsNullOrEmpty(loggedInUserId), "There should be a user id");
            output.WriteLine(fixture.GetAdminInn().getUserID());
        }
    }


}