using System;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public class ImageHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ImageHandler));

        public void ProcessRequest(HttpContext context)
        {
            string ID = context.Request.QueryString["id"];
            string Type = context.Request.QueryString["type"];
            string PageNo = context.Request.QueryString["page"];

            if (PageNo == null)
            {
                PageNo = "No page number passed";
            }

            string message = string.Format("Admin ImageHandler.ashx ProcessRequest() Parameters = ID: {0}, Type: {1}, PageNo: {2}", ID, Type, PageNo);
            Log.Info(message);

            try
            {
                var proxy = new BusinessProxy();
                byte[] ImageData = proxy.GetImage(ID, Type, PageNo);

                context.Response.ContentType = "image/jpg";
                context.Response.OutputStream.Write(ImageData, 0, ImageData.Length);
            }
            catch (Exception ex)
            {
                Log.Error("Admin ImageHandler.ashx ProcessRequest()", ex);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}