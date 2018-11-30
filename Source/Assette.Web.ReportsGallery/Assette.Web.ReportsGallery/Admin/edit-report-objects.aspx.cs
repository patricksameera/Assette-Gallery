using System;
using System.IO;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class edit_report_objects : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(edit_report_objects));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string GetReportObjects(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnValue = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnValue = proxy.GetReportObjects(sidx, sord, page, rows, search, searchField, searchString, searchOperator);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnValue;
        }

        [System.Web.Services.WebMethod]
        public static string UpdateReportObject(string Id, string preRegisterPreView, string isDeleted)
        {
            string rtnValue = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnValue = proxy.UpdateReportObject(int.Parse(Id), preRegisterPreView, isDeleted);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnValue;
        }

        [System.Web.Services.WebMethod]
        public static string GetPAObjectClientTypes()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetPAObjectClientTypes();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetPAObjeCtategories()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectCategories();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetPAObjectTypes()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectTypes();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        protected void btnExport_Click(object sender, EventArgs e)
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
    }
}