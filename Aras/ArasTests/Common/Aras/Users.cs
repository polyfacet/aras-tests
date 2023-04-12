using Innovator.Client.IOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ArasTests.Common.Aras {
    internal class Users : InnovatorBase {
        public Users(Innovator.Client.IOM.Innovator inn) : base(inn) {
        }

        public Item CreateNewUser(string login, string password, string firstName, string lastName) {
            Item newUser = Inn.newItem("User", "add");
            newUser.setProperty("login_name", login);
            newUser.setProperty("first_name", firstName);
            newUser.setProperty("last_name", lastName);
            newUser.setProperty("password", CreateMD5(password).ToLower());
            newUser.setProperty("logon_enabled", "1");
            newUser = newUser.apply();
            return newUser;
        }

        public bool UserExists(string login) {
            Item user = Inn.newItem("User", "get");
            user.setProperty("login_name", login);
            user = user.apply();
            if (user.isError()) return false;
            return true;
        }

        private static string CreateMD5(string input) {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                // StringBuilder sb = new System.Text.StringBuilder();
                // for (int i = 0; i < hashBytes.Length; i++)
                // {
                //     sb.Append(hashBytes[i].ToString("X2"));
                // }
                // return sb.ToString();
            }
        }

        public Item AddUserAsMember(Item user, string identityNameToAddAsMember) {
            Item member = GetMemberRelation(user, identityNameToAddAsMember);
            if (!member.isError()) return member; // Already added
            
            Item userIdentity = GetIdentity(Inn, user.getID());

            Item parentIdentity = Inn.newItem("Identity", "get");
            parentIdentity.setAttribute("select", "id");
            parentIdentity.setProperty("name", identityNameToAddAsMember);
            parentIdentity = parentIdentity.apply();

            member = Inn.newItem("Member", "add");
            member.setProperty("source_id", parentIdentity.getID());
            member.setProperty("related_id", userIdentity.getID());
            member = member.apply();
            return member;
        }

        public Item GetMemberRelation(Item user, string parentIdentityName) {
            Item userIdentity = GetIdentity(Inn, user.getID());
            Item parentIdentity = Inn.newItem("Identity", "get");
            parentIdentity.setAttribute("select", "id");
            parentIdentity.setProperty("name", parentIdentityName);
            parentIdentity = parentIdentity.apply();

            Item member = Inn.newItem("Member", "get");
            member.setAttribute("select", "id");
            member.setAttribute("maxRecords", "1");
            member.setProperty("source_id", parentIdentity.getID());
            member.setProperty("related_id", userIdentity.getID());
            member = member.apply();
            return member;
        }

    }
}
