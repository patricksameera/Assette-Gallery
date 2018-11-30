using System;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class edit_report_object_mockups : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(edit_report_object_mockups));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string UpdateReportObjectMockup(string Id, string name, string description, string clientType, string category, string type, string preRegisterPreView)
        {
            string rtnValue = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnValue = proxy.UpdateReportObjectMockup(Id, name, description, clientType, category, type, preRegisterPreView);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnValue;
        }

        [System.Web.Services.WebMethod]
        public static string GetReportObjectMockups(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnValue = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnValue = proxy.GetReportObjectMockups(sidx, sord, page, rows, search, searchField, searchString, searchOperator);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnValue;
        }

        [System.Web.Services.WebMethod]
        public static string DeleteReportObjectMockup(int Id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.DeleteReportObjectMockup(Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetAllPAObjeCtategories()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllPAObjectCategories();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetAllPAObjectTypes()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllPAObjectTypes();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }
    }
}