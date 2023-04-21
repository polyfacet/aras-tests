using Innovator.Client.IOM;
using Aras.Core.Tests.Common;
using Aras.Core.Tests;
using Aras.Core.Tests.Models;

namespace Aras.OOTB.Tests.Models {
    internal class ECO :
        ICreateNew
    {
        private const string ITEM_TYPE = "Express ECO";

        public Item CreateNew(Innovator.Client.IOM.Innovator inn) {
            Item part = inn.newItem(ITEM_TYPE, "add");
            part.setProperty("title", ArasTestBase.TEST_NAME);
            part = part.apply();
            return part;
        }


    }
}
