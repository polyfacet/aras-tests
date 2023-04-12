using ArasTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ArasTests.BusinessTests.OOTBTests {
    public class OOTBTest : ArasTestBase {
        
        protected readonly Innovator.Client.IOM.Innovator CMInn;

        public OOTBTest(ArasCollectionFixture fixture, ITestOutputHelper output) : base(fixture, output) {
            CMInn = fixture.GetInnovatorBySessionName("CM");
        }
    }
}
