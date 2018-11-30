using System;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class AdminMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();

            Response.Redirect("../Admin/login.aspx");
        }
    }
}