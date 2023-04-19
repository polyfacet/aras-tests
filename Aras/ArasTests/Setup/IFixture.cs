using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Setup {
    public interface IFixture : IDisposable {
        Innovator.Client.IOM.Innovator GetAdminInn();
    }
}
