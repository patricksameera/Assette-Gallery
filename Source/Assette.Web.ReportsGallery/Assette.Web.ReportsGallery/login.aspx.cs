using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using Assette.ReportGallery.Proxy;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assette.Web.ReportsGallery
{
    [SerializableAttribute]
    public partial class login : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(login));
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientBasePage.GenerateApplicationErrorForTesting();

            //ClientBasePage.GenerateExceptionForTesting();

            if (!IsPostBack)
            {
                // set version number
                //SetReportGalleryVersion();

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

            // set page
            if (Request.QueryString["page"] != null)
            {
                hdnPage.Value = Request.QueryString["page"];
            }
            else
            {
                hdnPage.Value = "client-welcome.aspx";              
            }  
        }

        [System.Web.Services.WebMethod]
        public static string CreateNewUser(string firstName, string lastName, string jobTitle, string email, string firmName, string title, string frimAum, string firmType)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                string emailExistenceRtnVal = proxy.CheckEmailExistence(email);

                if (emailExistenceRtnVal != "1")
                {
                    rtnVal = proxy.CreateUser(firstName, lastName, jobTitle, email, firmName, GetIPAddress(), title, frimAum, firmType);

                    // hubspot registration
                    CreateNewContactInHubSpot(firstName, lastName, jobTitle, email, firmName, title, frimAum, firmType);

                    // send email on client registration
                    SendEmailOnClientRegistration(email);  
                }
                else
                {
                    // user already exists. login straight away! - no need to add the record
                    rtnVal = "1"; 
                }

                // if user is exists, add user to session
                if (rtnVal == "1")
                {
                    string jsonEntUser = proxy.GetUserDetailsByEmail(email);

                    JArray userObjects = JArray.Parse(jsonEntUser);

                    // add session
                    foreach (var userObject in userObjects)
                    {
                        HttpContext.Current.Session["UserId"] = userObject["Id"].ToString();
                        HttpContext.Current.Session["Email"] = userObject["Email"].ToString();
                        HttpContext.Current.Session["Name"] = userObject["FirstName"].ToString();

                        break;
                    }

                    // add cookie
                    string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];

                    HttpCookie myCookie = new HttpCookie(cookieName);
                    myCookie.Value = email;
                    myCookie.Expires = DateTime.Now.AddDays(365d);
                    HttpContext.Current.Response.Cookies.Add(myCookie);

                    HttpCookie newCookie = new HttpCookie(cookieName);
                    newCookie = HttpContext.Current.Request.Cookies[cookieName];
                    string cookieValue = newCookie.Value;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        public static void SendEmailOnClientRegistration(string email)
        {
            // send confirmation email to client
            string sendEmailOnClientRegistrationToClient = System.Configuration.ConfigurationManager.AppSettings["SendEmailOnClientRegistrationToClient"];

            if (sendEmailOnClientRegistrationToClient == "1")
            {
                string template = System.Configuration.ConfigurationManager.AppSettings["ClientRegistrationEmailTemplateToClient"];

                SendMailOnClientRegistration(email, template, "client");
            }

            // send confirmation email to admin
            string sendEmailOnClientRegistrationToAdmin = System.Configuration.ConfigurationManager.AppSettings["SendEmailOnClientRegistrationToAdmin"];

            if (sendEmailOnClientRegistrationToAdmin == "1")
            {
                string template = System.Configuration.ConfigurationManager.AppSettings["ClientRegistrationEmailTemplateToAdmin"];
                
                SendMailOnClientRegistration(email, template, "admin");
            }
        }

        [System.Web.Services.WebMethod]
        public static string CheckEmailExistence(string email)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            //ClientBasePage.GenerateJqueryErrorForTesting();

            try
            {
                rtnVal = proxy.CheckEmailExistence(email);

                if (rtnVal == "1")
                {
                    string jsonEntUser = proxy.GetUserDetailsByEmail(email);

                    JArray userObjects = JArray.Parse(jsonEntUser);

                    // add session
                    foreach (var userObject in userObjects)
                    {
                        HttpContext.Current.Session["UserId"] = userObject["Id"].ToString();
                        HttpContext.Current.Session["Email"] = userObject["Email"].ToString();
                        HttpContext.Current.Session["Name"] = userObject["FirstName"].ToString();

                        break;
                    }

                    // get cookie name
                    string cookieName = System.Configuration.ConfigurationManager.AppSettings["CookieName"];

                    // add cookie
                    HttpCookie myCookie = new HttpCookie(cookieName);
                    myCookie.Value = email;
                    myCookie.Expires = DateTime.Now.AddDays(365d);
                    HttpContext.Current.Response.Cookies.Add(myCookie);

                    // check cookie
                    HttpCookie newCookie = new HttpCookie(cookieName);
                    newCookie = HttpContext.Current.Request.Cookies[cookieName];
                    string cookieValue = newCookie.Value;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string GetUserDetailsByEmail(string email)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                rtnVal = proxy.GetUserDetailsByEmail(email);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static string SendMail(string emailTo, string emailFrom, string emailMessage)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            string smtpServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];

            try
            {
                rtnVal = proxy.SendMail(emailTo, emailFrom, emailMessage, smtpServer);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        [System.Web.Services.WebMethod]
        public static void ClientSideLogging(string message)
        {
            Log.Error(message);
        }

        public static string GetIPAddress()
        {
            string IPAddress = string.Empty;

            // method 01
            //string hostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(hostName);
            //IPAddress[] addr = ipEntry.AddressList;
            //IPAddress = addr[addr.Length - 1].ToString();

            // method 02
            string ipList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                IPAddress = ipList.Split(',')[0];
            }
            else
            {
                IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return IPAddress;
        }

        public void SetReportGalleryVersion()
        {
            //Assembly assembly_web = Assembly.LoadFrom("Assette.Web.ReportsGallery.dll");
            //string dllInfo = string.Format("{0}: {1}</br>{2}: {3}</br>{4}: {5}", fileName, fileInfo.FileVersion, fileInfo.FileName, fileInfo.FileVersion, fileInfo.FileName, fileInfo.FileVersion);

            string webDllPath = string.Format("{0}.dll", Assembly.GetExecutingAssembly().Location);

            // development purpose
            webDllPath = @"G:\Report Gallery - Versions\Sam Repo\Source\Assette.Web.ReportsGallery\Assette.Web.ReportsGallery\bin\Assette.Web.ReportsGallery.dll";
            
            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(webDllPath);

            string webDllInfo = string.Format("{0} : v{1}", Path.GetFileName(fileInfo.FileName), fileInfo.FileVersion);

            ReportsGalleryVersion.InnerText = webDllInfo;

            Log.Info(webDllInfo);
        }

        public static void SendMailOnClientRegistration(string clientEmail, string templateName, string type)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            string smtpServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];
            string emailFrom = System.Configuration.ConfigurationManager.AppSettings["ClientRegistrationEmailFrom"];
            string isBodyHTML = System.Configuration.ConfigurationManager.AppSettings["ClientRegistrationEmailIsBodyHTML"];
            string subject = System.Configuration.ConfigurationManager.AppSettings["ClientRegistrationEmailSubject"];
            string emailImageEmbed = System.Configuration.ConfigurationManager.AppSettings["EmailImageEmbed"];
            string adminEmail = System.Configuration.ConfigurationManager.AppSettings["ClientRegistrationEmailAdminTo"];

            try
            {
                rtnVal = proxy.SendMailOnClientRegistration(clientEmail, smtpServer, emailFrom, isBodyHTML, subject, templateName, emailImageEmbed, adminEmail, type);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static bool CreateNewContactInHubSpot(string firstName, string lastName, string job_function, string email, string firm_name, string title, string firm_aum, string firm_type)
        {
            Dictionary<string, string> dictFormValues = new Dictionary<string, string>();
            dictFormValues.Add("firstname", firstName);
            dictFormValues.Add("lastname", lastName);
            dictFormValues.Add("email", email);
            dictFormValues.Add("jobtitle", title);
            dictFormValues.Add("job_function", job_function);
            dictFormValues.Add("company", firm_name);
            dictFormValues.Add("firm_aum_tier", firm_aum);
            dictFormValues.Add("firm_type", firm_type);
            dictFormValues.Add("message", "N/A");
            dictFormValues.Add("hear_about_assette", "N/A");

            // Form Variables (from the HubSpot Form Edit Screen)
            int intPortalID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["HubSpotPortalId"]); ; // YOUR PORTAL ID GOES HERE
            string strFormGUID = System.Configuration.ConfigurationManager.AppSettings["HubSpotFormGuid"]; // YOUR FORM ID GOES HERE

            // Tracking Code Variables
            string strHubSpotUTK = System.Web.HttpContext.Current.Request.Cookies["hubspotutk"].Value;
            string strIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            // Page Variables
            string strPageTitle = "Register for Gallery | Assette";
            string strPageURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

            // Do the post, returns true/false
            string strError = "";

            // blnRet = true;
            bool blnRet = Post_To_HubSpot_FormsAPI(intPortalID, strFormGUID, dictFormValues, strHubSpotUTK, strIpAddress, strPageTitle, strPageURL, ref strError);

            return blnRet;
        }

        public static bool Post_To_HubSpot_FormsAPI(int intPortalID, string strFormGUID, Dictionary<string, string> dictFormValues, string strHubSpotUTK, string strIpAddress, string strPageTitle, string strPageURL, ref string strMessage)
        {
            // Build Endpoint URL
            string strEndpointURL = string.Format("https://forms.hubspot.com/uploads/form/v2/{0}/{1}", intPortalID, strFormGUID);

            // Setup HS Context Object
            Dictionary<string, string> hsContext = new Dictionary<string, string>();
            hsContext.Add("hutk", strHubSpotUTK);
            hsContext.Add("ipAddress", strIpAddress);
            hsContext.Add("pageUrl", strPageURL);
            hsContext.Add("pageName", strPageTitle);

            // Serialize HS Context to JSON (string)
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            string strHubSpotContextJSON = json.Serialize(hsContext);

            // Create string with post data
            string strPostData = "";

            // Add dictionary values
            foreach (var d in dictFormValues)
            {
                strPostData += d.Key + "=" + System.Web.HttpContext.Current.Server.UrlEncode(d.Value) + "&";
            }

            // Append HS Context JSON
            strPostData += "hs_context=" + System.Web.HttpContext.Current.Server.UrlEncode(strHubSpotContextJSON);

            // Create web request object
            System.Net.HttpWebRequest r = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strEndpointURL);

            // Set headers for POST
            r.Method = "POST";
            r.ContentType = "application/x-www-form-urlencoded";
            r.ContentLength = strPostData.Length;
            r.KeepAlive = false;

            // POST data to endpoint
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(r.GetRequestStream()))
            {
                try
                {
                    sw.Write(strPostData);
                }
                catch (Exception ex)
                {
                    // POST Request Failed
                    strMessage = ex.Message;
                    return false;
                }
            }

            return true; //POST Succeeded
        }
    }
}