using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Aras.Core.Tests.Common.Aras {
    public class NewUserDTO {

        public readonly string LoginName;
        public readonly string Password;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly List<string> MemberOfIdentities;
        public List<Prop> Properties { get; internal set; }

        public NewUserDTO(
            string loginName, 
            string password, 
            string firstName, 
            string lastName,
            List<string> memberOfIdentities )
        {
            LoginName = loginName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            MemberOfIdentities = memberOfIdentities;
            Properties = new List<Prop>();
        }
    }

    public class Prop {
        public readonly string Name;
        public readonly string Value;
        public Prop(string name, string value) {
            Name = name; Value = value;
        }
    }
}
