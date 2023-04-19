using Innovator.Client.IOM;

namespace Aras.Core.Tests {
    public class InnovatorBase
    {
        protected readonly Innovator.Client.IOM.Innovator Inn;
        public InnovatorBase(Innovator.Client.IOM.Innovator inn) {
            Inn = inn;
        }

        private Item? _identity;
        public Item Identity {
            get {
                if (_identity == null) {
                    _identity = GetIdentity(Inn);
                }
                return _identity;
            }
        }

        public static Item GetIdentity(Innovator.Client.IOM.Innovator inn) {
            return GetIdentity(inn, inn.getUserID());
        }

        public static Item GetIdentity(Innovator.Client.IOM.Innovator inn, string userId) {
            Item user = inn.newItem("User", "get");
            user.setAttribute("select", "owned_by_id");
            user.setID(userId);
            user = user.apply();
            return user.getPropertyItem("owned_by_id");
        }

        public static Item GetItemByConfigId(Innovator.Client.IOM.Innovator inn, string itemType, string configId) {
            Item item = inn.newItem(itemType, "get");
            item.setProperty("config_id", configId);
            item = item.apply();
            return item;
        }
    }
}
