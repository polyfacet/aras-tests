using System;

namespace ArasTests.Setup.Impl {
    internal partial class TestFixtureParameterLoader {
        public class TestFixtureConfigException : ApplicationException {
            public TestFixtureConfigException(string message) : base(message) { }
        }
    }
}
