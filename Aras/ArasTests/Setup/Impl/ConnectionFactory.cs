
namespace Aras.Core.Tests.Setup.Impl
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
