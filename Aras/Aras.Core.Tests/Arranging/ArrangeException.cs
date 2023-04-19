
namespace Aras.Core.Tests.Arranging
{
    internal class ArrangeException : Exception
    {
        public ArrangeException(string message, Exception ex) : base(message, ex) {

        }

        public ArrangeException() : base() { }
    }
}
