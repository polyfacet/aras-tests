using Aras.Core.Tests;
using Aras.Core.Tests.Arranging;
using Aras.OOTB.Tests.Fixture;
using Microsoft.Extensions.Logging;
using Serilog;
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

        public static Microsoft.Extensions.Logging.ILogger CreateLogger() {
            var logger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", "ArasOOTBTests")
                .Enrich.WithProperty("Version", "1.0.6")  //TODO: Ersätt med 
                .WriteTo.Seq("http://localhost:5341") // Replace with your Seq server URL
                .CreateLogger();

            return LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(logger);
            }).CreateLogger<OOTBTest>();
        }

        public Innovator.Client.IOM.Innovator GetInnovatorBySessionName(string sessionName) {
            return defaultFixture.GetInnovatorBySessionName(sessionName);
        }

        protected Arrange NewArrange(Innovator.Client.IOM.Innovator inn) {
            return new Arrange(inn, Arranger);
        }
    }
}
