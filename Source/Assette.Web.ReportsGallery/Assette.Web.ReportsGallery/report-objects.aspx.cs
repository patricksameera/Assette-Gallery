using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using Assette.ReportGallery.Proxy;
using log4net;
using System.IO;
using System.Web.UI;

namespace Assette.Web.ReportsGallery
{
    public partial class reportobjects : ClientBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(reportobjects));

        protected void Page_Load(object sender, EventArgs e)
        {
            //GenerateApplicationErrorForTesting();

            //GenerateExceptionForTesting();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();

            // set tab message
            Label lblMessage = Master.FindControl("lblMessage") as Label;
            lblMessage.Text = "Click on an image on the right to see the library of chart and table layouts covering all angles needed to effectively communicate results to your clients. Your insights coupled with this broad array of content lets you easily show the value you provide to clients. " +
                "</br></br>" +
                "<div title='add to my gallery icon'><img src='Images/AddToLibrary.jpg' width='28' height='26' alt='Add To My Gallery' border='0' style='float: left; padding-right: 0px;' /></div>" +
                "&nbsp;&nbsp;As you view various layouts, click on this icon to add those you like to your own private gallery. This allows you to save layouts you are interested in for reviewing later.";
                //"<div style='color: #C1D82F;'>You can even send your gallery to colleagues so everyone can see how easy it is to take client communications to the next level.</div>";
            
            // setting delay time
            string strDelayTime = string.Empty;
            strDelayTime = ConfigurationManager.AppSettings["ReportObjectsDelayTime"].ToString();   
            if(string.IsNullOrEmpty(strDelayTime))
            {
                hdnDelayTime.Value = "2000";
            }
            else
            {
                hdnDelayTime.Value = strDelayTime;
            }
        }

        // working one
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

        // working one
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
        public static string GetPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type, int rowCount)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            int initialRecordCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RecordCountPerRow"]) * rowCount;

            try
            {
                rtnVal = proxy.GetPAObjects(pageIndex, categoryId, clientType, searchText, type, initialRecordCount, Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetAllPAObjects()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllPAObjects(Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetPAObjectIds(int categoryId, string clientType, string searchText, int type)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectIds(categoryId, clientType, searchText, type, Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetPAObjectDetails(int id)
        {
            string rtnVal = string.Empty;
            
            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectDetails(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetPAObjectDetailsByUserId(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetPAObjectDetailsByUserId(id, Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string AddToMyGallery(int id, string type)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.AddToMyGallery(id, type, Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string RemoveFromMyGallery(int id, string type)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.RemoveFromMyGallery(id, type, Guid.Parse(HttpContext.Current.Session["UserId"].ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        protected void lnkCreatePdf_Click(object sender, EventArgs e)
        {
            int[] paObjectIdList = new int[] { };
            int[] mockupObjectIdList = new int[] { };

            var proxy = new BusinessProxy();

            if (hdnPaObjectIds.Value != "")
            {
                paObjectIdList = Array.ConvertAll(hdnPaObjectIds.Value.Split(','), s => int.Parse(s));
            }

            if (hdnMockupObjectIds.Value != "")
            {
                mockupObjectIdList = Array.ConvertAll(hdnMockupObjectIds.Value.Split(','), s => int.Parse(s));
            }

            byte[] pdfByteArray = proxy.GetPAObjectDetailsForPdfByList(paObjectIdList, mockupObjectIdList);

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

        protected void lnkCreatePdf_Load(object sender, EventArgs e)
        {
            lnkCreatePdf.Attributes.Add("onclick", "return ExportLoading();");
        }
    }
}