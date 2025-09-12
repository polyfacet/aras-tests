using Innovator.Client.IOM;


namespace Aras.Core.Tests.ArasExtensions {
    public static class InnovatorExt {

        private const int WaitRetryTimeMs = 100;
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

        public static Item getItemByConfigId(this Innovator.Client.IOM.Innovator inn,
            string itemType,
            string config_id
            
            ) {

            Item item = inn.newItem(itemType, "get");
            item.setProperty("config_id", config_id);
            item = item.apply();
            return item;
        }

        public static Item GetIdentity(this Innovator.Client.IOM.Innovator inn) {
            string userId = inn.getUserID();
            string aml = $@"<AML>
                <Item action='get' type='User' id='{inn.getUserID()}' select='owned_by_id' /> 
                </AML>";
            Item user = inn.applyAML(aml);
            if (user.isError()) return user;
            return inn.getItemById("Identity", user.getProperty("owned_by_id", "N/A"));
        }

        public static Item ApplyAML(this Innovator.Client.IOM.Innovator inn, string aml) {
            Item res = inn.applyAML(aml);
            if (IsDeadLockError(res)) {
            System.Threading.Thread.Sleep(WaitRetryTimeMs);
            res = inn.applyAML(aml);
            }
            return res;       
        }

        private static bool IsDeadLockError(Item item) {
            if (!item.isError()) return false;
            if (item.getErrorString().Contains("deadlock victim")) return true;
            return false;
        }

    }
}
