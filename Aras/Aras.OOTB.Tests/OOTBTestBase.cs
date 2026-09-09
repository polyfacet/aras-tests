using Aras.Core.Tests;
using Aras.Core.Tests.Arranging;
using Aras.Core.Tests.Setup;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Aras.OOTB.Tests
{
    public abstract class OOTBTestBase
    {
        protected Innovator.Client.IOM.Innovator AdminInn { get; }
        protected Innovator.Client.IOM.Innovator CMInn { get; }
        private readonly IArasArranger arranger;
        private readonly ArasCollectionFixture fixture;

        protected const string TEST_NAME = ArasTestBase.TEST_NAME;

        protected OOTBTestBase(ArasCollectionFixture fixture)
        {
            this.fixture = fixture;
            AdminInn = fixture.GetAdminInn();
            CMInn = fixture.GetInnovatorBySessionName("CM");
            arranger = new OOTBArranger();
        }

        protected static string GetNewId()
        {
            return ArasTestBase.GetNewId();
        }

        protected Innovator.Client.IOM.Innovator GetInnovatorBySessionName(string sessionName)
        {
            return fixture.GetInnovatorBySessionName(sessionName);
        }

        protected Arrange NewArrange(Innovator.Client.IOM.Innovator innovator)
        {
            return new Arrange(innovator, arranger);
        }

        public static Microsoft.Extensions.Logging.ILogger CreateLogger()
        {
            var logger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", "ArasOOTBTests")
                .Enrich.WithProperty("Version", "1.0.10")
                .Enrich.WithProperty("ArasRelease", "1.0.1")
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            return LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(logger);
            }).CreateLogger<OOTBTestBase>();
        }
    }
}
