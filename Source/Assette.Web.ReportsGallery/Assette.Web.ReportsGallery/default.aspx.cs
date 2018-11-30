using System;
using System.Configuration;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Assette.Web.ReportsGallery
{
    public partial class index : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(index));

        protected void Page_Load(object sender, EventArgs e)
        {
            GetFirstSampleReportsForPreview();
            GetFirstAObjectsForPreView();
            GetFirstTemplateDesignsForPreView();

            //ClientBasePage.GenerateApplicationErrorForTesting();

            //ClientBasePage.GenerateExceptionForTesting();

            if (!IsPostBack)
            {
                // set head tag content
                var proxy = new BusinessProxy();

                string tagContents = proxy.GetTagContent();

                JArray tagContentObjects = JArray.Parse(tagContents);

                // add session
                foreach (var tagContentObject in tagContentObjects)
                {
                    ltlHeader.Text = tagContentObject["ParamValue"].ToString();

                    break;
                }
            }

            // check whether there is a cookie
            // if so, redirect to client-welcome.aspx
            // or else stay on the default.aspx page
            GetCookieTransfer();

            // setting delay time
            string strDelayTime = string.Empty;
            strDelayTime = ConfigurationManager.AppSettings["PreviewDelayTime"].ToString();
            if (string.IsNullOrEmpty(strDelayTime))
            {
                hdnDelayTime.Value = "2000";
            }
            else
            {
                hdnDelayTime.Value = strDelayTime;
            }
        }

        public void GetCookieTransfer()
        {
            string emailExists = string.Empty;

            string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];

            HttpCookie myCookie = new HttpCookie(cookieName);
            myCookie = HttpContext.Current.Request.Cookies[cookieName];

            // if cookie exists, redirect to client-welcome.aspx
            if (myCookie != null && myCookie.Value != "undefined")
            {
                string email = Server.UrlDecode(myCookie.Value);

                var proxy = new BusinessProxy();

                try
                {
                    emailExists = proxy.CheckEmailExistence(email);

                    if (emailExists == "1")
                    {
                        string jsonEntUser = proxy.GetUserDetailsByEmail(email);

                        JArray userObjects = JArray.Parse(jsonEntUser);

                        foreach (var userObject in userObjects)
                        {
                            HttpContext.Current.Session["UserId"] = userObject["Id"].ToString();
                            HttpContext.Current.Session["Email"] = userObject["Email"].ToString();
                            HttpContext.Current.Session["Name"] = userObject["FirstName"].ToString();

                            break;
                        }

                        // exception: Unable to evaluate expression because the code is optimized or a native frame is on top of the call stack
                        // to overcome add, make add response 'false'
                        Response.Redirect("client-welcome.aspx", false);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        #region Sample Reports

        [System.Web.Services.WebMethod]
        public static string GetSampleReportPages(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetSampleReportPages(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetSampleReportDetails(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetSampleReportDetails(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetSampleReportsForPreview()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //ClientBasePage.GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetSampleReportsForPreview();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetAllSampleReportsForPreview()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //ClientBasePage.GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetAllSampleReportsForPreview();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        #endregion

        #region Template Designs

        [System.Web.Services.WebMethod]
        public static string GetTemplateDesignPages(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetTemplateDesignPages(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetTemplateDesignDetails(int id)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetTemplateDesignDetails(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetTemplateDesignsForPreView()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetTemplateDesignsForPreView();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetAllTemplateDesignsForPreView()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllTemplateDesignsForPreView();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        #endregion

        #region PAObjects

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
        public static string GetPAObjectsForPreView()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //ClientBasePage.GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetPAObjectsForPreView();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        // working one
        [System.Web.Services.WebMethod]
        public static string GetAllPAObjectsForPreView()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //ClientBasePage.GenerateJqueryErrorForTesting();

                rtnVal = proxy.GetAllPAObjectsForPreView();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        public void GetFirstSampleReportsForPreview()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllSampleReportsForPreview();

                JArray sampleObjects = JArray.Parse(rtnVal);

                if (sampleObjects.Count > 0)
                {
                    DateTime date = DateTime.Parse(sampleObjects[0]["UploadedDate"].ToString());

                    string newDate = String.Format("{0:yyyyMMddHHmmss}", date);

                    //SamplesPreView.InnerHtml = "<a href='#' class='ViewLibrarySamples' Index='1' Id='" + sampleObjects[0]["ID"].ToString() + "'><img  class='PreviewImage urlImageSmall' src='ImageHandler.ashx?id=" + sampleObjects[0]["ID"].ToString() + "&type=s_t&date=" + newDate + "&uniqueindex=0' width='140' height='90'  border='0' style='float: left;padding:4px 2px' /></a>";
                    SamplesPreView.InnerHtml = "<a href='#' class='ViewLibrarySamples' Index='1' Id='" + sampleObjects[0]["ID"].ToString() + "'><img alt='Samples of PowerPoint® pitch books created for investment management firms using Assette cloud-based application.'  class='PreviewImage urlImageSmall' src='images/Investment-management-pitch-book-examples.png' width='272' height='200'  border='0' style='float: left; padding: 4px 0px' /></a>";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void GetFirstTemplateDesignsForPreView()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllTemplateDesignsForPreView();

                JArray templateObjects = JArray.Parse(rtnVal);

                if (templateObjects.Count > 0)
                {
                    DateTime date = DateTime.Parse(templateObjects[0]["UploadedDate"].ToString());

                    string newDate = String.Format("{0:yyyyMMddHHmmss}", date);

                    //DesignsPreView.InnerHtml = "<a href='#' class='ViewLibraryTemplates' Index='1' Id='" + templateObjects[0]["ID"].ToString() + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + templateObjects[0]["ID"].ToString() + "&type=t_t&date=" + newDate + "&uniqueindex=0' width='140' height='90'  border='0' style='float: left;' /></a>";
                    DesignsPreView.InnerHtml = "<a href='#' class='ViewLibraryTemplates' Index='1' Id='" + templateObjects[0]["ID"].ToString() + "'><img alt='Samples of PowerPoint® investment performance reports created with Assette cloud-based application.' class='urlImageSmall' src='images/pitchbook-examples.png' width='140' height='90'  border='0' style='float: left;' /></a>";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        // working one
        public void GetFirstAObjectsForPreView()
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetAllPAObjectsForPreView();

                JArray reportObjects = JArray.Parse(rtnVal);

                if (reportObjects.Count > 0)
                {
                    DateTime date = DateTime.Parse(reportObjects[0]["ModifiedDate"].ToString());

                    string newDate = String.Format("{0:yyyyMMddHHmmss}", date);

                    if (reportObjects[0]["ObjectTableType"].ToString() == "p")
                    {
                        //ReportObjectsPreView.InnerHtml = "<a href='#' TableType='" + reportObjects[0]["ObjectTableType"].ToString() + "' class='ViewLibraryReportObject' Index='1' Id='" + reportObjects[0]["ID"].ToString() + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + reportObjects[0]["ID"].ToString() + "&type=r_t&date=" + newDate + "&uniqueindex=0' width='140' height='90'  border='0' style='float: left;padding:4px 2px' /></a>";
                        ReportObjectsPreView.InnerHtml = "<a href='#' TableType='" + reportObjects[0]["ObjectTableType"].ToString() + "' class='ViewLibraryReportObject' Index='1' Id='" + reportObjects[0]["ID"].ToString() + "'><img alt='Samples of .PPT pitch books created for institutional investment management firms using Assette cloud-based application.' class='urlImageSmall' src='images/investment-performance-report-example.png' width='140' height='90'  border='0' style='float: left;padding:4px 2px' /></a>";
                    }
                    else
                    {
                        //ReportObjectsPreView.InnerHtml = "<a href='#' TableType='" + reportObjects[0]["ObjectTableType"].ToString() + "' class='ViewLibraryReportObject' Index='1' Id='" + reportObjects[0]["ID"].ToString() + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + reportObjects[0]["ID"].ToString() + "&type=m_t&date=" + newDate + "&uniqueindex=0' width='140' height='90'  border='0' style='float: left;padding:4px 2px' /></a>";
                        ReportObjectsPreView.InnerHtml = "<a href='#' TableType='" + reportObjects[0]["ObjectTableType"].ToString() + "' class='ViewLibraryReportObject' Index='1' Id='" + reportObjects[0]["ID"].ToString() + "'><img alt='Samples of .PPT pitch books created for institutional investment management firms using Assette cloud-based application.' class='urlImageSmall' src='images/investment-performance-report-example.png' width='140' height='90'  border='0' style='float: left;padding:4px 2px' /></a>";
                    }

                    //SamplesPreView.InnerHtml = "<a href='#' class='ViewLibrarySamples' Index='1' Id='" + sampleObjects[0]["ID"].ToString() + "'><img  class='PreviewImage urlImageSmall' src='ImageHandler.ashx?id=" + sampleObjects[0]["ID"].ToString() + "&type=s_t&date=" + newDate + "&uniqueindex=0' width='140' height='90'  border='0' style='float: left;padding:4px 2px' /></a>";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        #endregion
 
    }
}