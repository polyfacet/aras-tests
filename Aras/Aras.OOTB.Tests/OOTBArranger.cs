using Aras.Core.Tests.Arranging;
using Aras.Core.Tests.Models;

namespace Aras.OOTB.Tests {
    internal class OOTBArranger : Core.Tests.Arranging.IArasArranger {

        public ICreateNew? GetCreateNewImpl(string itemType) {
            if (itemType == "Part") return new Models.Part();
            if (itemType == "Express ECO") return new Models.ECO();
            if (itemType == "Document") return new Models.Document();
            return null;
        }

        public ICreateApproved? GetApprovedImpl(string itemType) {
            if (itemType == "Part") return new Models.Part();
            return null;
        }
    }
}
