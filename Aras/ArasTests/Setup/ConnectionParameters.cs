using Aras.Core.Tests.Common.Aras;
using Aras.Core.Tests.Setup.Impl;
using System.Collections.Generic;


namespace Aras.Core.Tests.Setup
{
    internal class ConnectionParameters
    {

        public ConnectionParameters(string connectionName, string url, string dbName, string loginName, string password) { 
            Label = connectionName;
            Url = url;
            DBName= dbName;
            LoginName= loginName;
            Password= password;
        }

        public string Label;
        public string Url;
        public string DBName;
        public string LoginName;
        public string Password;

        public static List<ConnectionParameters> GetConnectionParametersList() {
            return ConnectionFactory.GetConnectionParametersListLoader().GetConnectionParametersList();
        }

        public static NewUserDTO GetNewUser(string loginName) {
            var loader = ConnectionFactory.NewUserDTOLoader();
            return loader.GetNewUserDTO(loginName);
        }
    }
}
