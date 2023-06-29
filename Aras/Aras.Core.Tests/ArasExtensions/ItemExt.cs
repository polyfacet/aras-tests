using Innovator.Client.IOM;

namespace Aras.Core.Tests.ArasExtensions;
public static class ItemExt {

    public static Item CreateRelation(this Item item, Item relatedItem, string relationshipName ) {
         Innovator.Client.IOM.Innovator inn = item.getInnovator();
        Item rel = inn.newItem(relationshipName, "add");
        rel.setProperty("source_id", item.getID());
        rel.setProperty("related_id", relatedItem.getID());
        return rel.apply();
    }


}

