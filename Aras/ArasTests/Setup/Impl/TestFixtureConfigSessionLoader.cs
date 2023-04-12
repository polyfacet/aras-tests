using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup.Impl {
    internal class TestFixtureConfigSessionLoader : IConnectionParametersListLoader {
        
        List<ConnectionParameters> IConnectionParametersListLoader.GetConnectionParametersList() {
            var list = new List<ConnectionParameters>();

            TestFixtureParameterLoader paramLoader = new();

            HashSet<string> userLabels = paramLoader.GetUserLabels();

            foreach (string label in userLabels) {
                ConnectionParameters param = paramLoader.GetConnectionParameters(label);
                list.Add(param);
            }
            return list;
        }
    }
}
