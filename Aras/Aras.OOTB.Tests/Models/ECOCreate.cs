using Innovator.Client.IOM;
using Aras.Core.Tests.Common;
using Aras.Core.Tests;
using Aras.Core.Tests.Models;

namespace Aras.OOTB.Tests.Models {
    internal class ECOCreate :
        ICreateNew
    {
        private const string ITEM_TYPE = "Express ECO";
        private const string PRODUCT_TEAM_ID = "862B05F3713F43E793735EDF11D54611";

        public Item CreateNew(Innovator.Client.IOM.Innovator inn) {
            Item item = inn.newItem(ITEM_TYPE, "add");
            item.setProperty("title", ArasTestBase.TEST_NAME);
            item.setProperty("owned_by_id", InnovatorBase.GetIdentity(inn).getID());
            item.setProperty("team_id", PRODUCT_TEAM_ID);
            item = item.apply();
            return item;
        }

    }
}
