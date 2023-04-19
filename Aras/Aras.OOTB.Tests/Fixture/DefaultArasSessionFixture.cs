using ArasTests.Common.Aras;
using ArasTests.Setup;
using ArasTests.Setup.Impl;
using Innovator.Client.IOM;
using Innovator.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Xunit;

namespace Aras.OOTB.Tests.Fixture
{


    public class DefaultArasSessionFixture : IArasFixture
    {
        [CollectionDefinition("ArasOOTB")]
        public class DatabaseCollection : ICollectionFixture<DefaultArasSessionFixture> {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }

        private readonly List<UserSession> UserSessions = new();
        private readonly ArasCollectionFixture fixture;

        public DefaultArasSessionFixture()
        {
            fixture = new ArasCollectionFixture();
            UserSessions = fixture.UserSessions;
        }
        

        public Innovator.Client.IOM.Innovator GetAdminInn() {
            return fixture.GetAdminInn();
        }

        public void Dispose()
        {
            Console.WriteLine("Disconnect from Aras and other cleaning up");
        }

        internal Innovator.Client.IOM.Innovator GetInnovatorBySessionName(string name) {
            return fixture.GetInnovatorBySessionName(name);
        }
    }
   
}
