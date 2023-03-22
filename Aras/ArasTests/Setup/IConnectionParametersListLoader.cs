using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup
{
    internal interface IConnectionParametersListLoader {
        public List<ConnectionParameters> GetConnectionParametersList();
    }
}
