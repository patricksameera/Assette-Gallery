using System;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery
{
    public partial class mygallery : ClientBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(mygallery));

        protected void Page_Load(object sender, EventArgs e)
        {
            //GenerateApplicationErrorForTesting();

            //GenerateExceptionForTesting();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();

            // set tab message
            Label lblMessage = Master.FindControl("lblMessage") as Label;
            lblMessage.Text = "";

            // hide share div
            HtmlGenericControl lblDivSahre = Master.FindControl("divShareLink") as HtmlGenericControl;
            lblDivSahre.Style.Add("display", "none");

            // setting delay time
            string strDelayTime = string.Empty;
            strDelayTime = ConfigurationManager.AppSettings["MyGalleryDelayTime"].ToString();
            if (string.IsNullOrEmpty(strDelayTime))
            {
                hdnDelayTime.Value = "2000";
            }
            else
            {
                hdnDelayTime.Value = strDelayTime;
            }
            strDelayTime = ConfigurationManager.AppSettings["MyGalleryGetNotesDelayTime"].ToString();
            if (string.IsNullOrEmpty(strDelayTime))
            {
                hdnGetNotesDelayTime.Value = "1000";
            }
            else
            {
                hdnGetNotesDelayTime.Value = strDelayTime;
            }          
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetPAObjectClientTypes()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectClientTypes();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetPAObjeCtategories()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectCategories();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetMyGalleryPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetMyGalleryPAObjects(pageIndex, categoryId, clientType, searchText, type, Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetAllMyGalleryPAObjects()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetAllMyGalleryPAObjects(Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetMyGalleryPAObjectDetails(int id, string type)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetMyGalleryPAObjectDetails(id, type);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string RemoveFromMyGallery(int id, string type)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.RemoveFromMyGallery(id, type);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string AddNotes(int id, string note)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.AddNotes(id, note);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string RemoveNotes(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.RemoveNotes(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetNotes(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetNotes(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        } 
    }
}