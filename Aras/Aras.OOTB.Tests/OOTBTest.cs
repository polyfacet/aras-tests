using Aras.Core.Tests;
using Aras.Core.Tests.Arranging;
using Aras.OOTB.Tests.Fixture;
using Xunit.Abstractions;


namespace Aras.OOTB.Tests {


    [Collection("ArasOOTB")]
    public class OOTBTest : ArasTestBase {
        
        protected readonly Innovator.Client.IOM.Innovator CMInn;
        private readonly IArasArranger Arranger;
        private readonly DefaultArasSessionFixture defaultFixture;
        
        public OOTBTest(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output) {
            CMInn = fixture.GetInnovatorBySessionName("CM");
            defaultFixture = fixture;
            Arranger = new OOTBArranger();
        }

        public Innovator.Client.IOM.Innovator GetInnovatorBySessionName(string sessionName) {
            return defaultFixture.GetInnovatorBySessionName(sessionName);
        }

        protected Arrange NewArrange(Innovator.Client.IOM.Innovator inn) {
            return new Arrange(inn, Arranger);
        }
    }
}
