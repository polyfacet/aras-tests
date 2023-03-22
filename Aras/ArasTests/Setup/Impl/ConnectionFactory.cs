using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup.Impl
{
    internal class ConnectionFactory
    {
        public static IConnectionParameterLoader GetConnectionParameterLoader()
        {
            return new DevParameterLoader();
        }

    }
}
