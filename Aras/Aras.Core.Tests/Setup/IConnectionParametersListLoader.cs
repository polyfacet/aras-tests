using System.Collections.Generic;

namespace Aras.Core.Tests.Setup
{
    internal interface IConnectionParametersListLoader {
        public List<ConnectionParameters> GetConnectionParametersList();
    }
}
