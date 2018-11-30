using System;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;
using System.IO;

namespace Assette.Web.ReportsGallery.Admin
{
    public class FileDownloadHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileDownloadHandler));

        public void ProcessRequest(HttpContext context)
        {
            BusinessProxy proxy = new BusinessProxy();

            string Id = HttpContext.Current.Request.Params["id"];
            string type = HttpContext.Current.Request.Params["type"];

            byte[] rtnValue = proxy.DownLoadFile(Id, type);

            string fileName = string.Empty;

            if (type == "s")
            {
                fileName = "Sample_Report_" + Id + ".ppt";
            }
            else
            {
                fileName = "Template_Design_" + Id + ".ppt";
            }

            context.Response.Clear();
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            context.Response.ContentType = "application/ppt";

            MemoryStream stream = new MemoryStream(rtnValue);
            context.Response.Buffer = true;
            stream.WriteTo(context.Response.OutputStream);

            context.Response.End();
            context.Response.Flush();
        }

        private bool IsIE9(HttpRequest request)
        {
            return request["qqfile"] == null;
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}