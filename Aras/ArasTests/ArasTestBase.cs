using System;
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
        protected readonly ArasCollectionFixture fixture;
        protected readonly Innovator.Client.IOM.Innovator AdminInn;

        public ArasTestBase(ArasCollectionFixture fixture, ITestOutputHelper output) {
            this.fixture = fixture;
            this.output = output;
            AdminInn = fixture.GetAdminInn();
        }

        protected AssertItem AssertItem(Item item) {
            return new AssertItem(item);
        }

        protected const string TEST_NAME = "AutoTest";

        protected static string GetNewId(int length = 8) {
            string id = Guid.NewGuid().ToString();
            if (length > 32) return id;
            return id.Substring(0,length);
        }
       
    }

   

}