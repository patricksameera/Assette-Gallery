using System;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class edit_object_category_order : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(edit_object_category_order));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string GetPAObjectCategoryOrdering(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectCategoryOrdering(sidx, sord, page, rows, search, searchField, searchString, searchOperator);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string UpdateCategoryOrdering(string Id, string sortOrder, string isVisible)
        {
            string rtnValue = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnValue = proxy.UpdateCategoryOrdering(int.Parse(Id), int.Parse(sortOrder), isVisible);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnValue;
        }
    }
}