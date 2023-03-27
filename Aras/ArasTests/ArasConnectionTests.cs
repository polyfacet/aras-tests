using ArasTests.Setup;
using System;
using Xunit;
using Xunit.Abstractions;
using ArasTests.Arranging;

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

        [Fact]
        public void Test() {
            // Arrange
            string val = string.Empty;
            Action arrange = () => { 
                val = "Hepp";
                int i = 0;
                int j =3;
                int k = j/i;
            };
            ArrangeWrapper.Run(arrange);
            //Arranger.SetAction(arrangeAction).Run();

            // Act
            val = val.ToUpper();

            //Assert
            Assert.Equal("HEPP", val);
        }
    }
}