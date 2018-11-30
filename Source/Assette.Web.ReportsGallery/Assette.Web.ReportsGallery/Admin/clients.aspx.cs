using System;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class clients : AdminBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(clients));

        protected void Page_Load(object sender, EventArgs e)
        {
            //GenerateApplicationErrorForTesting();

            //GenerateExceptionForTesting();

            Uri url = Request.Url;
            string authority = url.Authority;

            hdnUrl.Value = System.Configuration.ConfigurationManager.AppSettings["DirectURL"].ToString();

            // check user session availability
            CheckUserSession();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();          
        }

        [System.Web.Services.WebMethod]
        public static string GetClients(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string jsonEntUser = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //GenerateJqueryErrorForTesting();

                jsonEntUser = proxy.GetClients(sidx, sord, page, rows, search, searchField, searchString, searchOperator);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return jsonEntUser;
        }
    }
}