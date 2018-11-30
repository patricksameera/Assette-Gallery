using System;
using System.Configuration;
using System.Web.UI.WebControls;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery
{
    public partial class samples : ClientBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(samples));

        protected void Page_Load(object sender, EventArgs e)
        {
            //GenerateApplicationErrorForTesting();

            //GenerateExceptionForTesting();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();

            // set tab message
            Label lblMessage = Master.FindControl("lblMessage") as Label;
            lblMessage.Text = "Click on an image on the right to view sales pitch books, client presentations and monthly and quarterly reports created using our applications. See how firms like yours are providing exceptional client communications by using Assette.";

            // setting delay time
            string strDelayTime = string.Empty;
            strDelayTime = ConfigurationManager.AppSettings["SamplesDelayTime"].ToString();
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
        public static string GetSampleReports(int pageIndex, int firmId, int productId, string searchText, int rowCount)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            int initialRecordCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RecordCountPerRow"]) * rowCount;

            try
            {
                rtnVal = proxy.GetSampleReports(pageIndex, firmId, productId, searchText, initialRecordCount);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetAllSampleReports()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllSampleReports();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetSampleReportPages(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetSampleReportPages(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetSampleReportDetails(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetSampleReportDetails(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetSampleReportIds(int firmId, int productId, string searchText)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetSampleReportIds(firmId, productId, searchText);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }
    }
}