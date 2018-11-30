using System;
using System.IO;
using System.Reflection;
using System.Web.Security;
using Assette.ReportGallery.Data;
using log4net;
using log4net.Config;
using log4net.Util;


#if !DEBUG

using System.EnterpriseServices;
using System.Runtime.InteropServices;

#endif

namespace Assette.ReportGallery.Proxy
{
#if DEBUG

    public class BusinessProxy : IBusinessProxy

#else

    [ClassInterface(ClassInterfaceType.None)]
    public class BusinessProxy : ServicedComponent, IBusinessProxy

#endif
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BusinessProxy));

        public BusinessProxy()
        {
          
        }

        static BusinessProxy()
        {
            LogLog.InternalDebugging = true;

            string log4netConfigFilePath = string.Format("{0}.log4net", Assembly.GetExecutingAssembly().Location);

            XmlConfigurator.ConfigureAndWatch(new FileInfo(log4netConfigFilePath));
        }

        public string GetConnectionString()
        {
            var ret = DataManager.GetConnectionString();

            return ret;
        }

        public string CheckHeartBeat()
        {
            return DataManager.CheckHeartBeat();
        }

        public string CreateUser(string firstName, string lastName, string jobTitle, string email, string company, string registeredIP, string title, string frimAum, string firmType)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.CreateUser(firstName, lastName, jobTitle, email, company, registeredIP, title, frimAum, firmType);

            return rtnVal;
        }

        public string CheckEmailExistence(string email)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.CheckEmailExistence(email);

            return rtnVal;
        }

        public string GetUserDetailsByEmail(string email)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetUserDetailsByEmail(email);

            return rtnVal;
        }

        public string GetUserDetailsById(string guid)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetUserDetailsById(guid);

            return rtnVal;
        }

        public string GetClientDetails()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetClientDetails();

            return rtnVal;
        }

        public string GetAllPAObjects(Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllPAObjects(userId);

            return rtnVal;
        }

        public string GetPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type, int recordCount, Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjects(pageIndex, categoryId, clientType, searchText, type, recordCount, userId);

            return rtnVal;
        }

        public string GetPAObjectIds(int categoryId, string clientType, string searchText, int type, Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectIds(categoryId, clientType, searchText, type, userId);

            return rtnVal;
        }

        public string GetSampleReportIds(int firmId, int productId, string searchText)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetSampleReportIds(firmId, productId, searchText);

            return rtnVal;
        }

        public string GetTemplateDesignRecordIds(int firmId, int productId, string searchText)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetTemplateDesignRecordIds(firmId, productId, searchText);

            return rtnVal;
        }

        public string GetMyGalleryPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type, Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetMyGalleryPAObjects(pageIndex, categoryId, clientType, searchText, type, userId);

            return rtnVal;
        }

        public string GetAllMyGalleryPAObjects(Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllMyGalleryPAObjects(userId);

            return rtnVal;
        }

        public string GetSampleReports(int pageIndex, int firmId, int productId, string searchText, int recordCount)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetSampleReports(pageIndex, firmId, productId, searchText, recordCount);

            return rtnVal;
        }

        public string GetTemplateDesigns(int pageIndex, int firmId, int productId, string searchText, int recordCount)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetTemplateDesigns(pageIndex, firmId, productId, searchText, recordCount);

            return rtnVal;
        }

        public string GetPAObjectClientTypes()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectClientTypes();

            return rtnVal;
        }

        public string GetPAObjectTypes()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectTypes();

            return rtnVal;
        }

        public string GetAllPAObjectTypes()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllPAObjectTypes();

            return rtnVal;
        }

        public string GetPAObjectCategories()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectCategories();

            return rtnVal;
        }

        public string GetPAObjectCategoryOrdering(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectCategoryOrdering(sidx, sord, page, rows, search, searchField, searchString, searchOperator);

            return rtnVal;
        }

        public string GetAllPAObjectCategories()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllPAObjectCategories();

            return rtnVal;
        }

        public byte[] GetImage(string Id, string Type, string PageNo)
        {
            byte[] rtnVal = new byte[0];

            rtnVal = DataManager.GetImage(Id, Type, PageNo);

            return rtnVal;
        }

        public string GetSampleReportPages(int id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetSampleReportPages(id);

            return rtnVal;
        }

        public string GetSampleReportDetails(int id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetSampleReportDetails(id);

            return rtnVal;
        }

        public string GetTemplateDesignDetails(int id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetTemplateDesignDetails(id);

            return rtnVal;
        }

        public string GetTemplateDesignPages(int id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetTemplateDesignPages(id);

            return rtnVal;
        }

        public string GetPAObjectDetails(int id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectDetails(id);

            return rtnVal;
        }

        public string GetPAObjectDetailsByUserId(int id, Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectDetailsByUserId(id, userId);

            return rtnVal;
        }

        public string GetMyGalleryPAObjectDetails(int id, string type)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetMyGalleryPAObjectDetails(id, type);

            return rtnVal;
        }

        public byte[] GetSlideImage(string Id, string PageNo)
        {
            byte[] rtnVal = new byte[0];

            rtnVal = DataManager.GetSlideImage(Id, PageNo);

            return rtnVal;
        }

        public string GetFirmTypes()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetFirmTypes();

            return rtnVal;
        }

        public string GetProductTypes()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetProductTypes();

            return rtnVal;
        }

        public bool SaveSampleReports(byte[] PowerPointFile, string Name, string LongDescription, string ShortDescription, Guid UploadedBy, string ProductType, string FirmType, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight, string Preview)
        {
            bool rtnVal = false;

            rtnVal = DataManager.SaveSampleReports(PowerPointFile, Name, LongDescription, ShortDescription, UploadedBy, ProductType, FirmType, PageImageWidth, PageImageHeight, ThumbImageWidth, ThumbImageHeight, Preview);

            return rtnVal;
        }

        public bool SaveTemplateDesigns(byte[] PowerPointFile, string Name, string LongDescription, string ShortDescription, Guid UploadedBy, string ProductType, string FirmType, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight, string Preview)
        {
            bool rtnVal = false;

            rtnVal = DataManager.SaveTemplateDesigns(PowerPointFile, Name, LongDescription, ShortDescription, UploadedBy, ProductType, FirmType, PageImageWidth, PageImageHeight, ThumbImageWidth, ThumbImageHeight, Preview);

            return rtnVal;
        }

        public string AddToMyGallery(int id, string type, Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.AddToMyGallery(id, type, userId);

            return rtnVal;
        }

        public string RemoveFromMyGallery(int id, string type)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.RemoveFromMyGallery(id, type);

            return rtnVal;
        }

        public string RemoveFromMyGallery(int id, string type, Guid userId)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.RemoveFromMyGallery(id, type, userId);

            return rtnVal;
        }

        public string AddNotes(int id, string note)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.AddNotes(id, note);

            return rtnVal;
        }

        public string RemoveNotes(int id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.RemoveNotes(id);

            return rtnVal;
        }

        public string GetNotes(int id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetNotes(id);

            return rtnVal;
        }

        public string GetClients(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetClients(sidx, sord, page, rows, search, searchField, searchString, searchOperator);

            return rtnVal;
        }

        public bool ValidateUser(string username, string password)
        {
            return DataManager.ValidateUser(username, password);
        }

        public string GetUserNameByEmail(string email)
        {
            return DataManager.GetUserNameByEmail(email);
        }

        public string[] GetAllRoles()
        {
            return DataManager.GetAllRoles();
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            DataManager.AddUsersToRoles(usernames, roleNames);
        }

        public string[] GetRolesForUser(string userNames)
        {
            return DataManager.GetRolesForUser(userNames);
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return DataManager.DeleteUser(username, deleteAllRelatedData);
        }

        public bool CreateAdminUser(string userName, string password, string email, string ApplicationName)
        {
            return DataManager.CreateAdminUser(userName, password, email, ApplicationName);
        }

        public MembershipUser GetUser(string userName, bool userIsOnline)
        {
            return DataManager.GetUser(userName, userIsOnline);
        }

        public string CheckUserNameExistence(string userName)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.CheckUserNameExistence(userName);

            return rtnVal;
        }

        public string CreateNewUser(string userName, string applicationName, string email, string password, string[] roles)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.CreateNewUser(userName, applicationName, email, password, roles);

            return rtnVal;
        }

        public string GetUserRoles()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetUserRoles();

            return rtnVal;
        }

        public string GetSamples(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetSamples(sidx, sord, page, rows, search, searchField, searchString, searchOperator);

            return rtnVal;
        }

        public string GetReportObjects(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetReportObjects(sidx, sord, page, rows, search, searchField, searchString, searchOperator);

            return rtnVal;
        }

        public string GetReportObjectMockups(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetReportObjectMockups(sidx, sord, page, rows, search, searchField, searchString, searchOperator);

            return rtnVal;
        }

        public string UpdateSampleReport(int Id, string name, string shortDescription, string longDescription, int firmId, int productId, string preRegisterPreView)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.UpdateSampleReport(Id, name, shortDescription, longDescription, firmId, productId, preRegisterPreView);

            return rtnVal;
        }

        public string UpdateReportObject(int Id, string preRegisterPreView, string isDeleted)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.UpdateReportObject(Id, preRegisterPreView, isDeleted);

            return rtnVal;
        }

        public string UpdateCategoryOrdering(int Id, int SortOrder, string isVisible)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.UpdateCategoryOrdering(Id, SortOrder, isVisible);

            return rtnVal;
        }

        public string UpdateReportObjectMockup(string Id, string name, string description, string clientType, string category, string type, string preRegisterPreView)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.UpdateReportObjectMockup(Id, name, description, clientType, category, type, preRegisterPreView);

            return rtnVal;
        }

        public string DeleteSampleReport(int Id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.DeleteSampleReport(Id);

            return rtnVal;
        }

        public string DeleteReportObjectMockup(int Id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.DeleteReportObjectMockup(Id);

            return rtnVal;
        }

        public string GetTemplates(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetTemplates(sidx, sord, page, rows, search, searchField, searchString, searchOperator);

            return rtnVal;
        }

        public string UpdateTemplateDesigns(int Id, string name, string shortDescription, string longDescription, int firmId, int productId, string preRegisterPreView)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.UpdateTemplateDesigns(Id, name, shortDescription, longDescription, firmId, productId, preRegisterPreView);

            return rtnVal;
        }

        public string DeleteTemplateDesign(int Id)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.DeleteTemplateDesign(Id);

            return rtnVal;
        }

        public string EditSampleReportUploadedFile(int Id, byte[] PowerPointFile, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.EditSampleReportUploadedFile(Id, PowerPointFile, PageImageWidth, PageImageHeight, ThumbImageWidth, ThumbImageHeight);

            return rtnVal;
        }

        public string EditTemplateDesignUploadedFile(int Id, byte[] PowerPointFile, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.EditTemplateDesignUploadedFile(Id, PowerPointFile, PageImageWidth, PageImageHeight, ThumbImageWidth, ThumbImageHeight);

            return rtnVal;
        }

        public string GetSampleReportsForPreview()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetSampleReportsForPreview();

            return rtnVal;
        }

        public string GetAllSampleReportsForPreview()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllSampleReportsForPreview();

            return rtnVal;
        }

        public string GetTemplateDesignsForPreView()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetTemplateDesignsForPreView();

            return rtnVal;
        }

        public string GetAllTemplateDesignsForPreView()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllTemplateDesignsForPreView();

            return rtnVal;
        }

        public string GetPAObjectsForPreView()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetPAObjectsForPreView();

            return rtnVal;
        }

        public string GetAllPAObjectsForPreView()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllPAObjectsForPreView();

            return rtnVal;
        }

        public string SendMail(string emailTo, string emailFrom, string emailMessage, string smtpServer)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.SendMail(emailTo, emailFrom, emailMessage, smtpServer);

            return rtnVal;
        }

        public string GetAllSampleReports()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllSampleReports();

            return rtnVal;
        }

        public string GetAllTemplateDesigns()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetAllTemplateDesigns();

            return rtnVal;
        }

        public bool SavePAObjectMockups(byte[] byteArray, Guid UploadedBy, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight)
        {
            bool rtnVal = false;

            rtnVal = DataManager.SavePAObjectMockups(byteArray, UploadedBy, PageImageWidth, PageImageHeight, ThumbImageWidth, ThumbImageHeight);

            return rtnVal;
        }

        public byte[] DownLoadFile(string Id, string Type)
        {
            byte[] rtnVal = new byte[0];

            rtnVal = DataManager.DownLoadFile(Id, Type);

            return rtnVal;
        }

        public string SendMailOnClientRegistration(string clientEmail, string smtpServer, string emailFrom, string isBodyHTML, string subject, string templateName, string emailImageEmbed, string adminEmail, string type)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.SendMailOnClientRegistration(clientEmail, smtpServer, emailFrom, isBodyHTML, subject, templateName, emailImageEmbed, adminEmail, type);

            return rtnVal;
        }

        public string AddTagContent(string tagContent)
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.AddTagContent(tagContent);

            return rtnVal;
        }

        public string GetTagContent()
        {
            string rtnVal = string.Empty;

            rtnVal = DataManager.GetTagContent();

            return rtnVal;
        }

        public byte[] GetAllPAObjectDetailsForPdf_itextsharp()
        {
            byte[] rtnVal = new byte[0];

            rtnVal = DataManager.GetAllPAObjectDetailsForPdf_itextsharp();

            return rtnVal;
        }

        public byte[] GetPAObjectDetailsForPdfByList(int[] paObjectIdList, int[] mockupObjectIdList)
        {
            byte[] rtnVal = new byte[0];

            rtnVal = DataManager.GetPAObjectDetailsForPdfByList(paObjectIdList, mockupObjectIdList);

            return rtnVal;
        }

        public byte[] GetAllPAObjectDetailsForPdf_pdfsharp()
        {
            byte[] rtnVal = new byte[0];

            rtnVal = DataManager.GetAllPAObjectDetailsForPdf_pdfsharp();

            return rtnVal;
        }
    }
}
