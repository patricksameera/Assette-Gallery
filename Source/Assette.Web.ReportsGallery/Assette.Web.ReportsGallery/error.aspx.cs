using System;
using System.Web;

namespace Assette.Web.ReportsGallery
{
    public partial class error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // add error to error-page
            if (System.Configuration.ConfigurationManager.AppSettings["WriteToErrorPage"].ToString() == "1" && Cache["Error"] != null)
            {
                lblError.Text = HttpUtility.HtmlEncode(Cache["Error"].ToString());
            }

            // clear cookie
            string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);

            // clear session
            HttpContext.Current.Session.Abandon();  
        }
    }
}