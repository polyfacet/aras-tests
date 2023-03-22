using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArasTests.Setup.Impl;

namespace ArasTests.Setup
{
    internal class ConnectionParameters
    {

        public ConnectionParameters(string url, string dbName, string loginName, string password) { 
            Url = url;
            DBName= dbName;
            LoginName= loginName;
            Password= password;
        }

        public string Url;
        public string DBName;
        public string LoginName;
        public string Password;

        public static ConnectionParameters GetConnectionParameters(string name) {
            return ConnectionFactory.GetConnectionParameterLoader().GetConnectionParameters(name);
        }
    }
}
