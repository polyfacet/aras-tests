

namespace Aras.Core.Tests.Setup {
    public interface IFixture : IDisposable {
        Innovator.Client.IOM.Innovator CreateAndAddSessionFromLabel(string name);
        Innovator.Client.IOM.Innovator GetAdminInn();
    }
}
