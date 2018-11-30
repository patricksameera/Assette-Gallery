using System;
using Assette.ReportGallery.Proxy;
using log4net;
using System.Net;

namespace Assette.Web.ReportsGallery.Admin
{
    public partial class create_user : AdminBasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(create_user));

        protected void Page_Load(object sender, EventArgs e)
        {
            //GenerateApplicationErrorForTesting();

            //GenerateExceptionForTesting();

            // check user session availability
            CheckUserSession();

            // set log-in log-out button visibility
            SetLogInLogOutButtonVisibility();      
        }

        [System.Web.Services.WebMethod]
        public static string CreateNewUser(string firstName, string lastName, string jobTitle, string email, string company, string title, string frimAum, string firmType)
        {
            string rtnVal = string.Empty;

            var proxy = new BusinessProxy();

            try
            {
                //GenerateJqueryErrorForTesting();

                string emailExistenceRtnVal = proxy.CheckEmailExistence(email);

                if (emailExistenceRtnVal != "1")
                {
                    rtnVal = proxy.CreateUser(firstName, lastName, jobTitle, email, company, GetIPAddress(), title, frimAum, firmType);
                }
                else
                {
                    // user already exists. login straight away! - no need to add the record
                    rtnVal = "1";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return rtnVal;
        }

        public static string GetIPAddress()
        {
            string IPAddress = string.Empty;

            string hostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(hostName);
            IPAddress[] addr = ipEntry.AddressList;
            IPAddress = addr[addr.Length - 1].ToString();

            return IPAddress;
        }
    }
}