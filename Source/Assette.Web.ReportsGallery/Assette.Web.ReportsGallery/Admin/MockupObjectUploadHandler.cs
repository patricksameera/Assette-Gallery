using System;
using System.IO;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;
using System.Collections.Generic;
using System.Collections;

namespace Assette.Web.ReportsGallery.Admin
{
    public class MockupObjectUploadHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MockupObjectUploadHandler));

        public void ProcessRequest(HttpContext context)
        {
            BusinessProxy proxy = new BusinessProxy();

            int ImageWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ImageWidth"]);
            int ImageHeight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ImageHeight"]);
            int ThumbnailWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThumbnailWidth"]);
            int ThumbnailHeight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThumbnailHeight"]);

            string message = string.Format("MockupObjectUploadHandler Parameters from web config = ImageWidth: {0}, ImageHeight: {1}, ThumbnailWidth: {2}, ThumbnailHeight: {3}", ImageWidth.ToString(), ImageHeight.ToString(), ThumbnailWidth.ToString(), ThumbnailHeight.ToString());
            Log.Info(message);

            // get file name
            String filename = HttpContext.Current.Request.Headers["X-File-Name"];

            if (string.IsNullOrEmpty(filename) && HttpContext.Current.Request.Files.Count <= 0)
            {
                message = string.Format("MockupObjectUploadHandler: no files passed");
                Log.Error(message);

                context.Response.Write("{ \"success\": false }");
            }
            else
            {
                if (filename == null)
                {
                    message = string.Format("MockupObjectUploadHandler - browser: IE");
                    Log.Info(message);

                    //This work for IE 
                    HttpPostedFile uploadedfile = context.Request.Files[0];
                    Stream inputStream = uploadedfile.InputStream;

                    /*
                    byte[] fileData = null;

                    using (var binaryReader = new BinaryReader(inputStream))
                    {
                        fileData = binaryReader.ReadBytes(uploadedfile.ContentLength);
                    }
                    */

                    int read;

                    // convert stream to array
                    byte[] buffer = new byte[16 * 1024];

                    MemoryStream memoryStream = new MemoryStream();

                    while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                    }

                    byte[] fileData = memoryStream.ToArray();


                    message = string.Format("MockupObjectUploadHandler IE Parameters = contentLength: {0}, contentType: {1}, fileName: {2}", uploadedfile.ContentLength, uploadedfile.ContentType, uploadedfile.FileName);
                    Log.Info(message);

                    // insert record
                    bool rtnVal = proxy.SavePAObjectMockups(fileData, Guid.NewGuid(), ImageWidth, ImageHeight, ThumbnailWidth, ThumbnailHeight);

                    if (rtnVal == true)
                    {
                        message = string.Format("MockupObjectUploadHandler - browser: IE file upload successful.");
                        Log.Info(message);

                        // ajax - success
                        context.Response.Write("{ \"success\": true }");
                        context.Response.End();
                    }
                    else
                    {
                        message = string.Format("MockupObjectUploadHandler - browser: IE file upload unsuccessful.");
                        Log.Info(message);

                        // ajax - error
                        context.Response.Write("{ \"success\": false }");
                        context.Response.End();
                    }
                }
                else
                {
                    message = string.Format("MockupObjectUploadHandler - browser: FF/Chrome ");
                    Log.Info(message);

                    message = string.Format("MockupObjectUploadHandler FF/Chrome Parameters = filename: {0}", filename);
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

                    byte[] fileData = memoryStream.ToArray();

                    // insert record
                    bool rtnVal = proxy.SavePAObjectMockups(fileData, Guid.NewGuid(), ImageWidth, ImageHeight, ThumbnailWidth, ThumbnailHeight);

                    // close streams
                    inputStream.Close();
                    memoryStream.Close();

                    if (rtnVal == true)
                    {
                        message = string.Format("MockupObjectUploadHandler - browser: FF/Chrome file upload successful.");
                        Log.Info(message);

                        // ajax - success
                        context.Response.Write("{ \"success\": true }");
                        context.Response.End();
                    }
                    else
                    {
                        message = string.Format("MockupObjectUploadHandler - browser: FF/Chrome file upload unsuccessful.");
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