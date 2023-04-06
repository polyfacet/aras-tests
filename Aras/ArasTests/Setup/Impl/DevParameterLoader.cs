using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArasTests.Setup.Impl
{
    internal class DevParameterLoader : IConnectionParameterLoader
    {
        private string Url = "http://localhost/2023";
        private string DBName = "2023";
        private string LoginName = "admin";
        private string Password = "innovator";

        public ConnectionParameters GetConnectionParameters(string name)
        {
            ConnectionParameters connectionParameters = new ConnectionParameters(name, Url, DBName, LoginName, Password);
            return connectionParameters;
        }
    }
}
