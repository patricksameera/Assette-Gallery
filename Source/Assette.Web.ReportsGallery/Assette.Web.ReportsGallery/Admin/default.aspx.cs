using System;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class admindefault : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // check user session availability
            CheckUserSession();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();
        }
    }
}