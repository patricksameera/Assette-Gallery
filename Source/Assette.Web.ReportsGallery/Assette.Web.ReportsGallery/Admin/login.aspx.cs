using System;
using System.Web;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class login : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(login));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UserManagement userManagement = new UserManagement();

            if (!userManagement.LoginValidate(username.Value, password.Value))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "showErrorMessage();", true);
            }
            else
            {
                HttpContext.Current.Session["AdminUserName"] = username.Value;
            }
        }

        [System.Web.Services.WebMethod]
        public static void ClientSideLogging(string message)
        {
            Log.Error(message);
        }
    }
}