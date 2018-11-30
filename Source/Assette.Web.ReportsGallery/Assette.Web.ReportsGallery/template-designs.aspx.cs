using System;
using System.Configuration;
using System.Web.UI.WebControls;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery
{
    public partial class templatedesigns : ClientBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(templatedesigns));

        protected void Page_Load(object sender, EventArgs e)
        {
            //GenerateApplicationErrorForTesting();

            //GenerateExceptionForTesting();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();

            // set tab message
            Label lblMessage = Master.FindControl("lblMessage") as Label;
            lblMessage.Text = "Click on an image on the right to view the designs that are provided with Assette. You can use the designs as-is, modify them as you see fit or come up with your own designs. The various chart and table layouts coupled with pre-created template designs, let you get started quickly and easily.";

            // setting delay time
            string strDelayTime = string.Empty;
            strDelayTime = ConfigurationManager.AppSettings["TemplateDesignDelayTime"].ToString();
            if (string.IsNullOrEmpty(strDelayTime))
            {
                hdnDelayTime.Value = "2000";
            }
            else
            {
                hdnDelayTime.Value = strDelayTime;
            }
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetFirmTypes()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetFirmTypes();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetProductTypes()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetProductTypes();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetTemplateDesigns(int pageIndex, int firmId, int productId, string searchText, int rowCount)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            int initialRecordCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RecordCountPerRow"]) * rowCount;

            try
            {
                rtnVal = proxy.GetTemplateDesigns(pageIndex, firmId, productId, searchText, initialRecordCount);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetTemplateDesignPages(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetTemplateDesignPages(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetTemplateDesignDetails(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetTemplateDesignDetails(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetTemplateDesignRecordIds(int firmId, int productId, string searchText)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetTemplateDesignRecordIds(firmId, productId, searchText);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetAllTemplateDesigns()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllTemplateDesigns();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }
    }
}