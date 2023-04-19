using Innovator.Client.IOM;
using Aras.Core.Tests.Common;
using Aras.Core.Tests;
using Aras.Core.Tests.Models;

namespace Aras.OOTB.Tests.Models {
    internal class Part :
        ICreateNew, 
        ICreateApproved {

        private const string ITEM_TYPE = "Part";

        public Item CreateNew(Innovator.Client.IOM.Innovator inn) {
            Item part = inn.newItem(ITEM_TYPE, "add");
            string itemNumber = Generators.GetNewId();;
            part.setProperty("item_number", itemNumber);
            part.setProperty("name", ArasTestBase.TEST_NAME);
            part.setProperty("owned_by_id", InnovatorBase.GetIdentity(inn).getID());
            part = part.apply();
            return part;
        }

        public Item CreateApproved(Innovator.Client.IOM.Innovator inn) {
            Item part = CreateNew(inn);
            string configId = part.getProperty("config_id");
            Item result = part.apply("PE_ManualRelease");
            if (result.isError()) throw new Exception(result.getErrorString());
            part = InnovatorBase.GetItemByConfigId(inn, ITEM_TYPE, configId);
            return part;
        }
    }
}
