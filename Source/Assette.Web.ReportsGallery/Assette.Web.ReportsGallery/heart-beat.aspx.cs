using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Assette.ReportGallery.Proxy;
using System.Configuration;

namespace Assette.Web.ReportsGallery
{
    public partial class heartbeat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var proxy = new BusinessProxy();
            lblResponse.Text = proxy.CheckHeartBeat();
        }
    }
}