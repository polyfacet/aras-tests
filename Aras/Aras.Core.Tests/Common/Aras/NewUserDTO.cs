using System.Collections.Generic;

namespace Aras.Core.Tests.Common.Aras {
    internal class NewUserDTO {

        public readonly string LoginName;
        public readonly string Password;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly List<string> MemberOfIdentities;

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
        }
    }
}
