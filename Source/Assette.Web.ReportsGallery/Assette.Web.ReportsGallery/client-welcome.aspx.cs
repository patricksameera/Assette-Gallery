using System;
using log4net;

namespace Assette.Web.ReportsGallery
{
    public partial class client_welcome : ClientBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(client_welcome));

        protected void Page_Load(object sender, EventArgs e)
        {
            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();
        }
    }
}