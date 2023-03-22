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
        private Item item;

        public AssertItem(Item item) {
            this.item = item;
        }

        internal void IsNotError() {
            Assert.False(item.isError(), item.getErrorString());
        }
    }
}
