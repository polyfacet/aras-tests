using Aras.Core.Tests;
using Aras.Core.Tests.Arranging;
using Aras.OOTB.Tests.Fixture;
using Xunit.Abstractions;


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
