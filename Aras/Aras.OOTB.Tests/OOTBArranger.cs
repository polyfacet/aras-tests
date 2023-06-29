using Aras.Core.Tests.Arranging;
using Aras.Core.Tests.Models;

namespace Aras.OOTB.Tests {
    internal class OOTBArranger : Core.Tests.Arranging.IArasArranger {
        public ICreateApproved? GetApprovedImpl(string itemType) {
            if (itemType == "Part") return new Models.Part();
            return null;
        }

        public ICreateNew? GetCreateNewImpl(string itemType) {
            if (itemType == "Part") return new Models.Part();
            return null;
        }
    }
}
