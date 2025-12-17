
namespace Aras.Core.Tests.Setup.Impl {
    public partial class TestFixtureParameterLoader {
        public class TestFixtureConfigException : ApplicationException {
            public TestFixtureConfigException(string message) : base(message) { }
        }
    }
}
