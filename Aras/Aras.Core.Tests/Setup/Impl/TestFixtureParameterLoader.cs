using Aras.Core.Tests.Common.Aras;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Aras.Core.Tests.Setup.Impl {
    public partial class TestFixtureParameterLoader : IConnectionParameterLoader, INewUserDTO
    {

        private string? _configFileName;
        private string ConfigFileName
        {
            get { 
                if (String.IsNullOrEmpty(_configFileName)) {
                    _configFileName = CONFIG_FILE_NAME;
                    string? testEnvironmentVariable = Environment.GetEnvironmentVariable("TestEnv");
                    if (!String.IsNullOrEmpty(testEnvironmentVariable)) {
                        _configFileName = $"{testEnvironmentVariable}_{_configFileName}";
                    }
                }
                return _configFileName; 
            }
        }
        

        private const string CONFIG_FILE_NAME = "TestFixture.config";
        private string Url = "http://localhost/innovator";
        private string DBName = "InnovatorSolutions";
        public readonly XmlDocument XmlDoc;

        public TestFixtureParameterLoader()
        {
            XmlDoc = new XmlDocument();
            string configFilePath = GetConfigFilePath();
            XmlDoc.Load(configFilePath);
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
            string path = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
            if (File.Exists(path)) {
                return path;
            }

            string? pathFromEnvironmentVariable = GetPathFromEnvironmentVariable("TestFixturePath");
            if (!string.IsNullOrEmpty(pathFromEnvironmentVariable) && File.Exists(pathFromEnvironmentVariable)) {
                return pathFromEnvironmentVariable;
            }

            string pathAlternative = GetConfigFileFromNearestParentFolder(path);
            
            if (File.Exists(pathAlternative)) {
                return pathAlternative;
            }

            string errorMessage = $"Config file not found: {path}";
            errorMessage += $" or {ConfigFileName} not found in any parent directory.";
            throw new FileNotFoundException(errorMessage);
        }

        private string? GetPathFromEnvironmentVariable(string environmentVariableName) {
            return Environment.GetEnvironmentVariable(environmentVariableName);
        }

        private string GetConfigFileFromNearestParentFolder(string path) {
            string pathAlternative = "N/A";
            DirectoryInfo currentDir = new DirectoryInfo(path);
            DirectoryInfo? parentDir = currentDir.Parent;
            while (parentDir != null) {
                pathAlternative = Path.Combine(parentDir.FullName, ConfigFileName);
                if (File.Exists(pathAlternative)) {
                    return pathAlternative;
                }
                parentDir = parentDir.Parent; 
            }
            return pathAlternative;
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

        public NewUserDTO GetNewUserDTO(string username) {
            string xPath = $"//User[@label='{username}']";
            XmlNode userNode = XmlDoc.SelectSingleNode(xPath)
                ?? throw new TestFixtureConfigException("Could not find node with xpath:" + xPath);

            var firstNameNode = userNode.Attributes?.GetNamedItem("firstName")
                    ?? throw new TestFixtureConfigException("firstName not found for: " + userNode.InnerText);
            string firstName = firstNameNode.InnerText;

            var lastNameNode = userNode.Attributes?.GetNamedItem("lastName")
                    ?? throw new TestFixtureConfigException("lastName not found for: " + userNode.InnerText);
            string lastName = lastNameNode.InnerText;

            var loginNode = userNode.SelectSingleNode("Login")
                ?? throw new TestFixtureConfigException("Could not find Login for xpath: " + xPath);
            string loginName = loginNode.InnerText;
            var passwordNode = userNode.SelectSingleNode("Password")
                ?? throw new TestFixtureConfigException("Could not find Password for xpath: " + xPath);
            string password = passwordNode.InnerText;

            List<string> memberOfList = new List<string>();
            XmlNodeList? memberOfNodes = userNode.SelectNodes("MemberOf");
            if (memberOfNodes != null)
                foreach (XmlNode membeOfNode in memberOfNodes)
                    memberOfList.Add(membeOfNode.InnerText);

            NewUserDTO newUserDTO = new NewUserDTO(
                loginName, password, firstName, lastName, memberOfList);

            var propertyNodes = userNode.SelectNodes("./Properties/Property");
            if (propertyNodes != null) {
                foreach (XmlNode propertyNode in propertyNodes)
                {
                    string? name = propertyNode.Attributes?.GetNamedItem("name")?.Value;
                    string? value = propertyNode.Attributes?.GetNamedItem("value")?.Value;
                    if (name != null && value!= null) {
                        Prop prop = new Prop(name, value);
                        newUserDTO.Properties.Add(prop);
                    }
                }
            }

            return newUserDTO;

            throw new TestFixtureConfigException("Could not find config for New User: " + username);
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
