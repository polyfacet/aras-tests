using Aras.Core.Tests.Models;

namespace Aras.Core.Tests.Arranging {
    public interface IArasArranger {

        ICreateNew? GetCreateNewImpl(string itemType);
        ICreateApproved? GetApprovedImpl(string itemType);

    }
}
