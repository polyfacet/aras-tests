
namespace Aras.Core.Tests.Setup {
    public interface IFixture : IDisposable {
        Innovator.Client.IOM.Innovator GetAdminInn();
    }
}
