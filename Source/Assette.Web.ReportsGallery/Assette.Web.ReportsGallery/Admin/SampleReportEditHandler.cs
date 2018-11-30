using System;
using System.IO;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public class SampleReportEditHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SampleReportEditHandler));

        public void ProcessRequest(HttpContext context)
        {
            BusinessProxy proxy = new BusinessProxy();

            int ImageWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ImageWidth"]);
            int ImageHeight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ImageHeight"]);
            int ThumbnailWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThumbnailWidth"]);
            int ThumbnailHeight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThumbnailHeight"]);

            string message = string.Format("SampleReportEditHandler Parameters from web config = ImageWidth: {0}, ImageHeight: {1}, ThumbnailWidth: {2}, ThumbnailHeight: {3}", ImageWidth.ToString(), ImageHeight.ToString(), ThumbnailWidth.ToString(), ThumbnailHeight.ToString());
            Log.Info(message);

            int Id = Int32.Parse(HttpContext.Current.Request.Params["id"]);
            HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;

            message = string.Format("SampleReportEditHandler Parameters = id: {0}, browser: {1}", Id, browser.Browser.ToLower());
            Log.Info(message);

            if (browser.Browser.ToLower() == "ie")
            {
                message = string.Format("SampleReportEditHandler - browser: IE");
                Log.Info(message);

                //This work for IE 
                HttpPostedFile uploadedfile = context.Request.Files[0];
                Stream inputStream = uploadedfile.InputStream;

                int read;

                // convert stream to array
                byte[] buffer = new byte[16 * 1024];

                MemoryStream memoryStream = new MemoryStream();

                while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, read);
                }

                byte[] PowerPointFile = memoryStream.ToArray();

                // close streams
                inputStream.Close();
                memoryStream.Close();

                message = string.Format("SampleReportEditHandler IE Parameters = contentLength: {0}, contentType: {1}, fileName: {2}", uploadedfile.ContentLength, uploadedfile.ContentType, uploadedfile.FileName);
                Log.Info(message);

                // edit record
                string rtnVal = proxy.EditSampleReportUploadedFile(Id, PowerPointFile, ImageWidth, ImageHeight, ThumbnailWidth, ThumbnailHeight);

                message = string.Format("SampleReportEditHandler IE Return Value: {0}", rtnVal);
                Log.Info(message);

                // ajax - success
                context.Response.Write("{ \"success\": true }");
                context.Response.End();
            }
            else
            {
                message = string.Format("SampleReportEditHandler - browser: FF/Chrome file upload successful.");
                Log.Info(message);

                //This work for Firefox and Chrome. 
                HttpPostedFile postedfile = HttpContext.Current.Request.Files.Get(0) as HttpPostedFile;
                Stream inputStream  = postedfile.InputStream;

                message = string.Format("SampleReportEditHandler FF/Chrome Parameters = filename: {0}", postedfile.FileName);
                Log.Info(message);

                int read;

                // convert stream to array
                byte[] buffer = new byte[16 * 1024];

                MemoryStream memoryStream = new MemoryStream();

                while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, read);
                }

                byte[] PowerPointFile = memoryStream.ToArray();

                // close streams
                inputStream.Close();
                memoryStream.Close();

                // edit record
                string rtnVal = proxy.EditSampleReportUploadedFile(Id, PowerPointFile, ImageWidth, ImageHeight, ThumbnailWidth, ThumbnailHeight);

                message = string.Format("SampleReportEditHandler FF/Chrome Return Value: {0}", rtnVal);
                Log.Info(message);

                // ajax - success
                context.Response.Write("{ \"success\": true }");
                context.Response.End();
            }
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
