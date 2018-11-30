using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Assette.ReportGallery.Proxy;
using log4net;
using Newtonsoft.Json.Linq;

namespace Assette.Web.ReportsGallery
{
    public class ClientBasePage : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientBasePage));

        protected override void OnLoad(EventArgs e)
        {
            string userID = Request.QueryString["userid"];

            // if it's a query string url, it's direct url
            if (userID != null)
            {
                string email = string.Empty;

                var proxy = new BusinessProxy();

                string jsonEntUser = proxy.GetUserDetailsById(userID);

                JArray userObjects = JArray.Parse(jsonEntUser);

                // add session
                foreach (var userObject in userObjects)
                {
                    HttpContext.Current.Session["UserId"] = userObject["Id"].ToString();
                    HttpContext.Current.Session["Email"] = userObject["Email"].ToString();
                    HttpContext.Current.Session["Name"] = userObject["FirstName"].ToString();

                    email = userObject["Email"].ToString(); ;

                    break;
                }

                // add cookie
                string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];

                HttpCookie myCookie = new HttpCookie(cookieName);
                myCookie.Value = email;
                myCookie.Expires = DateTime.Now.AddDays(365d);
                HttpContext.Current.Response.Cookies.Add(myCookie);

                HttpCookie newCookie = new HttpCookie(cookieName);
                newCookie = HttpContext.Current.Request.Cookies[cookieName];
                string cookieValue = newCookie.Value;
            }
            else
            {
                // check whether the session exists
                if (Session["Email"] == null)
                {
                    string pagePath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                    FileInfo fileInfo = new FileInfo(pagePath);
                    string pageName = fileInfo.Name;

                    if (pageName.ToLower() != ("default.aspx").ToLower())
                    {
                        Response.Redirect("login.aspx?page=" + pageName);
                    }
                }
                else
                {
                    Label lblName = Master.FindControl("lblName") as Label;
                    lblName.Text = Session["Name"].ToString().ToUpper();
                }
            }  

            base.OnLoad(e);
        }

        public void SetLogInLogOutButtonVisibility()
        {
            Label lblName = Master.FindControl("lblName") as Label;
            HtmlGenericControl divLogin = Master.FindControl("divLogin") as HtmlGenericControl;
            HtmlGenericControl divLogout = Master.FindControl("divLogout") as HtmlGenericControl;

            //if (Session["Email"] == null)
            //{
            //    lblName.Text = "GUEST";
            //    divLogin.Style.Add("display", "inline");
            //    divLogout.Style.Add("display", "none");
            //}
            //else
            //{
            //    lblName.Text = Session["Name"].ToString().ToUpper();
            //    divLogin.Style.Add("display", "none");
            //    divLogout.Style.Add("display", "inline");
            //}
        }

        public static void GenerateApplicationErrorForTesting()
        {
            // application error test - testing purpose

            throw new ApplicationException("programmatically generating an application error");
        }

        public static void GenerateExceptionForTesting()
        {
            // exception test - testing purpose

            try
            {
                throw new ArgumentException("programmatically generating an exception");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static void GenerateJqueryErrorForTesting()
        {
            // jquery error test - testing purpose

            HttpContext context = HttpContext.Current;
            context.Response.Write("programmatically generating a jquery error");
        }
    }
}