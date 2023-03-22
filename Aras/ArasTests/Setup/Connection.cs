using Innovator.Client.Model;
using Innovator.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup
{
    public class Connection
    {
        private const string CONNECTION_NAME = "TestConnection";

        public Innovator.Client.IOM.Innovator Inn;

        public string Url = string.Empty;
        public string DB = string.Empty;
        public string UserName = string.Empty;
        private string Password = string.Empty;
        public int? TimeOutMilliSecs = null;
        public string Key = string.Empty;

        private static Connection? _connection;

        private Connection()
        {
            // Do not use... singleton...
        }
        private Connection(string url, string db, string userName, string password) {
            Url = url;
            DB= db;
            UserName = userName;
            Password = password;
            Inn = GetInnovator();
        }

        public static Connection GetInstance()
        {
            if (_connection == null) {
                throw new ApplicationException("Connection instance not created");
            }
            return _connection;
        }

        public static Connection CreateInstance(string url, string db, string userName, string password)
        {
            if (_connection == null)
            {
                _connection = new Connection(url,db, userName, password);
                _connection.Key = $"{url};{db};{userName};"; 
            }
            return _connection;
        }

        private Innovator.Client.IOM.Innovator GetInnovator() {
            Innovator.Client.IOM.Innovator inn;
            IRemoteConnection conn;
            if (TimeOutMilliSecs == null) {
                conn = Innovator.Client.Factory.GetConnection(Url, CONNECTION_NAME);
            }
            else {
                ConnectionPreferences connectionPreferences = new ConnectionPreferences();
                connectionPreferences.Url = Url;
                connectionPreferences.Name = CONNECTION_NAME;
                connectionPreferences.DefaultTimeout = TimeOutMilliSecs;
                conn = Innovator.Client.Factory.GetConnection(connectionPreferences);
            }
            conn.Login(new ExplicitCredentials(DB, UserName, Password));
            inn = new Innovator.Client.IOM.Innovator(conn);
            
            return inn;
        }
    }
}
