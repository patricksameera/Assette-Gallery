using System;
using System.Web;
using System.Web.UI.HtmlControls;
using log4net;
using Assette.ReportGallery.Proxy;
using Newtonsoft.Json.Linq;

namespace Assette.Web.ReportsGallery
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {       
        private static readonly ILog Log = LogManager.GetLogger(typeof(MasterPage));

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            // robots - meta tag
            HtmlMeta metaRobots = new HtmlMeta();
            metaRobots.Name = "robots";
            metaRobots.Content = "robots";
            MetaPlaceHolder.Controls.Add(metaRobots);

            // description - meta tag
            HtmlMeta metaDescription = new HtmlMeta();
            metaDescription.Name = "description";
            metaDescription.Content = "description";
            MetaPlaceHolder.Controls.Add(metaDescription);
            */

            if (!IsPostBack)
            {
                // set head tag content
                var proxy = new BusinessProxy();

                string tagContents = proxy.GetTagContent();

                JArray tagContentObjects = JArray.Parse(tagContents);

                // add session
                foreach (var tagContentObject in tagContentObjects)
                {
                    ltlHeader.Text = tagContentObject["ParamValue"].ToString();

                    break;
                }
            }

            // setting hidden fields
            hdnEmail.Value = HttpContext.Current.Session["Email"].ToString();
            hdnFirstName.Value = HttpContext.Current.Session["Name"].ToString();
            hdnUrl.Value = System.Configuration.ConfigurationManager.AppSettings["DirectURL"].ToString();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // clear cookie
            string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
            Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);

            // clear session
            HttpContext.Current.Session.Abandon();

            // redirect to default page
            Response.Redirect("default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx?page=default.aspx");
        }
    }
}