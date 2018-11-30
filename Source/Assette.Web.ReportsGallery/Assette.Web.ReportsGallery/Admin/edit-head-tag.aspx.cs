using System;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class edit_head_tag : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(edit_head_tag));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string AddTagContent(string tagContent)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //GenerateJqueryErrorForTesting();

                rtnVal = proxy.AddTagContent(tagContent);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetTagContent()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetTagContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }
    }
}