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
    public class Arrange {
        private Innovator.Client.IOM.Innovator Inn;

        public Arrange(Innovator.Client.IOM.Innovator inn) {
            this.Inn = inn;
        }

        public Item CreateDefault(string itemType) {
            Item? item = null;
            ArrangeWrapper.Run(() =>
            {
                ICreateNew creator = GetCreateNewImplementation(itemType);
                item = creator.CreateNew(Inn);
                AssertItem.IsNotError(item);
            });
            if (item == null) { throw new ArrangeException(); }
            return item;
        }

        public Item CreateDefaultApproved(string itemType) {
            Item? item = null;
            ArrangeWrapper.Run(() =>
            {
                ICreateApproved creator = GetCreateApprovedImplementation(itemType);
                item = creator.CreateApproved(Inn);
                AssertItem.IsNotError(item);
            });
            if (item == null) { throw new ArrangeException(); }
            return item;
        }

        public void Run(Action action) {
            ArrangeWrapper.Run(action);
        }

        private ICreateNew GetCreateNewImplementation(string itemType) {
            if (itemType == "Part") return new Models.OOTB.Part();
            throw new NotImplementedException($"No implementation found for: {itemType}");
        }

        private ICreateApproved GetCreateApprovedImplementation(string itemType) {
            if (itemType == "Part") return new Models.OOTB.Part();
            throw new NotImplementedException($"No implementation found for: {itemType}");
        }
    }
}
