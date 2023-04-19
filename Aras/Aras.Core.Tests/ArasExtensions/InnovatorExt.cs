using Innovator.Client.IOM;


namespace Aras.Core.Tests.ArasExtensions {
    public static class InnovatorExt {

        public static Item getItemById(this Innovator.Client.IOM.Innovator inn,
            string itemType,
            string id,
            string select
            ) {

            Item item = inn.newItem(itemType, "get");
            item.setID(id);
            item.setAttribute("select", select);
            item = item.apply();
            return item;
        }

    }
}
