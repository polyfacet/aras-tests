using Innovator.Client.IOM;
using Xunit;

namespace Aras.Core.Tests {
    public class AssertItem
    {
        
        public static void IsNotError(Item item) {
            Assert.NotNull(item);
            Assert.False(item.isError(), item.getErrorString());
        }

        public static void IsError(Item item) {
            Assert.NotNull(item);
            Assert.True(item.isError(), $"Expected error item. DOM: {item.dom.InnerXml}");
        }
    }
}
