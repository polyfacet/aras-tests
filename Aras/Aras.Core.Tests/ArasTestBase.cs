using Aras.Core.Tests.Setup;
using Xunit;

namespace Aras.Core.Tests {
    public abstract class ArasTestBase
    {
        protected readonly IFixture fixture;
        protected readonly Innovator.Client.IOM.Innovator AdminInn;
        

        public ArasTestBase(IFixture fixture) {
            this.fixture = fixture;
            AdminInn = fixture.GetAdminInn();
        }

        public const string TEST_NAME = Common.Generators.TEST_NAME;

        public static string GetNewId() {
            return Common.Generators.GetNewId();
        }
       
    }

   

}