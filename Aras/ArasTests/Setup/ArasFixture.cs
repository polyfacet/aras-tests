using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArasTests.Setup
{
    public class ArasFixture : IDisposable
    {
        public Innovator.Client.IOM.Innovator Inn;
        public Connection Connection;

        public ArasFixture()
        {
            Console.WriteLine("Setup and connect to Aras");
            ConnectionParameters param = ConnectionParameters.GetConnectionParameters("Test");
            Connection = Connection.CreateInstance(param.Url,param.DBName,param.LoginName, param.Password);
            Inn = Connection.Inn;
        }

        public void Dispose()
        {
            Console.WriteLine("Disconnect from Aras and other cleaning up");
        }
    }

    [CollectionDefinition("Aras setup")]
    public class DatabaseCollection : ICollectionFixture<ArasFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
