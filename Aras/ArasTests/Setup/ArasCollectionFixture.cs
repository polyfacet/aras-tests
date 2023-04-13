using ArasTests.Common.Aras;
using ArasTests.Setup.Impl;
using Innovator.Client.IOM;
using Innovator.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Xunit;

namespace ArasTests.Setup
{
    public class ArasCollectionFixture : IDisposable
    {
        //public Connection Connection;
        private readonly List<UserSession> UserSessions = new();
        private const string ADMIN = "admin";
        

        public ArasCollectionFixture()
        {
            foreach (ConnectionParameters param in ConnectionParameters.GetConnectionParametersList()) {
                string label = param.Label;
                string loginName = param.LoginName;
                if (label != ADMIN) {
                    CreateNewUserIfNotAlreadyExists(label, loginName);
                }
                var connection = Connection.CreateInstance(
                        param.Url, param.DBName, loginName, param.Password);
                UserSessions.Add(new UserSession(param.Label, connection.Inn));
            }
        }

        private void CreateNewUserIfNotAlreadyExists(string label, string loginName) {
            NewUserDTO newUser = ConnectionFactory.NewUserDTOLoader().GetNewUserDTO(label);
            Users users = new Users(GetAdminInn());
            if (!users.UserExists(loginName)) {
                Item newArasUser = users.CreateNewUser(
                    newUser.LoginName, newUser.Password, newUser.FirstName, newUser.LastName);
                foreach (var memberOf in newUser.MemberOfIdentities) {
                    users.AddUserAsMember(newArasUser, memberOf);
                }
            }
        }

        public Innovator.Client.IOM.Innovator GetAdminInn() {
            return GetInnovatorBySessionName(ADMIN);
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
