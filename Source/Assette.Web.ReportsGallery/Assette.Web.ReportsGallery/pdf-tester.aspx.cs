using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Assette.ReportGallery.Proxy;
using System.IO;

namespace Assette.Web.ReportsGallery
{
    public partial class pdf_tester : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(pdf_tester));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreatePdf_Click(object sender, EventArgs e)
        {
            var proxy = new BusinessProxy();

            byte[] pdfByteArray = proxy.GetAllPAObjectDetailsForPdf_itextsharp();

            string fileName = "PAObjects" + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".pdf";

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/pdf";

            MemoryStream stream = new MemoryStream(pdfByteArray);
            Response.Buffer = true;
            stream.WriteTo(Response.OutputStream);

            Response.End();
            Response.Flush();
        }

        protected void bttnCreatePdf_Click(object sender, EventArgs e)
        {
            var proxy = new BusinessProxy();

            byte[] pdfByteArray = proxy.GetAllPAObjectDetailsForPdf_pdfsharp();

            string fileName = "PAObjects" + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".pdf";

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/pdf";

            MemoryStream stream = new MemoryStream(pdfByteArray);
            Response.Buffer = true;
            stream.WriteTo(Response.OutputStream);

            Response.End();
            Response.Flush();
        }
    }
}