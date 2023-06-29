using Innovator.Client.IOM;
using Xunit;

namespace Aras.Core.Tests {
    public class AssertItem
    {
        
        public static void IsNotError(Item item) {
            Assert.NotNull(item);
            Assert.False(item.isError(), item.getErrorString());
        }

        public static void IsNotError(Item item, string message) {
            Assert.NotNull(item);
            Assert.False(item.isError(), message);
        }

        public static void IsError(Item item) {
            IsError(item, $"Expected error item. DOM: {item.dom.InnerXml}");
        }

        public static void IsError(Item item, string message) {
            Assert.NotNull(item);
            Assert.True(item.isError(), message);
        }

        public static void IsInState(Item part, string expectedState) {
            AssertItem.IsNotError(part);
            string actualState = part.getProperty("state", "N/A");
            Assert.Equal(expectedState, actualState);
        }
    }
}
