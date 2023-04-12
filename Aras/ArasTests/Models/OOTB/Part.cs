using Innovator.Client.IOM;
using ArasTests.Common;
using ArasTests.Common.Aras;

namespace ArasTests.Models.OOTB {
    internal class Part : IDefaultCreateAble {
        public Item CreateDefault(Innovator.Client.IOM.Innovator inn) {
            Item part = inn.newItem("Part", "add");
            string itemNumber = Generators.GetNewId();;
            part.setProperty("item_number", itemNumber);
            part.setProperty("name", ArasTestBase.TEST_NAME);
            part.setProperty("owned_by_id",  User.GetIdentity(inn).getID());
            part = part.apply();
            return part;
        }
    }
}
