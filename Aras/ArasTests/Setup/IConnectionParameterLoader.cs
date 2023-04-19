
namespace Aras.Core.Tests.Setup
{
    internal interface IConnectionParameterLoader
    {
        public ConnectionParameters GetConnectionParameters(string name);
    }
}
