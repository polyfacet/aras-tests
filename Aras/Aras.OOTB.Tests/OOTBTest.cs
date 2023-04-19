using Aras.OOTB.Tests.Fixture;
using ArasTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Aras.OOTB.Tests {


    [Collection("ArasOOTB")]
    public class OOTBTest : ArasTestBase {
        
        protected readonly Innovator.Client.IOM.Innovator CMInn;
        
        public OOTBTest(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output) {
            CMInn = fixture.GetInnovatorBySessionName("CM");
        }
    }
}
