using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ArasTests.Setup.Impl {
    internal partial class TestFixtureParameterLoader : IConnectionParameterLoader
    {
        private const string CONFIG_FILE_NAME = "TestFixtureConfig.xml";
        private string Url = "http://localhost/innovator";
        private string DBName = "InnovatorSolutions";
        private readonly XmlDocument XmlDoc;

        public TestFixtureParameterLoader()
        {
            XmlDoc = new XmlDocument();
            XmlDoc.Load(GetConfigFilePath());
        }

        public ConnectionParameters GetConnectionParameters(string name)
        {

            XmlNode? node = XmlDoc.SelectSingleNode("//Url");
            Url = (node != null) ? node.InnerText : Url;

            node = XmlDoc.SelectSingleNode("//DatabaseName");
            DBName = (node != null) ? node.InnerText : DBName;

            var configUser = GetConfigUser(name);

            ConnectionParameters connectionParameters = new(
                name, Url, DBName, configUser.Login, configUser.Password);
            return connectionParameters;
        }

        private ConfigUser GetConfigUser(string name) {
            string xPath = $"//User[@label='{name}']";
            XmlNode node = XmlDoc.SelectSingleNode(xPath) 
                ?? throw new TestFixtureConfigException("Could not find node with xpath:" + xPath);
            var loginNode = node.SelectSingleNode("Login")
                ?? throw new TestFixtureConfigException("Could not find Login for xpath: " + xPath);
            var passwordNode = node.SelectSingleNode("Password")
                ?? throw new TestFixtureConfigException("Could not find Password for xpath: " + xPath);
            return new ConfigUser(loginNode.InnerText, passwordNode.InnerText);
        }

        private string GetConfigFilePath() {
            string path = Path.Combine(AppContext.BaseDirectory, CONFIG_FILE_NAME);
            if (File.Exists(path)) {
                return path;
            }
            throw new FileNotFoundException(path);
        }

        internal HashSet<string> GetUserLabels() {
            HashSet<string> labels = new HashSet<string>();
            string xPath = $"//User";
            var userNodes = XmlDoc.SelectNodes(xPath)
                ?? throw new TestFixtureConfigException("No User nodes found.");
            foreach (XmlNode userNode in userNodes)
            {
                var labelNode = userNode.Attributes?.GetNamedItem("label")
                    ?? throw new TestFixtureConfigException("Label not found for: " + userNode.InnerText);
                string label = labelNode.InnerText;
                labels.Add(label);
            }
            return labels;
        }

        private class ConfigUser {

            public readonly string Login;
            public readonly string Password;
            public ConfigUser(string login, string password)
            {
                Login = login;
                Password = password;
            }
        }
    }
}
