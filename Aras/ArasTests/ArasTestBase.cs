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

        public const string TEST_NAME = Common.Generators.TEST_NAME;

        public static string GetNewId() {
            return Common.Generators.GetNewId();
        }
       
    }

   

}