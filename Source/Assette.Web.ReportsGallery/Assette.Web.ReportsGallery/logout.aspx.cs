using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assette.Web.ReportsGallery
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // clear cookie
            string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);

            // clear session
            HttpContext.Current.Session.Abandon();

            // redirect to default page
            Response.Redirect("default.aspx");
        }
    }
}