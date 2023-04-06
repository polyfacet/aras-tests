using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ArasTests.Setup
{
    public class ArasCollectionFixture : IDisposable
    {
        //public Connection Connection;
        private readonly List<UserSession> UserSessions = new();

        public ArasCollectionFixture()
        {
            foreach (ConnectionParameters param in ConnectionParameters.GetConnectionParametersList()) {
                var connection = Connection.CreateInstance(param.Url, param.DBName, param.LoginName, param.Password);
                UserSessions.Add(new UserSession(param.Name, connection.Inn));
            } 
        }

        public Innovator.Client.IOM.Innovator GetAdminInn() {
            return GetInnovatorBySessionName("admin");
        }

        public Innovator.Client.IOM.Innovator GetInnovatorBySessionName(string name) {
            var userSession = UserSessions.FirstOrDefault(u => u.Name?.ToUpper() == name?.ToUpper());
            if (userSession != null) return userSession.Inn;
            throw new ApplicationException($"No session with name: {name} found");
        }

        public void Dispose()
        {
            Console.WriteLine("Disconnect from Aras and other cleaning up");
        }
    }

    [CollectionDefinition("Aras setup")]
    public class DatabaseCollection : ICollectionFixture<ArasCollectionFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
