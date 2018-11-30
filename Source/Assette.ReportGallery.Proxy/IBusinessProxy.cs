using System;
using System.Runtime.InteropServices;
using System.Web.Security;

namespace Assette.ReportGallery.Proxy
{
    [Guid("2A8C3C98-38BB-410D-A99A-3C8DB470282E")]
    [ComVisible(true)]
    public interface IBusinessProxy
    {
        string GetConnectionString();
        string CreateUser(string firstName, string lastName, string jobTitle, string email, string company, string registeredIP, string title, string frimAum, string firmType);
        string CheckEmailExistence(string email);
        string GetUserDetailsByEmail(string email);
        string GetUserDetailsById(string guid);
        string GetClientDetails();
        string GetPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type, int recordCount, Guid userId);
        string GetPAObjectIds(int categoryId, string clientType, string searchText, int type, Guid userId);
        string GetMyGalleryPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type, Guid userId);
        string GetSampleReports(int pageIndex, int firmId, int productId, string searchText, int recordCount);
        string GetTemplateDesigns(int pageIndex, int firmId, int productId, string searchText, int recordCount);
        string GetPAObjectClientTypes();
        string GetPAObjectCategories();
        byte[] GetImage(string Id, string Type, string PageNo);
        string GetSampleReportPages(int id);
        string GetSampleReportDetails(int id);
        string GetTemplateDesignDetails(int id);
        string GetTemplateDesignPages(int id);
        string GetPAObjectDetails(int id);
        string GetMyGalleryPAObjectDetails(int id, string type);
        byte[] GetSlideImage(string Id, string PageNo);
        string GetFirmTypes();
        string GetProductTypes();
        bool SaveSampleReports(byte[] PowerPointFile, string Name, string ShortDescription, string LongDescription, Guid UploadedBy, string ProductType, string FirmType, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight, string Preview);
        bool SaveTemplateDesigns(byte[] PowerPointFile, string Name, string ShortDescription, string LongDescription, Guid UploadedBy, string ProductType, string FirmType, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight, string Preview);
        string AddToMyGallery(int id, string type, Guid userId);
        string RemoveFromMyGallery(int id, string type);
        string AddNotes(int id, string note);
        string RemoveNotes(int id);
        string GetNotes(int id);
        string GetClients(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator);
        string CheckHeartBeat();
        string GetUserNameByEmail(string email);
        string[] GetAllRoles();
        void AddUsersToRoles(string[] usernames, string[] roleNames);
        string[] GetRolesForUser(string userNames);
        bool DeleteUser(string username, bool deleteAllRelatedData);
        bool CreateAdminUser(string userName, string password, string email, string ApplicationName);
        MembershipUser GetUser(string userName, bool userIsOnline);
        string CheckUserNameExistence(string userName);
        string CreateNewUser(string userName, string applicationName, string email, string password, string[] roles);
        string GetUserRoles();
        string GetSamples(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator);
        string GetTemplates(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator);       
        string UpdateSampleReport(int Id, string name, string shortDescription, string longDescription, int firmId, int productId, string preRegisterPreView);
        string UpdateTemplateDesigns(int Id, string name, string shortDescription, string longDescription, int firmId, int productId, string preRegisterPreView);
        string UpdateReportObject(int Id, string preRegisterPreView, string isDeleted);
        string DeleteSampleReport(int Id);
        string DeleteTemplateDesign(int Id);
        string EditSampleReportUploadedFile(int Id, byte[] PowerPointFile, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight);
        string EditTemplateDesignUploadedFile(int Id, byte[] PowerPointFile, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight);
        string GetSampleReportsForPreview();
        string GetTemplateDesignsForPreView();
        string GetPAObjectsForPreView();
        string SendMail(string emailTo, string emailFrom, string emailMessage, string smtpServer);
        string GetSampleReportIds(int firmId, int productId, string searchText);
        string GetTemplateDesignRecordIds(int firmId, int productId, string searchText);
        string RemoveFromMyGallery(int id, string type, Guid userId);
        string GetPAObjectTypes();
        bool ValidateUser(string username, string password);
        string GetPAObjectDetailsByUserId(int id, Guid userId);
        string GetAllPAObjects(Guid userId);
        string GetAllSampleReports();
        string GetAllTemplateDesigns();
        string GetAllMyGalleryPAObjects(Guid userId); string GetAllPAObjectsForPreView();
        string GetAllSampleReportsForPreview();
        string GetAllTemplateDesignsForPreView();
        string GetReportObjects(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator);
        bool SavePAObjectMockups(byte[] byteArray, Guid UploadedBy, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight);
        string GetReportObjectMockups(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator);
        string UpdateReportObjectMockup(string Id, string name, string description, string clientType, string category, string type, string preRegisterPreView);
        string DeleteReportObjectMockup(int Id);
        string GetAllPAObjectCategories();
        string GetAllPAObjectTypes();
        string GetPAObjectCategoryOrdering(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator);
        string UpdateCategoryOrdering(int Id, int SortOrder, string isVisible);
        byte[] DownLoadFile(string Id, string Type);
        string SendMailOnClientRegistration(string clientEmail, string smtpServer, string emailFrom, string isBodyHTML, string subject, string templateName, string emailImageEmbed, string adminEmail, string type);
        string AddTagContent(string tagContent);
        string GetTagContent();
        byte[] GetAllPAObjectDetailsForPdf_itextsharp();
        byte[] GetAllPAObjectDetailsForPdf_pdfsharp();
        byte[] GetPAObjectDetailsForPdfByList(int[] paObjectIdList, int[] mockupObjectIdList);
    }
}
