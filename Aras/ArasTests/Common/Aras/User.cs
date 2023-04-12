using Innovator.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innovator.Client.IOM;
using IC = Innovator.Client.IOM;

namespace ArasTests.Common.Aras {
    internal class User {

        private IC.Innovator Inn;

        public User(IC.Innovator inn)
        {
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

        public static Item GetIdentity(IC.Innovator inn) {
            Item user = inn.newItem("User", "get");
            user.setAttribute("select", "owned_by_id");
            user.setID(inn.getUserID());
            user = user.apply();
            return user.getPropertyItem("owned_by_id");
        }
    }
}
