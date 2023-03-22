using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup.Impl {
    internal class TestFixtureConfigSessionLoader : IConnectionParametersListLoader {
        
        List<ConnectionParameters> IConnectionParametersListLoader.GetConnectionParametersList() {
            var list = new List<ConnectionParameters>();
            ConnectionParameters param = new DevParameterLoader().GetConnectionParameters("admin");
            list.Add(param);
            return list;
        }
    }
}
