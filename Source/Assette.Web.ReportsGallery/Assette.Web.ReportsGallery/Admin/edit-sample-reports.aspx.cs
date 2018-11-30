using System;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class edit_sample_reports : AdminBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(edit_sample_reports));

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
        public static string GetSamples(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnValue = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnValue = proxy.GetSamples(sidx, sord, page, rows, search, searchField, searchString, searchOperator);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnValue;
        }

        [System.Web.Services.WebMethod]
        public static string UpdateSampleReport(string Id, string name, string shortDescription, string longDescription, string firmId, string productId, string preRegisterPreView)
        {
            string rtnValue = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnValue = proxy.UpdateSampleReport(int.Parse(Id), name, shortDescription, longDescription, int.Parse(firmId), int.Parse(productId), preRegisterPreView);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnValue;
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
        public static string DeleteSampleReport(int Id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.DeleteSampleReport(Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }
    }
}