using Innovator.Client.IOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArasTests
{
    public class AssertItem
    {
        
        internal static void IsNotError(Item item) {
            Assert.NotNull(item);
            Assert.False(item.isError(), item.getErrorString());
        }

        internal static void IsError(Item item) {
            Assert.NotNull(item);
            Assert.True(item.isError(), $"Expected error item. DOM: {item.dom.InnerXml}");
        }
    }
}
