using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public class AdminBasePage : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdminBasePage));

        protected override void OnLoad(EventArgs e)
        {
            if (Session["AdminUserName"] == null)
            {
                Response.Redirect("../Admin/login.aspx");
            }
            else
            {
                Label lblName = Master.FindControl("lblName") as Label;
                lblName.Text = Session["AdminUserName"].ToString().ToUpper();
            }

            base.OnLoad(e);
        }

        public void SetLogInLogOutButtonVisibility()
        {
            Label lblName = Master.FindControl("lblName") as Label;
            HtmlGenericControl divLogout = Master.FindControl("divLogout") as HtmlGenericControl;

            lblName.Text = Session["AdminUserName"].ToString().ToUpper();
            divLogout.Style.Add("display", "inline");
        }

        public void CheckUserSession()
        {
            
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