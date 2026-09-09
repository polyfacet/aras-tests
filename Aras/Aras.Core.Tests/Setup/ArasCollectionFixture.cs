using Aras.Core.Tests.Common.Aras;
using Aras.Core.Tests.Setup.Impl;
using Innovator.Client.IOM;
using System.Collections.Generic;
using System.Linq;
using Xunit;

[assembly: AssemblyFixture(typeof(Aras.Core.Tests.Setup.ArasCollectionFixture))]

namespace Aras.Core.Tests.Setup
{
    public class ArasCollectionFixture : IArasFixture
    {
        //public Connection Connection;
        public readonly List<UserSession> UserSessions = new();
        private const string ADMIN = "admin";
        

        public ArasCollectionFixture()
        {
            foreach (ConnectionParameters param in ConnectionParameters.GetConnectionParametersList()) {
                string label = param.Label;
                string loginName = param.LoginName;
                if (label != ADMIN) {
                    if (!UserExists(loginName)) continue;
                    if (!UserIsEnabled(loginName)) continue;
                }
                var connection = Connection.CreateInstance(
                        param.Url, param.DBName, loginName, param.Password);
                UserSessions.Add(new UserSession(param.Label, connection.Inn));
            }
        }

        private bool UserIsEnabled(string loginName)
        {
            Innovator.Client.IOM.Innovator adminInn = GetAdminInn();
            Users users = new Users(adminInn);
            if (users.UserIsEnabled(loginName)) return true;
            return false;
        }

        private bool UserExists(string loginName) {
            Innovator.Client.IOM.Innovator adminInn = GetAdminInn();
            Users users = new Users(adminInn);
            if (users.UserExists(loginName)) return true;
            return false;
        }

        private void CreateNewUserIfNotAlreadyExists(string label, string loginName) {
            Innovator.Client.IOM.Innovator adminInn = GetAdminInn();
            NewUserDTO newUser = ConnectionFactory.NewUserDTOLoader().GetNewUserDTO(label);
            Users users = new Users(adminInn);
            if (!users.UserExists(loginName)) {
                if (Connection.IsMD5(newUser.Password)) throw new Exception("Can not create a new user with a hashed password.");
                Item newArasUser = users.CreateNewUser(
                    newUser.LoginName, newUser.Password, newUser.FirstName, newUser.LastName);
                foreach (var prop in newUser.Properties) {
                    newArasUser.setAction("edit");
                    newArasUser.setProperty(prop.Name, prop.Value);
                    newArasUser.apply();
                }
                foreach (var memberOf in newUser.MemberOfIdentities) {
                    users.AddUserAsMember(newArasUser, memberOf);
                }
            }
            else
            {
                // Make sure it has logon enabled
                Item user = users.GetUserByLoginName(loginName);
                bool logonEnabled = (user.getProperty("logon_enabled","0") == "1") ? true : false;
                if (!logonEnabled)
                {
                    Item updateUser = adminInn.newItem(user.getType(), "edit");
                    updateUser.setID(user.getID());
                    updateUser.setProperty("logon_enabled", "1");
                    updateUser = updateUser.apply();
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
            //Console.WriteLine("Disconnect from Aras and other cleaning up");
        }

        public Innovator.Client.IOM.Innovator CreateAndAddSessionFromLabel(string name)
        {
            
            NewUserDTO newUser = ConnectionFactory.NewUserDTOLoader().GetNewUserDTO(name);          
            string loginName = newUser.LoginName;
            CreateNewUserIfNotAlreadyExists(name, loginName);

            var param = ConnectionParameters.GetConnectionParametersList().First();
             var connection = Connection.CreateInstance(
                         param.Url, param.DBName, loginName, newUser.Password);
            UserSessions.Add(new UserSession(name, connection.Inn));
            return connection.Inn;
        }
    }

}
