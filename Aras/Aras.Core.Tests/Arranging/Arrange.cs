using Aras.Core.Tests.Models;
using Innovator.Client.IOM;

namespace Aras.Core.Tests.Arranging
{
    public class Arrange {
        private Innovator.Client.IOM.Innovator Inn;
        private IArasArranger Arranger;

        public Arrange(Innovator.Client.IOM.Innovator inn, IArasArranger arasArranger) {
            Inn = inn;
            Arranger = arasArranger;
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
            ICreateNew? impl = Arranger.GetCreateNewImpl(itemType);
            if (impl == null) throw new NotImplementedException($"No implementation found for: {itemType}");
            return impl;
        }

        private ICreateApproved GetCreateApprovedImplementation(string itemType) {
            ICreateApproved? impl = Arranger.GetApprovedImpl(itemType);
            if (impl == null) throw new NotImplementedException($"No implementation found for: {itemType}");
            return impl;
        }
    }
}
