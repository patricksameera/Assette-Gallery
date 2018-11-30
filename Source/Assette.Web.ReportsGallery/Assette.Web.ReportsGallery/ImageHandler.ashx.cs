using System;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;
using System.Globalization;
using System.Drawing;
using System.Collections.Specialized;
using System.Reflection;

namespace Assette.Web.ReportsGallery
{
    public class ImageHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ImageHandler));

        /*
        //solution : 01      
        public void ProcessRequest(HttpContext context)
        {
            string ID = context.Request.QueryString["id"];
            string Type = context.Request.QueryString["type"];

            string message = string.Format("Client ImageHandler.ashx ProcessRequest() Parameters = ID: {0}, Type: {1}", ID, Type);
            Log.Info(message);

            int CacheInMinutes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CacheInMinutes"].ToString());

            // get from database
            try
            {
                var proxy = new BusinessProxy();
                byte[] ImageData = proxy.GetImage(ID, Type);

                // cache data
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(CacheInMinutes)); 

                // write the image to placeholder
                context.Response.ContentType = "image/jpg";
                context.Response.OutputStream.Write(ImageData, 0, ImageData.Length);   
            }
            catch (Exception ex)
            {
                Log.Error("Client ImageHandler.ashx ProcessRequest()", ex);
            }
        }*/
        
        /*
        // solution : 02
        public void ProcessRequest(HttpContext context)
        {
            string ID = context.Request.QueryString["id"];
            string Type = context.Request.QueryString["type"];
            string Date = context.Request.QueryString["date"];

            string message = string.Format("Client ImageHandler.ashx ProcessRequest() Parameters = ID: {0}, Type: {1}, Date: {2}", ID, Type, Date);
            Log.Info(message);

            int CacheInMinutes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CacheInMinutes"].ToString());

            try
            {
                string cacheKey = Type + "_" + ID + "_" + Date;

                if (context.Cache[cacheKey] == null)
                {
                    message = string.Format("Getting image data from database - ImageHandler.ashx ProcessRequest() Parameters = ID: {0}, Type: {1}, Date: {2}, CacheKey: {3}", ID, Type, Date, cacheKey);
                    Log.Info(message);

                    // get image from database
                    var proxy = new BusinessProxy();
                    byte[] ImageData = proxy.GetImage(ID, Type);

                    // cache data
                    context.Cache.Insert(cacheKey, ImageData, null, DateTime.Now.AddMinutes(CacheInMinutes), new TimeSpan(0, 0, 0));

                    // write the image to placeholder
                    context.Response.ContentType = "image/jpg";
                    context.Response.OutputStream.Write(ImageData, 0, ImageData.Length);
                }
                else
                {
                    message = string.Format("Getting image data from cache - ImageHandler.ashx ProcessRequest() Parameters = ID: {0}, Type: {1}, Date: {2}, CacheKey: {3}", ID, Type, Date, cacheKey);
                    Log.Info(message);

                    // get image from cache
                    byte[] CacheImageData = (byte[])context.Cache[cacheKey];

                    // write the image to placeholder
                    context.Response.ContentType = "image/jpg";
                    context.Response.OutputStream.Write(CacheImageData, 0, CacheImageData.Length);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Client ImageHandler.ashx ProcessRequest()", ex);
            }
        }
        */

        // solution : 03
        public void ProcessRequest(HttpContext context)
        {
            string ID = context.Request.QueryString["id"];
            string Type = context.Request.QueryString["type"];
            string Date = context.Request.QueryString["date"];
            string PageNo = context.Request.QueryString["page"];

            if (PageNo == null)
            {
                PageNo = "No page number passed";
            }

            int CacheInHours = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CacheInHours"].ToString());
            int CacheInMinutes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CacheInMinutes"].ToString());
            int CacheInSeconds = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CacheInSeconds"].ToString());

            int Year = int.Parse(Date.Substring(0, 4));
            int Month = int.Parse(Date.Substring(4, 2));
            int Day = int.Parse(Date.Substring(6, 2));
            int Hour = int.Parse(Date.Substring(8, 2));
            int Minute = int.Parse(Date.Substring(10, 2));
            int Seconds = int.Parse(Date.Substring(12, 2));

            DateTime LastModifiedDate = new DateTime(Year, Month, Day, Hour, Minute, Seconds).AddDays(-1).ToUniversalTime();

            string rawIfModifiedSince = context.Request.Headers.Get("If-Modified-Since");

            // cache properties
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(CacheInHours, CacheInMinutes, CacheInSeconds));

            if (string.IsNullOrEmpty(rawIfModifiedSince))
            {
                string message = string.Format("Getting image data from database - ImageHandler.ashx ProcessRequest() Parameters = ID: {0}, Type: {1}, Date: {2}, PageNo: {3}", ID, Type, Date, PageNo);
                Log.Info(message);

                // set last modified time
                context.Response.Cache.SetLastModified(LastModifiedDate); 
            }
            else
            {
                string message = string.Format("Getting image data from cache - ImageHandler.ashx ProcessRequest() Parameters = ID: {0}, Type: {1}, Date: {2}, PageNo: {3}", ID, Type, Date, PageNo);
                Log.Info(message);

                DateTime ifModifiedSince = DateTime.Parse(rawIfModifiedSince).AddDays(-1).ToUniversalTime();
                
                // http does not provide milliseconds, so remove it from the comparison
                if (LastModifiedDate.AddMilliseconds(-LastModifiedDate.Millisecond) == ifModifiedSince)
                {
                    // The requested file has not changed
                    context.Response.StatusCode = 304;
                    context.Response.StatusDescription = "Not Modified";

                    return;
                }
            }

            // get data from database
            try
            {
                var proxy = new BusinessProxy();
                byte[] ImageData = proxy.GetImage(ID, Type, PageNo);

                // write the image to placeholder
                context.Response.ContentType = "image/jpg";
                context.Response.OutputStream.Write(ImageData, 0, ImageData.Length);
                context.Response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                Log.Error("Client ImageHandler.ashx ProcessRequest()", ex);
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}