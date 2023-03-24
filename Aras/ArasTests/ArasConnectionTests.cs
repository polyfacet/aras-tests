using ArasTests.Setup;
using System;
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
            Arranger.Run(arrange);
            //Arranger.SetAction(arrangeAction).Run();

            // Act
            val = val.ToUpper();

            //Assert
            Assert.Equal("HEPP", val);
        }

        private class Arranger {
            private Action _action;
            public Arranger(Action action) {
                _action = action;
            }

            public static Arranger SetAction(Action action) {
                return new Arranger(action);
            }

            public void Run() {
                try {
                    _action();
                }
                catch (Exception ex) {
                    throw new ArrangeException("Arrange exception: ", ex);
                }
            }

            public static void Run(Action action) {
                try {
                    action();
                }
                catch (Exception ex) {
                    throw new ArrangeException("Arrange exception: ", ex);
                }
            }
        }

        public class ArrangeException : Exception {
            public ArrangeException(string message, Exception ex) : base(message, ex) {
               
            }
        }


    }


}