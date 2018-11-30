using System;
using System.IO;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;

namespace Assette.Web.ReportsGallery.Admin
{
    public class TemplateDesignUploadHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TemplateDesignUploadHandler));

        public void ProcessRequest(HttpContext context)
        {
            BusinessProxy proxy = new BusinessProxy();

            int ImageWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ImageWidth"]);
            int ImageHeight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ImageHeight"]);
            int ThumbnailWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThumbnailWidth"]);
            int ThumbnailHeight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThumbnailHeight"]);

            string message = string.Format("TemplateDesignUploadHandler Parameters from web config = ImageWidth: {0}, ImageHeight: {1}, ThumbnailWidth: {2}, ThumbnailHeight: {3}", ImageWidth.ToString(), ImageHeight.ToString(), ThumbnailWidth.ToString(), ThumbnailHeight.ToString());
            Log.Info(message);

            string name = HttpContext.Current.Request["name"];
            string shortDescription = HttpContext.Current.Request["shortdescription"];
            string longDescription = HttpContext.Current.Request["longdescription"];
            string product = HttpContext.Current.Request["product"];
            string firm = HttpContext.Current.Request["firm"];
            string preview = HttpContext.Current.Request["preview"];

            message = string.Format("TemplateDesignUploadHandler Parameters = name: {0}, shortDescription: {1}, longDescription: {2}, product: {3}, firm: {4}, preview: {5}", name, shortDescription, longDescription, product, firm, preview);
            Log.Info(message);

            // get file name
            String filename = HttpContext.Current.Request.Headers["X-File-Name"];

            if (string.IsNullOrEmpty(filename) && HttpContext.Current.Request.Files.Count <= 0)
            {
                message = string.Format("TemplateDesignUploadHandler: no files passed");
                Log.Error(message);

                context.Response.Write("{ \"success\": false }");
            }
            else
            {
                if (filename == null)
                {
                    message = string.Format("TemplateDesignUploadHandler - browser: IE");
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

                    message = string.Format("TemplateDesignUploadHandler IE Parameters = contentLength: {0}, contentType: {1}, fileName: {2}", uploadedfile.ContentLength, uploadedfile.ContentType, uploadedfile.FileName);
                    Log.Info(message);

                    // insert record
                    bool rtnVal = proxy.SaveTemplateDesigns(PowerPointFile, name, shortDescription, longDescription, Guid.NewGuid(), product, firm, ImageWidth, ImageHeight, ThumbnailWidth, ThumbnailHeight, preview);

                    if (rtnVal == true)
                    {
                        message = string.Format("TemplateDesignUploadHandler - browser: IE file upload successful.");
                        Log.Info(message);

                        // ajax - success
                        context.Response.Write("{ \"success\": true }");
                        context.Response.End();
                    }
                    else
                    {
                        message = string.Format("TemplateDesignUploadHandler - browser: IE file upload unsuccessful.");
                        Log.Info(message);

                        // ajax - error
                        context.Response.Write("{ \"success\": false }");
                        context.Response.End();
                    }
                }
                else
                {
                    message = string.Format("TemplateDesignUploadHandler - browser: FF/Chrome ");
                    Log.Info(message);

                    message = string.Format("TemplateDesignUploadHandler FF/Chrome Parameters = filename: {0}", filename);
                    Log.Info(message);

                    //This work for Firefox and Chrome. 
                    Stream inputStream = HttpContext.Current.Request.InputStream;

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

                    // insert record
                    bool rtnVal = proxy.SaveTemplateDesigns(PowerPointFile, name, shortDescription, longDescription, Guid.NewGuid(), product, firm, ImageWidth, ImageHeight, ThumbnailWidth, ThumbnailHeight, preview);

                    if (rtnVal == true)
                    {
                        message = string.Format("TemplateDesignUploadHandler - browser: FF/Chrome file upload successful.");
                        Log.Info(message);

                        // ajax - success
                        context.Response.Write("{ \"success\": true }");
                        context.Response.End();
                    }
                    else
                    {
                        message = string.Format("TemplateDesignUploadHandler - browser: FF/Chrome file upload unsuccessful.");
                        Log.Info(message);

                        // ajax - error
                        context.Response.Write("{ \"success\": false }");
                        context.Response.End();
                    }
                }
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
