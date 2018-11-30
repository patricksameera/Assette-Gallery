using System;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;
using Newtonsoft.Json.Linq;

namespace Assette.Web.ReportsGallery.HttpModules
{
    public class AuthenticationModule : IHttpModule
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthenticationModule));

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                HttpApplication application = (HttpApplication)sender;
                HttpContext context = application.Context;

                string emailExists = string.Empty;

                string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];
                HttpCookie myCookie = new HttpCookie(cookieName);
                myCookie = HttpContext.Current.Request.Cookies[cookieName];

                if (myCookie != null && myCookie.Value != "undefined")
                {
                    // string email = Server.UrlDecode();
                    string email = Uri.UnescapeDataString(myCookie.Value);
                    
                    var proxy = new BusinessProxy();

                    emailExists = proxy.CheckEmailExistence(email);

                    if (emailExists == "1")
                    {
                        string jsonEntUser = proxy.GetUserDetailsByEmail(email);

                        JArray userObjects = JArray.Parse(jsonEntUser);

                        foreach (var userObject in userObjects)
                        {
                            if (HttpContext.Current.Session != null)
                            {
                                HttpContext.Current.Session["UserId"] = userObject["Id"].ToString();
                                HttpContext.Current.Session["Email"] = userObject["Email"].ToString();
                                HttpContext.Current.Session["Name"] = userObject["FirstName"].ToString();

                                break;
                            }
                        }

                        // exception: Unable to evaluate expression because the code is optimized or a native frame is on top of the call stack
                        // to overcome add, make add response 'false'
                        //HttpContext.Current.Response.Redirect("client-welcome.aspx", false);
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("login.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
