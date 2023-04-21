using Innovator.Client.IOM;
using Aras.Core.Tests.Common;
using Aras.Core.Tests;
using Aras.Core.Tests.Models;

namespace Aras.OOTB.Tests.Models {
    internal class Document :
        ICreateNew
    {
        private const string ITEM_TYPE = "Document";

        public Item CreateNew(Innovator.Client.IOM.Innovator inn) {
            Item part = inn.newItem(ITEM_TYPE, "add");
            string itemNumber = Generators.GetNewId();;
            part.setProperty("item_number", itemNumber);
            part = part.apply();
            return part;
        }


    }
}
