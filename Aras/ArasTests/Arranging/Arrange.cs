using ArasTests.Models;
using Innovator.Client.IOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArasTests.Arranging
{
    internal class Arrange {
        private Innovator.Client.IOM.Innovator Inn;

        public Arrange(Innovator.Client.IOM.Innovator inn) {
            this.Inn = inn;
        }

        internal Item CreateDefault(string itemType) {
            Item? item = null;
            ArrangeWrapper.Run(() =>
            {
                IDefaultCreateAble creator = GetImplementation(itemType);
                item = creator.CreateDefault(Inn);
                AssertItem.IsNotError(item);
            });
            if (item == null) { throw new ArrangeException(); }
            return item;
        }

        internal void Run(Action action) {
            ArrangeWrapper.Run(action);
        }

        private IDefaultCreateAble GetImplementation(string itemType) {
            if (itemType == "Part") return new Models.OOTB.Part();
            throw new NotImplementedException($"No implementation found for: {itemType}");
        }
    }
}
