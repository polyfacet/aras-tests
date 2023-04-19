using Aras.OOTB.Tests.Fixture;
using Aras.Core.Tests;
using Xunit.Abstractions;
using Aras.Core.Tests.Arranging;

namespace Aras.OOTB.Tests {


    [Collection("ArasOOTB")]
    public class OOTBTest : ArasTestBase {
        
        protected readonly Innovator.Client.IOM.Innovator CMInn;
        protected readonly IArasArranger Arranger;
        
        public OOTBTest(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output) {
            CMInn = fixture.GetInnovatorBySessionName("CM");
            Arranger = new OOTBArranger();
        }
    }
}
