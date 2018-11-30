using System;
using System.Linq;
using System.Web.Security;

namespace Assette.Web.ReportsGallery.Admin
{
    public class UserManagement
    {
        const string PROVIDER_NAME = "AssetteMembershipProvider";

        internal string CreateUser(string username, string password, string email, string roles)
        {
            string returnValue = AddUser(username, password, email);

            int userId = 0;

            bool valid = Int32.TryParse(returnValue, out userId);

            if (valid && userId > 0)
            {
                AddRole(username, roles);
            }

            return returnValue;
        }

        private void AddRole(string username, string roles)
        {
            Roles.AddUserToRoles(username, roles.Split(','));
        }

        private string AddUser(string username, string password, string email)
        {
            MembershipCreateStatus status;

            MembershipUser user = Membership.Providers[PROVIDER_NAME].CreateUser(username, password, email, null, null, true, null, out status);

            if (user != null)
            {
                return user.ProviderUserKey.ToString();
            }
            else
            {
                return status.ToString();
            }
        }

        internal MembershipUserCollection GetAllUsers(int index, int pagesize)
        {
            int totalRecords;

            MembershipUserCollection members = Membership.GetAllUsers(index, pagesize, out totalRecords);

            return members;
        }

        internal bool LoginValidate(string username, string password)
        {
            bool createPersistentCookie = false;

            if (Membership.Providers[PROVIDER_NAME].ValidateUser(username, password))
            {
                FormsAuthentication.RedirectFromLoginPage(username, createPersistentCookie);

                return true;
            }

            return false;
        }

        internal string DeleteUsers(string users)
        {
            string[] userslsit = users.Split(',');

            if (userslsit != null && userslsit.Length > 0)
            {
                if (userslsit.Contains(Membership.GetUser().UserName))
                {
                    return "Cannot Delete Yourselves";
                }
                else
                {
                    foreach (string user in users.Split(','))
                    {

                        Membership.Providers[PROVIDER_NAME].DeleteUser(users, false);
                    }

                    return "0";
                }
            }

            return "User deletion failed";
        }
    }
}