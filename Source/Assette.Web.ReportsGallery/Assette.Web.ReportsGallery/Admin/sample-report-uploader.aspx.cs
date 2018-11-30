using System;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class sample_report_uploader : AdminBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(sample_report_uploader));

        protected void Page_Load(object sender, EventArgs e)
        {
            //GenerateApplicationErrorForTesting();

            //GenerateExceptionForTesting();

            // check user session availability
            CheckUserSession();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();
        }

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
    }
}