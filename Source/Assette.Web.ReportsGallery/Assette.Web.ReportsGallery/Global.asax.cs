using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Assette.ReportGallery.Proxy;
using log4net;
using log4net.Config;
using log4net.Util;

namespace Assette.Web.ReportsGallery
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            LogLog.InternalDebugging = true;

            var log4NetConfigFilePath = string.Format("{0}.log4net", Assembly.GetExecutingAssembly().Location);

            // development purpose
            //log4NetConfigFilePath = @"G:\Report Gallery - Versions\Sam Repo\Source\Assette.Web.ReportsGallery\Assette.Web.ReportsGallery\Assette.Web.ReportsGallery.dll.log4net";

            XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFilePath));
            BasicConfigurator.Configure();

            // write log4net path
            if (System.Configuration.ConfigurationManager.AppSettings["WriteToLog4netLogTest"] == "1")
            {
                Log.Info("Assette.Web.ReportsGallery.dll.log4net: " + log4NetConfigFilePath);
                WriteToTextFile(log4NetConfigFilePath);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            BusinessProxy proxy = new BusinessProxy();

            // logging - log4net /////////////////////////////////////////////////////////////////////////////////////////
            Exception ex = Server.GetLastError().GetBaseException();

            if (ex is System.Web.HttpException)
            {
                var filePath = Context.Request.FilePath;
                var url = ((HttpApplication)sender).Context.Request.Url;

                Log.Error("Application Error --> URL: " + url + "; FilePath: " + filePath, ex);
            }
            else
            {
                Log.Error("Application Error -->", ex);
            }

            // logging - event log /////////////////////////////////////////////////////////////////////////////////////////
            string errorDescription = Server.GetLastError().ToString();

            // logging on EventLog
            if (System.Configuration.ConfigurationManager.AppSettings["WriteToEventLog"].ToString() == "1")
            {           
                string eventLogName = "AssetteReportsGallery";

                if (!EventLog.SourceExists(eventLogName))
                {
                    EventLog.CreateEventSource(eventLogName, eventLogName);
                }

                EventLog eventLog = new EventLog();
                eventLog.Source = eventLogName;
                eventLog.WriteEntry(errorDescription, EventLogEntryType.Error);
            }

            // add error to cache
            if (System.Configuration.ConfigurationManager.AppSettings["WriteToErrorPage"].ToString() == "1")
            {
                HttpContext.Current.Cache["Error"] = errorDescription; 
            }

            // clear error /////////////////////////////////////////////////////////////////////////////////////////
            Context.ClearError();

            // re-direct to error page
            string fullOrigionalPath = Request.Url.ToString();

            if (fullOrigionalPath.ToLower().Contains("admin"))
            {
                Response.Redirect("/error.aspx");
            }
            else
            {
                Response.Redirect("error.aspx");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["UserId"]).Length > 0)
            {
                Session["UserId"] = null;
                Session["Email"] = null;
                Session["Name"] = null;
                Session["Error"] = null;
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private void RegisterRoutes(RouteCollection routes)
        {

        }

        public static void WriteToTextFile(string message)
        {
            string path = @"C:\Assette Logs\AssetteReportsGallery_log4net.txt";

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
        }     
    }
}