using System.Threading;
using Innovator.Client.IOM;

namespace Aras.Core.Tests.ArasExtensions;
public static class ItemExt {

    private const int WaitRetryTimeMs = 100;
    public static Item CreateRelation(this Item item, Item relatedItem, string relationshipName ) {
         Innovator.Client.IOM.Innovator inn = item.getInnovator();
        Item rel = inn.newItem(relationshipName, "add");
        rel.setProperty("source_id", item.getID());
        rel.setProperty("related_id", relatedItem.getID());
        return rel.apply();
    }

    public static Item Apply(this Item item) {
        Item res = item.apply();
        if (IsDeadLockError(res)) {
          Thread.Sleep(WaitRetryTimeMs);
          res = item.apply();
        }
        return res;       
      }

    public static Item Apply(this Item item, string action) {
        Item res = item.apply(action);
        if (IsDeadLockError(res)) {
          Thread.Sleep(WaitRetryTimeMs);
          res = item.apply(action);
        }
        return res;       
      }

    public static string GetProperty(this Item item, string propertyName) {
        try {
          string value = item.getProperty(propertyName);
          return value;
        }
        catch (Exception ex) {
          Thread.Sleep(WaitRetryTimeMs);
          return item.getProperty(propertyName);
        }
      }

    private static bool IsDeadLockError(Item item) {
        if (!item.isError()) return false;
        if (item.getErrorString().Contains("deadlock victim")) return true;
        return false;
    }

}

