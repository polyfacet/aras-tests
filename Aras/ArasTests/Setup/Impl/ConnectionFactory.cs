using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup.Impl
{
    internal class ConnectionFactory
    {
        internal static IConnectionParametersListLoader GetConnectionParametersListLoader() {
            return new TestFixtureConfigSessionLoader();
        }

        internal static INewUserDTO NewUserDTOLoader() {
            return new TestFixtureParameterLoader();
        }
    }
}
