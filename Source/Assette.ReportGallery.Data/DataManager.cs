using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI.WebControls;
using Aspose.Slides;
using Assette.PA.Data;
using Assette.ReportGallery.Data.Models;
using BTA.EPM.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using log4net.Config;
using log4net.Util;
using SlideStripper;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
//using MigraDoc.DocumentObjectModel;

namespace Assette.ReportGallery.Data
{
    public class DataManager
    {
        public static string clientCode = string.Empty;      
        private static string message = string.Empty;
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataManager));

        #region CONNECTION

        static DataManager()
        {
            clientCode = System.Configuration.ConfigurationManager.AppSettings["ClientCode"];

            LogLog.InternalDebugging = true;

            var log4NetConfigFilePath = string.Format("{0}.log4net", Assembly.GetExecutingAssembly().Location);

            // development purpose
            //log4NetConfigFilePath = @"G:\Report Gallery - Versions\Sam Repo\Source\Assette.Web.ReportsGallery\Assette.Web.ReportsGallery\Assette.Web.ReportsGallery.dll.log4net";

            XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFilePath));
            BasicConfigurator.Configure();

            // aspose license
            License license = new License();
            license.SetLicense(new MemoryStream(Resource.Aspose_Slides));

            // create connection
            BTAConnectionManager.CreateConnection(clientCode);
        }

        public static string GetConnectionString()
        {
            var ConnectionString = string.Empty;

            if (BTAConnectionManager.IsInTransaction)
            {
                ConnectionString = BTAConnectionManager.Connection.ClientConnectionString;

                //message = string.Format("GetConnectionString() ConnectionString: {0}", ConnectionString);
                //Log.Info(message);
            }
            else
            {
                try
                {
                    BTAConnectionManager.CreateConnection(clientCode);

                    ConnectionString = BTAConnectionManager.Connection.ClientConnectionString;

                    //message = string.Format("GetConnectionString() ClientCode: {0}, ConnectionString: {1}", clientCode, ConnectionString);
                    //Log.Info(message);
                }
                catch (Exception ex)
                {
                    Log.Error("GetConnectionString() - Error: Database connection creation error !!", ex);
                }
            }

            //ConnectionString = "user id=sa;password=hammer;database=Gallery_PRD;server=192.168.180.41";

            return ConnectionString;
        }

        public static string CheckHeartBeat()
        {
            try
            {
                Log.Info("CheckHeartBeat()");

                SqlHelper.ExecuteNonQuery(GetConnectionString(), CommandType.Text, "Select top 1 Id from paObjects;");

                Log.Info("CheckHeartBeat() - Connection to database is successful !!");

                return "Connection to database is successful !!";
            }
            catch (Exception ex)
            {
                Log.Error("CheckHeartBeat() - Error: Connection to database is unsuccessful !!", ex);

                return "ERROR: Connection to database is unsuccessful !!";
            }
        }

        public static RepGlryEntities GetEntity()
        {
            var entity = new RepGlryEntities();

            try
            {
                ((EntityConnection)entity.Connection).StoreConnection.ConnectionString = GetConnectionString();
            }
            catch (Exception ex)
            {
                Log.Error("GetEntity()", ex);
            }

            return entity;
        }

        #endregion


        #region USER

        public static string CreateUser(string firstName, string lastName, string jobTitle, string email, string company, string registeredIP, string title, string firmAum, string firmType)
        {
            var rtnValue = string.Empty;

            message = string.Format("CreateUser() Parameters = firstName: {0}, lastName: {1}, jobTitle: {2}, email: {3}, company: {4}, registeredIP: {5}, email: {6}, company: {7}, registeredIP: {8}", firstName, lastName, jobTitle, email, company, registeredIP, title, firmAum, firmType);
            Log.Info(message);

            try
            {
                var prospect = new Prospects();

                prospect.Id = Guid.NewGuid();
                prospect.FirstName = firstName.Trim();
                prospect.LastName = lastName.Trim();
                prospect.JobTitle = jobTitle.Trim();
                prospect.Email = email.Trim();
                prospect.Company = company.Trim();
                prospect.RegisteredDate = DateTime.Now;
                prospect.RegisteredIP = registeredIP.Trim();
                prospect.Title = title.Trim();
                prospect.FirmAUM = firmAum.Trim();
                prospect.FirmType = firmType.Trim();

                var entity = GetEntity();

                entity.AddToProspects(prospect);
                rtnValue = entity.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("CreateUser()", ex);
            }

            return rtnValue;
        }

        public static string CheckEmailExistence(string email)
        {
            var rtnValue = string.Empty;

            message = string.Format("CheckEmailExistence() Parameters = email: {0}", email);
            Log.Info(message);

            try
            {
                var entity = GetEntity();
                var record = entity.Prospects.Any(m => m.Email.ToUpper() == email.Trim().ToUpper());

                if (record == true)
                {
                    rtnValue = "1"; // record exists
                }
                else
                {
                    rtnValue = "0"; // no record found
                }

                // testing purpose
                //throw new System.ArgumentException("Parameter cannot be null", "original");
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("CheckEmailExistence()", ex);

                // testing purpose
                //throw new System.ArgumentException("Parameter cannot be null", "original");
            }

            return rtnValue;
        }

        public static string GetUserDetailsByEmail(string email)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetUserDetailsByEmail() Parameters = email: {0}", email);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<Prospects> prospects = entity.Prospects;

                var objects =
                    (from prospect in prospects
                     where prospect.Email.ToUpper() == email.ToUpper()
                     orderby prospect.Id
                     select new
                     {
                         Id = prospect.Id,
                         FirstName = prospect.FirstName,
                         LastName = prospect.LastName,
                         JobTitle = prospect.JobTitle,
                         Email = prospect.Email,
                         Company = prospect.Company,
                         RegisteredIP = prospect.RegisteredIP,
                         RegisteredDate = prospect.RegisteredDate
                     });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetUserDetailsByEmail()", ex);
            }

            return rtnValue;
        }

        public static string GetUserDetailsById(string guid)
        {
            var rtnValue = string.Empty;

            Guid userGuid = Guid.Parse(guid);

            message = string.Format("GetUserDetailsById() Parameters = guid: {0}", guid);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<Prospects> prospects = entity.Prospects;

                var objects =
                    (from prospect in prospects
                     where prospect.Id == userGuid
                     orderby prospect.Id
                     select new
                     {
                         Id = prospect.Id,
                         FirstName = prospect.FirstName,
                         LastName = prospect.LastName,
                         JobTitle = prospect.JobTitle,
                         Email = prospect.Email,
                         Company = prospect.Company,
                         RegisteredIP = prospect.RegisteredIP,
                         RegisteredDate = prospect.RegisteredDate
                     });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetUserDetailsById()", ex);
            }

            return rtnValue;
        }

        public static string GetClientDetails()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                ObjectSet<Prospects> prospects = entity.Prospects;

                var objects =
                    (from prospect in prospects
                     orderby prospect.Id
                     select new
                     {
                         Id = prospect.Id,
                         FirstName = prospect.FirstName,
                         LastName = prospect.LastName,
                         JobTitle = prospect.JobTitle,
                         Email = prospect.Email,
                         Company = prospect.Company,
                         RegisteredIP = prospect.RegisteredIP,
                         RegisteredDate = prospect.RegisteredDate
                     });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetClientDetails()", ex);
            }

            return rtnValue;
        }

        public static string GetClients(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            Helper hlpr = new Helper();

            return hlpr.JsonForJqgrid(GetClientData(sidx, sord, page, rows, search, searchField, searchString, searchOperator), rows, GetClientCount(), page);
        }

        public static DataTable GetClientData(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            DataTable rtnValue = new DataTable();

            message = string.Format("GetClientData() Parameters = sidx: {0}, sord: {1}, page: {2}, rows: {3}", sidx, sord, page.ToString(), rows.ToString());
            Log.Info(message);

            int initialRecordCount = rows;
            int startRow = (page - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<Prospects> prospects = entity.Prospects;
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;

                var objects =
                    (from prospect in prospects
                     orderby prospect.RegisteredDate
                     select new
                     {
                         Id = prospect.Id,
                         FirstName = prospect.FirstName,
                         LastName = prospect.LastName,
                         JobTitle = prospect.JobTitle,
                         Email = prospect.Email,
                         Company = prospect.Company,
                         RegisteredIP = prospect.RegisteredIP,
                         RegisteredDate = prospect.RegisteredDate,
                         ObjectCount = prospectsGalleryContents.Count(i => i.PropsectId == prospect.Id)
                     });

                int RecordCount = objects.Count();

                if (search == true)
                {
                    // filter by firstname
                    if (searchField == "FirstName")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.FirstName.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.FirstName.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by lastname
                    if (searchField == "LastName")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.LastName.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.LastName.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    // filter by jobtitle
                    if (searchField == "JobTitle")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.JobTitle.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.JobTitle.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by email
                    if (searchField == "Email")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.Email.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.Email.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by company
                    if (searchField == "Company")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.Company.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.Company.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();
                }

                RecordCount = objects.Count();

                // sorting by date
                if (sord == "desc")
                {
                    objects = objects.OrderByDescending(x => x.RegisteredDate);
                }
                else
                {
                    objects = objects.OrderBy(x => x.RegisteredDate);
                }

                RecordCount = objects.Count();

                // paging functionality
                objects = objects.Skip(startRow).Take(initialRecordCount);

                RecordCount = objects.Count();

                // serializing - JavaScriptSerializer
                //var serializer = new JavaScriptSerializer();             
                //string json = serializer.Serialize(objects);

                // converting json string to datatable
                //rtnValue = JsonConvert.DeserializeObject<DataTable>(json);

                // serializing - JsonConvert
                string json = JsonConvert.SerializeObject(objects);

                // converting json string to datatable
                rtnValue = JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                rtnValue = null; // error

                Log.Error("GetClientData()", ex);
            }

            return rtnValue;
        }

        public static int GetClientCount()
        {
            int rtnValue = 0;

            message = string.Format("GetClientCount()");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<Prospects> prospects = entity.Prospects;

                var objects =
                    (from prospect in prospects
                     orderby prospect.RegisteredDate
                     select new
                     {
                         Id = prospect.Id
                     });

                rtnValue = objects.Count();
            }
            catch (Exception ex)
            {
                rtnValue = -1; // error

                Log.Error("GetClientCount()", ex);
            }

            return rtnValue;
        }

        #endregion


        #region REPORT OBJECTS

        // working one
        public static string GetAllPAObjects(Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetAllPAObjects() Parameters = userId: {0}", userId.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                IQueryable<PAObject> pAObjects = entity.PAObjects1.Where(pao => (pao.IsDeleted == false) && (pao.ObjectTypeID == 1 || pao.ObjectTypeID == 2)).OrderBy(pao => pao.ID); ;
                IQueryable<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;
                IQueryable<PAObjectMockup> pAObjectMockups = entity.PAObjectMockups1.OrderBy(pam => pam.ID);
                IQueryable<PAObjectCategoryOrder> pAObjectCategoryOrders = entity.PAObjectCategoryOrders.Where(pa => pa.IsVisible == true);

                int RecordCount = 0;

                // get pa-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var PAObjects =
                     (from pAObject in pAObjects
                      join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObject.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId                     
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObject.paObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObject.paObjectType.Type,
                          ModifiedDate = pAObject.ModifiedDate,
                          AddedToMyGallery = prospectsGalleryContents.Count(i => i.PropsectId == userId && i.ObjectId == pAObject.ID && i.TableType == "p"),
                          ObjectTableType = "p",
                          CategoryOrderId = pAObjectCategoryOrder.SortOrder
                      });

                RecordCount = PAObjects.Count();
               
                // get mockup objects that doesn't exists on pa-object table
                var PAObjectMokupsDoesntExists =
                    (from pAObjectMockup in pAObjectMockups
                     join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObjectMockup.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                     where
                    (!pAObjects.Any(pao => pao.Name.Replace(" ", "").Trim().ToLower() == pAObjectMockup.Name.Replace(" ", "").Trim().ToLower() && pAObjectMockup.ObjectCategoryID == pao.ObjectCategoryID && pAObjectMockup.ObjectTypeID == pao.ObjectTypeID))
                    select new
                    {
                        ID = pAObjectMockup.ID,
                        Name = pAObjectMockup.Name,
                        Description = pAObjectMockup.Description,
                        ClientType = pAObjectMockup.ClientType,
                        CategoryId = pAObjectMockup.ObjectCategoryID,
                        Category = pAObjectMockup.paObjectCategory.Category,
                        TypeId = pAObjectMockup.ObjectTypeID,
                        Type = pAObjectMockup.paObjectType.Type,
                        ModifiedDate = pAObjectMockup.UploadedDate,
                        AddedToMyGallery = prospectsGalleryContents.Count(i => i.PropsectId == userId && i.ObjectId == pAObjectMockup.ID && i.TableType == "m"),
                        ObjectTableType = "m",
                        CategoryOrderId = pAObjectCategoryOrder.SortOrder
                    });

                RecordCount = PAObjectMokupsDoesntExists.Count();

                // join 2 results
                var Objects = PAObjects.Union(PAObjectMokupsDoesntExists);

                RecordCount = Objects.Count();

                // sorting: 
                // 1 - category order - PAObjectCategoryOrder
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                Objects = Objects.OrderBy(obj => obj.CategoryOrderId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                // get objects
                var serializer = new JavaScriptSerializer();
                var pagedobjects = serializer.Serialize(Objects);

                rtnValue = pagedobjects;
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllPAObjects()", ex);
            }

            return rtnValue;
        }

        public static string GetPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type, int recordCount, Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetPAObjects() Parameters = pageIndex: {0}, categoryId: {1}, clientType: {2}, searchText: {3}, type: {4}, userId: {5}", pageIndex, categoryId, clientType, searchText, type, userId.ToString());
            Log.Info(message);

            int initialRecordCount = recordCount;
            int startRow = (pageIndex - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;

                int RecordCount = 0;

                // get all objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsAll =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      where pAObject.ObjectTypeID == 1 || pAObject.ObjectTypeID == 2 // 1: tables, 2: charts
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type
                      });

                RecordCount = ObjectsAll.Count();

                // get objects that has been added to my gallery ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsAddedToMyGallery =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      join prospectsGalleryContent in prospectsGalleryContents on pAObject.ID equals prospectsGalleryContent.ObjectId
                      where prospectsGalleryContent.PropsectId == userId && (pAObject.ObjectTypeID == 1 || pAObject.ObjectTypeID == 2) // 1: tables, 2: charts
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type
                      });

                RecordCount = ObjectsAddedToMyGallery.Count();

                // get objects that haven't been added to my gallery ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // if the columns are not equal, except is not working
                var ObjectsNotAddedToMyGallery = ObjectsAll.Except(ObjectsAddedToMyGallery);

                //adding new column 'AddedToMyGallery = 1'
                var ObjectsAddedToMyGalleryNew =
                     (from pAObject in ObjectsAddedToMyGallery
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.CategoryId,
                          Category = pAObject.Category,
                          TypeId = pAObject.TypeId,
                          Type = pAObject.Type,
                          AddedToMyGallery = "1"
                      });

                RecordCount = ObjectsAddedToMyGalleryNew.Count();

                //adding new column 'AddedToMyGallery = 0'
                var ObjectsNotAddedToMyGalleryNew =
                     (from pAObject in ObjectsNotAddedToMyGallery
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.CategoryId,
                          Category = pAObject.Category,
                          TypeId = pAObject.TypeId,
                          Type = pAObject.Type,
                          AddedToMyGallery = "0",
                      });

                // joining - union ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var Objects = ObjectsAddedToMyGalleryNew.Union(ObjectsNotAddedToMyGalleryNew);

                RecordCount = Objects.Count();

                // filter by category
                if (categoryId != 0)
                {
                    Objects = Objects.Where(e => e.CategoryId == categoryId);
                }

                RecordCount = Objects.Count();

                // filter by client type
                if (clientType != "0")
                {
                    Objects = Objects.Where(e => e.ClientType == clientType);
                }

                RecordCount = Objects.Count();

                // filter by search text
                if (searchText != "")
                {
                    Objects = Objects.Where(e => e.Name.ToUpper().Contains(searchText.ToUpper()));
                }

                RecordCount = Objects.Count();

                // filter by type
                if (type != 0)
                {
                    Objects = Objects.Where(e => e.TypeId == type);
                }

                RecordCount = Objects.Count();

                // sorting: 
                // 1 - category
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                Objects = Objects.OrderBy(obj => obj.CategoryId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                // get only the ids - test
                //var Obj =
                //     (from Obje in Objects
                //      select new
                //      {
                //          ID = Obje.ID
                //      });

                // scroll functionality
                Objects = Objects.Skip(startRow).Take(initialRecordCount);

                RecordCount = Objects.Count();

                // get objects
                var serializer = new JavaScriptSerializer();
                var pagedobjects = serializer.Serialize(Objects);

                rtnValue = pagedobjects;
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjects()", ex);
            }

            return rtnValue;
        }

        public static string GetPAObjectIds(int categoryId, string clientType, string searchText, int type, Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetPAObjectIds() Parameters = categoryId: {0}, clientType: {1}, searchText: {2}, type: {3}, userId: {4}", categoryId, clientType, searchText, type, userId.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;

                int RecordCount = 0;

                // get all objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsAll =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      where pAObject.ObjectTypeID == 1 || pAObject.ObjectTypeID == 2 // 1: tables, 2: charts
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type
                      });

                RecordCount = ObjectsAll.Count();

                // get objects that has been added to my gallery ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsAddedToMyGallery =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      join prospectsGalleryContent in prospectsGalleryContents on pAObject.ID equals prospectsGalleryContent.ObjectId
                      where prospectsGalleryContent.PropsectId == userId && (pAObject.ObjectTypeID == 1 || pAObject.ObjectTypeID == 2) // 1: tables, 2: charts
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type
                      });

                RecordCount = ObjectsAddedToMyGallery.Count();

                // get objects that haven't been added to my gallery ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // if the columns are not equal, except is not working
                var ObjectsNotAddedToMyGallery = ObjectsAll.Except(ObjectsAddedToMyGallery);

                //adding new column 'AddedToMyGallery = 1'
                var ObjectsAddedToMyGalleryNew =
                     (from pAObject in ObjectsAddedToMyGallery
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.CategoryId,
                          Category = pAObject.Category,
                          TypeId = pAObject.TypeId,
                          Type = pAObject.Type,
                          AddedToMyGallery = "1"
                      });

                RecordCount = ObjectsAddedToMyGalleryNew.Count();

                //adding new column 'AddedToMyGallery = 0'
                var ObjectsNotAddedToMyGalleryNew =
                     (from pAObject in ObjectsNotAddedToMyGallery
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.CategoryId,
                          Category = pAObject.Category,
                          TypeId = pAObject.TypeId,
                          Type = pAObject.Type,
                          AddedToMyGallery = "0",
                      });

                // joining - union ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var Objects = ObjectsAddedToMyGalleryNew.Union(ObjectsNotAddedToMyGalleryNew);

                // filter by category
                if (categoryId != 0)
                {
                    Objects = Objects.Where(e => e.CategoryId == categoryId);
                }

                // filter by client type
                if (clientType != "0")
                {
                    Objects = Objects.Where(e => e.ClientType == clientType);
                }

                // filter by search text
                if (searchText != "")
                {
                    Objects = Objects.Where(e => e.Name.ToUpper().Contains(searchText.ToUpper()));
                }

                // filter by type
                if (type != 0)
                {
                    Objects = Objects.Where(e => e.TypeId == type);
                }

                // sorting: 
                // 1 - category
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                Objects = Objects.OrderBy(obj => obj.CategoryId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                // get only the ids
                var Obj =
                     (from Obje in Objects
                      select new
                      {
                          ID = Obje.ID
                      });

                // get paged objects
                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(Obj);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjectIds()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetPAObjectClientTypes()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                var pAObjects = entity.PAObjects1.OrderBy(o => o.ID);

                var objectsStandard =
                    (from pAObject in pAObjects
                     where pAObject.ClientType == "Standard"
                     select new
                     {
                         ID = pAObject.ClientType,
                         Type = "Presentations & Reports",
                         ToolTip = "View objects available in Assette Presentations and Assette Reports.  All objects are available in these products"
                     }).Distinct();

                var objectsEasy =
                    (from pAObject in pAObjects
                     where pAObject.ClientType == "Easy"
                     select new
                     {
                         ID = pAObject.ClientType,
                         Type = "EasyReports",
                         ToolTip = "View objects available in Assette EasyReports. Only a subset of objects are available in this product."
                     }).Distinct();

                // sorting
                var obj = objectsStandard.Concat(objectsEasy);

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjectClientTypes()", ex);
            }

            return rtnValue;
        }

        public static string GetPAObjectTypes()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;

                var objects =
                    (from pAObjectType in pAObjectTypes
                     orderby pAObjectType.ID
                     where pAObjectType.ID == 1 || pAObjectType.ID == 2
                     select new
                     {
                         ID = pAObjectType.ID,
                         Type = pAObjectType.Type
                     }).Distinct();


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjectTypes()", ex);
            }

            return rtnValue;
        }

        public static string GetAllPAObjectTypes()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;

                var objects =
                    (from pAObjectType in pAObjectTypes
                     orderby pAObjectType.ID
                     select new
                     {
                         ID = pAObjectType.ID,
                         Type = pAObjectType.Type
                     }).Distinct();


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllPAObjectTypes()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetPAObjectCategories()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                var PAObjectCategoryOrders = entity.PAObjectCategoryOrders.Where(obj => obj.IsVisible == true).OrderBy(obj => obj.SortOrder);

                var objects = (from pAObjectCategoryOrder in PAObjectCategoryOrders
                               select new
                               {
                                   ID = pAObjectCategoryOrder.paObjectCategory.ID,
                                   Type = pAObjectCategoryOrder.paObjectCategory.Category
                               });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjectCategories()", ex);
            }

            return rtnValue;
        }

        public static string GetAllPAObjectCategories()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObjectCategory> PAObjectCategories = entity.PAObjectCategories1;

                var objects = (from PAObjectCategorie in PAObjectCategories
                               orderby PAObjectCategorie.ID
                               select new
                               {
                                   ID = PAObjectCategorie.ID,
                                   Type = PAObjectCategorie.Category
                               });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllPAObjectCategories()", ex);
            }

            return rtnValue;
        }

        public static string GetPAObjectDetails(int id)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetPAObjectDetails() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;

                var objects =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      where pAObject.ID == id
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type
                      });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjectDetails()", ex);
            }

            return rtnValue;
        }

        public static string GetPAObjectDetailsByUserId(int id, Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetPAObjectDetails() Parameters = Id: {0}, userId: {1}", id, userId.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;

                var objects =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      where pAObject.ID == id
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type,
                          AddedToMyGallery = prospectsGalleryContents.Count(obj => obj.PropsectId == userId && obj.ObjectId == id)
                      });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjectDetails()", ex);
            }

            return rtnValue;
        }

        public static string GetPAObjectsForPreView()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetPAObjects() Parameters = no parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var pAObjects = entity.PAObjects1.Where(pao => (pao.IsDeleted == false) && (pao.ObjectTypeID == 1 || pao.ObjectTypeID == 2)); 
               
                var paObjectPreViewsList = entity.PAObjectPreViews.Select(pp => pp.Id);
                var objects = pAObjects.Where(p => paObjectPreViewsList.Contains(p.ID)).OrderBy(p =>p.ID).Select(pAObject => new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category =pAObject.paObjectCategory.Category ,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObject.paObjectType.ID
                      });

                int RecordCount = objects.Count();

                // sorting: 
                // 1 - category
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                objects = objects.OrderBy(obj => obj.CategoryId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetPAObjectsForPreView()", ex);
            }

            return rtnValue;
        }

        public static string GetReportObjects(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            Helper hlpr = new Helper();

            int recordCount = 0;

            DataTable tbl = GetReportObjectData(sidx, sord, page, rows, search, searchField, searchString, searchOperator, out recordCount);

            return hlpr.JsonForJqgrid(tbl, rows, recordCount, page);
        }

        public static DataTable GetReportObjectData(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator, out int recordCount)
        {
            DataTable rtnValue = new DataTable();

            message = string.Format("GetReportObjectData() Parameters = sidx: {0}, sord: {1}, page: {2}, rows: {3}", sidx, sord, page.ToString(), rows.ToString());
            Log.Info(message);

            int RecordCount = 0;

            int initialRecordCount = rows;
            int startRow = (page - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;
                ObjectSet<PAObjectPreView> pAObjectPreViews = entity.PAObjectPreViews;

                var Objects =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      orderby pAObject.ID
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type,
                          IsDeleted = pAObject.IsDeleted,
                          PreRegisterPreView = pAObjectPreViews.Any(obj => obj.ObjectId == pAObject.ID)
                      });

                if (search == true)
                {
                    // filter by ID
                    if (searchField == "ID")
                    {
                        int Id = 0;
                        bool valid = Int32.TryParse(searchString, out Id);

                        if (!valid)
                        {
                            Id = 0;
                        }

                        Objects = Objects.Where(e => e.ID == Id);
                    }

                    RecordCount = Objects.Count();

                    // filter by Name
                    if (searchField == "Name")
                    {
                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.Name.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            Objects = Objects.Where(e => e.Name.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by Description
                    if (searchField == "Description")
                    {
                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.Description.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            Objects = Objects.Where(e => e.Description.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by ClientType
                    if (searchField == "ClientType")
                    {
                        string ClientTypeId = searchString;

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.ClientType == ClientTypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.ClientType != ClientTypeId);
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by Category
                    if (searchField == "Category")
                    {
                        int CategoryId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.CategoryId == CategoryId);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.CategoryId != CategoryId);
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by ProductType
                    if (searchField == "Type")
                    {
                        int TypeId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.TypeId == TypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.TypeId != TypeId);
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by PreRegisterPreView
                    if (searchField == "PreRegisterPreView")
                    {
                        bool PreRegisterPreView = bool.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.PreRegisterPreView == PreRegisterPreView);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.PreRegisterPreView != PreRegisterPreView);
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by IsDeleted
                    if (searchField == "IsDeleted")
                    {
                        bool IsDeleted = bool.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.IsDeleted == IsDeleted);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.IsDeleted != IsDeleted);
                        }
                    }

                    RecordCount = Objects.Count();
                }

                RecordCount = Objects.Count();

                // sorting by Id
                if (sord == "desc")
                {
                    Objects = Objects.OrderByDescending(x => x.ID);
                }

                RecordCount = Objects.Count();

                // paging functionality
                Objects = Objects.Skip(startRow).Take(initialRecordCount);

                int pagingRecordCount = Objects.Count();

                // serializing
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(Objects);

                // converting json string to datatable
                rtnValue = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                rtnValue = null; // error

                Log.Error("GetReportObjectData()", ex);
            }

            // assign record count for paging - out parameter
            recordCount = RecordCount;

            return rtnValue;
        }

        public static string UpdateReportObject(int Id, string preRegisterPreView, string isDeleted)
        {
            var rtnValue = string.Empty;

            message = string.Format("UpdateReportObject() Parameters = Id: {0}, preRegisterPreView: {1}, isDeleted: {2}", Id.ToString(), preRegisterPreView, isDeleted);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                // setting isdeleted
                var pAObject = entity.PAObjects1.FirstOrDefault(m => m.ID == Id);

                if (pAObject != null)
                {
                    if (isDeleted == "True")
                    {
                        pAObject.IsDeleted = true;
                    }
                    else
                    {
                        pAObject.IsDeleted = false;
                    }

                    rtnValue = entity.SaveChanges().ToString();
                }

                // setting preview
                var pAObjectPreView = entity.PAObjectPreViews.FirstOrDefault(m => m.ObjectId == Id);

                if (preRegisterPreView == "True")
                {
                    if (pAObjectPreView == null)
                    {
                        PAObjectPreView pAObjectPreViewNew = new PAObjectPreView();

                        pAObjectPreViewNew.ObjectId = Id;

                        entity.AddToPAObjectPreViews(pAObjectPreViewNew);

                        rtnValue = entity.SaveChanges().ToString();
                    }
                    else
                    {
                        // do nothing - keep the record
                    }
                }
                else
                {
                    if (pAObjectPreView == null)
                    {
                        // do nothing - no record to delete
                    }
                    else
                    {
                        entity.PAObjectPreViews.DeleteObject(pAObjectPreView);

                        rtnValue = entity.SaveChanges().ToString();
                    }
                } 
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("UpdateReportObject()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetAllPAObjectsForPreView()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetAllPAObjectsForPreView() Parameters = no parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                // filters: 'isdeleted = 0' | type = tables/charts
                var pAObjects = entity.PAObjects1.Where(pao => (pao.IsDeleted == false) && (pao.ObjectTypeID == 1 || pao.ObjectTypeID == 2));
                var pAObjectMockups = entity.PAObjectMockups1.Where(paom => paom.PreRegisterPreView == true && (paom.ObjectTypeID == 1 || paom.ObjectTypeID == 2));
                var pAObjectCategoryOrders = entity.PAObjectCategoryOrders.Where(pa => pa.IsVisible == true);

                int RecordCount = 0;

                // important filters: 
                // type: tables/charts
                // isdeleted = false
                // preview = true

                // get all pa-objects to list ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var paObjectPreViewsList = entity.PAObjectPreViews.Select(pp => pp.ObjectId);

                RecordCount = paObjectPreViewsList.Count();

                // get pa-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsAlls =
                     (from pAObject in pAObjects
                      where paObjectPreViewsList.Contains(pAObject.ID)
                      join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObject.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObject.paObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObject.paObjectType.Type,
                          ModifiedDate = pAObject.ModifiedDate,
                          ObjectTableType = "p",
                          CategoryOrderId = pAObjectCategoryOrder.SortOrder
                      });

                RecordCount = ObjectsAlls.Count();


                // get all pa-object-mockups ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (ObjectsAlls.Count() != 0)
                {
                    // get mockup objects that doesn't exists on pa-object table
                    var PAObjectMokupsDoesntExists =
                        (from pAObjectMockup in pAObjectMockups
                         join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObjectMockup.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                         where
                        (!ObjectsAlls.Any(paom => paom.Name.Replace(" ", "").Trim().ToLower() == pAObjectMockup.Name.Replace(" ", "").Trim().ToLower() && pAObjectMockup.ObjectCategoryID == paom.CategoryId && pAObjectMockup.ObjectTypeID == paom.TypeId))
                         select new
                         {
                             ID = pAObjectMockup.ID,
                             Name = pAObjectMockup.Name,
                             Description = pAObjectMockup.Description,
                             ClientType = pAObjectMockup.ClientType,
                             CategoryId = pAObjectMockup.ObjectCategoryID,
                             Category = pAObjectMockup.paObjectCategory.Category,
                             TypeId = pAObjectMockup.ObjectTypeID,
                             Type = pAObjectMockup.paObjectType.Type,
                             ModifiedDate = pAObjectMockup.UploadedDate,
                             ObjectTableType = "m",
                             CategoryOrderId = pAObjectCategoryOrder.SortOrder
                         });

                    RecordCount = PAObjectMokupsDoesntExists.Count();

                    // join 2 results
                    var objects = ObjectsAlls.Union(PAObjectMokupsDoesntExists);

                    RecordCount = objects.Count();

                    // sorting: 
                    // 1 - category
                    // 2 - type - charts first then table
                    // 3 - name alpha asc              
                    objects = objects.OrderBy(obj => obj.CategoryOrderId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                    RecordCount = objects.Count();

                    var serializer = new JavaScriptSerializer();
                    rtnValue = serializer.Serialize(objects);
                }
                else
                {
                    // get pa-objects-mockups
                    var pAObjectMockupsObjects = pAObjectMockups
                        .Select(p => new
                        {
                            ID = p.ID,
                            Name = p.Name,
                            Description = p.Description,
                            ClientType = p.ClientType,
                            CategoryId = p.ObjectCategoryID,
                            Category = p.paObjectCategory.Category,
                            TypeId = p.ObjectTypeID,
                            Type = p.paObjectType.Type,
                            ModifiedDate = p.UploadedDate,
                            ObjectTableType = "m"
                        }); ;

                    RecordCount = pAObjectMockupsObjects.Count();

                    // sorting: 
                    // 1 - category
                    // 2 - type - charts first then table
                    // 3 - name alpha asc              
                    var objects = pAObjectMockupsObjects.OrderBy(obj => obj.CategoryId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                    RecordCount = objects.Count();

                    var serializer = new JavaScriptSerializer();
                    rtnValue = serializer.Serialize(objects);
                }
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllPAObjectsForPreView()", ex);
            }

            return rtnValue;
        }

        #endregion


        #region SAMPLE REPORTS

        public static string GetSampleReports(int pageIndex, int firmId, int productId, string searchText, int recordCount)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetSampleReports() Parameters = pageIndex: {0}, firmId: {1}, productId: {2}, searchText: {3}", pageIndex, firmId, productId, searchText);
            Log.Info(message);

            int initialRecordCount = recordCount;
            int startRow = (pageIndex - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReports> sampleReports = entity.SampleReports;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from sampleReport in sampleReports
                      join firmType in firmTypes on sampleReport.FirmType equals firmType.Id
                      join productType in productTypes on sampleReport.ProductType equals productType.Id
                      orderby sampleReport.Id
                      select new
                      {
                          ID = sampleReport.Id,
                          Name = sampleReport.Name,
                          ShortDescription = sampleReport.ShortDescription,
                          LongDescription = sampleReport.LongDescription,
                          FirmTypeId = sampleReport.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = sampleReport.ProductType,
                          ProductName = productType.ProductName
                      });

                int objectCount = objects.Count();

                // filter by firm
                if (firmId != 0)
                {
                    objects = objects.Where(e => e.FirmTypeId == firmId);
                }

                objectCount = objects.Count();

                // filter by product
                if (productId != 0)
                {
                    objects = objects.Where(e => e.ProductTypeId == productId);
                }

                objectCount = objects.Count();

                // filter by search text
                if (searchText != "")
                {
                    objects = objects.Where(e => e.Name.ToUpper().Contains(searchText.ToUpper()));
                }

                objectCount = objects.Count();

                // scroll functionality
                objects = objects.Skip(startRow).Take(initialRecordCount);

                objectCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);

                // testing purpose
                //throw new System.ArgumentException("Parameter cannot be null", "original");
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetSampleReports()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetAllSampleReports()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetAllSampleReports() Parameters = no parameters passed");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                //var sampleReports = entity.SampleReports.OrderBy(sam => sam.Id);
                var sampleReports = entity.SampleReports.Where(sam => sam.ShortDescription != "preview").OrderBy(sam => sam.Id); // bad fix
                var sampleReportPages = entity.SampleReportPages;

                var objects =
                     (from sampleReport in sampleReports
                      orderby sampleReport.Id
                      select new
                      {
                          ID = sampleReport.Id,
                          Name = sampleReport.Name,
                          ShortDescription = sampleReport.ShortDescription,
                          LongDescription = sampleReport.LongDescription,
                          FirmTypeId = sampleReport.FirmType,
                          FirmName = sampleReport.GLRY_FirmTypes.FirmName,
                          ProductTypeId = sampleReport.ProductType,
                          ProductName = sampleReport.GLRY_ProductTypes.ProductName,
                          PageId = sampleReportPages.OrderBy(p => p.PageNo).FirstOrDefault(i => i.SampleReportId == sampleReport.Id).Id,
                          NoOfPages = sampleReportPages.Count(i => i.SampleReportId == sampleReport.Id),
                          UploadedDate = sampleReport.UploadedDate
                      });

                int objectCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllSampleReports()", ex);
            }

            return rtnValue;
        }

        public static string GetSampleReportIds(int firmId, int productId, string searchText)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetSampleReportIds() Parameters = firmId: {0}, productId: {1}, searchText: {2}", firmId, productId, searchText);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReports> sampleReports = entity.SampleReports;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from sampleReport in sampleReports
                      join firmType in firmTypes on sampleReport.FirmType equals firmType.Id
                      join productType in productTypes on sampleReport.ProductType equals productType.Id
                      orderby sampleReport.Id
                      select new
                      {
                          ID = sampleReport.Id,
                          Name = sampleReport.Name,
                          ShortDescription = sampleReport.ShortDescription,
                          LongDescription = sampleReport.LongDescription,
                          FirmTypeId = sampleReport.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = sampleReport.ProductType,
                          ProductName = productType.ProductName
                      });

                // filter by firm
                if (firmId != 0)
                {
                    objects = objects.Where(e => e.FirmTypeId == firmId);
                }

                // filter by product
                if (productId != 0)
                {
                    objects = objects.Where(e => e.ProductTypeId == productId);
                }

                // filter by search text
                if (searchText != "")
                {
                    objects = objects.Where(e => e.Name.ToUpper().Contains(searchText.ToUpper()));
                }

                // get only the ids
                var Obj =
                     (from Obje in objects
                      orderby Obje.ID
                      select new
                      {
                          ID = Obje.ID
                      });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(Obj);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetSampleReportIds()", ex);
            }

            return rtnValue;
        }

        public static bool SaveSampleReports(byte[] PowerPointFile, string Name, string ShortDescription, string LongDescription, Guid UploadedBy, string ProductType, string FirmType, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight, string Preview)
        {
            message = string.Format("SaveSampleReports() Parameters = PowerPointFile: {0}, Name: {1}, ShortDescription: {2}, LongDescription: {3}, UploadedBy: {4}, ProductType: {5}, FirmType: {6}, PageImageWidth: {7}, PageImageHeight: {8}, ThumbImageWidth: {9}, ThumbImageHeight: {10}, Preview: {11}", PowerPointFile.Length.ToString(), Name, ShortDescription, LongDescription, UploadedBy, ProductType, FirmType, PageImageWidth.ToString(), PageImageHeight.ToString(), ThumbImageWidth.ToString(), ThumbImageHeight.ToString(), Preview);
            Log.Info(message);

            try
            {
                List<PowerpointImage> slideList = new List<PowerpointImage>();

                Stream FileStream = new MemoryStream(PowerPointFile);
                Presentation pres = new Presentation(FileStream);

                foreach (Slide slide in pres.Slides)
                {
                    PowerpointImage powerpointImage = new PowerpointImage();

                    powerpointImage.slideImage = GetSlideImage(slide, PageImageWidth, PageImageHeight);
                    powerpointImage.thumbnailImage = GetSlideImage(slide, ThumbImageWidth, ThumbImageHeight);
                    powerpointImage.pageNo = slide.SlidePosition;

                    //string path = @"G:\images\" + idx + "_" + slide.SlidePosition.ToString() + ".bmp";
                    //Image bitmap = GetSlideImage(slide, PageImageWidth, PageImageHeight);
                    //bitmap.Save(path);  

                    slideList.Add(powerpointImage);
                }

                var entity = GetEntity();

                SampleReports report = new SampleReports();

                report.LongDescription = LongDescription.Trim();
                report.Name = Name.Trim();
                report.PresentationFile = PowerPointFile;
                report.ShortDescription = ShortDescription.Trim();
                report.UploadedBy = UploadedBy;
                report.UploadedDate = DateTime.Now;
                report.ViewCount = 0;
                report.ProductType = int.Parse(ProductType);
                report.FirmType = int.Parse(FirmType);

                if (Preview == "1")
                {
                    report.PreRegisterPreView = true;
                }
                else
                {
                    report.PreRegisterPreView = false;
                }

                SampleReportPages pages;

                foreach (PowerpointImage powerpointImage in slideList)
                {
                    pages = new SampleReportPages();

                    pages.PageImage = (Byte[])new ImageConverter().ConvertTo(powerpointImage.slideImage, typeof(Byte[]));
                    pages.Thumbnail = (Byte[])new ImageConverter().ConvertTo(powerpointImage.thumbnailImage, typeof(Byte[]));
                    pages.PageNo = short.Parse(powerpointImage.pageNo.ToString());
                    pages.ViewCount = 0;

                    report.GLRY_SampleReportPages.Add(pages);
                }

                entity.SampleReports.AddObject(report);
                entity.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("SaveSampleReports()", ex);

                return false;
            }
        }

        public static string GetSampleReportPages(int id)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetSampleReportPages() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReportPages> sampleReportPages = entity.SampleReportPages;

                var objects =
                    (from sampleReportPage in sampleReportPages
                     orderby sampleReportPage.PageNo
                     where sampleReportPage.SampleReportId == id
                     select new
                     {
                         ID = sampleReportPage.Id,
                         PageNo = sampleReportPage.PageNo,
                         SampleReportId = sampleReportPage.SampleReportId
                     });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetSampleReportPages()", ex);
            }

            return rtnValue;
        }

        public static string GetSampleReportDetails(int id)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetSampleReportDetails() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReports> sampleReports = entity.SampleReports;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                    (from sampleReport in sampleReports
                     join firmType in firmTypes on sampleReport.FirmType equals firmType.Id
                     join productType in productTypes on sampleReport.ProductType equals productType.Id
                     where sampleReport.Id == id
                     select new
                     {
                         ID = sampleReport.Id,
                         Name = sampleReport.Name,
                         ShortDescription = sampleReport.ShortDescription,
                         LongDescription = sampleReport.LongDescription,
                         FirmTypeId = sampleReport.FirmType,
                         FirmName = firmType.FirmName,
                         ProductTypeId = sampleReport.ProductType,
                         ProductName = productType.ProductName
                     });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetSampleReportDetails()", ex);
            }

            return rtnValue;
        }

        public static string GetSamples(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            Helper hlpr = new Helper();

            int recordCount = 0;

            DataTable tbl = GetSampleReportData(sidx, sord, page, rows, search, searchField, searchString, searchOperator, out recordCount);

            return hlpr.JsonForJqgrid(tbl, rows, recordCount, page);
        }

        public static DataTable GetSampleReportData(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator, out int recordCount)
        {
            DataTable rtnValue = new DataTable();

            message = string.Format("GetSampleReportData() Parameters = sidx: {0}, sord: {1}, page: {2}, rows: {3}", sidx, sord, page.ToString(), rows.ToString());
            Log.Info(message);

            int RecordCount = 0;

            int initialRecordCount = rows;
            int startRow = (page - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReports> sampleReports = entity.SampleReports;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from sampleReport in sampleReports
                      join firmType in firmTypes on sampleReport.FirmType equals firmType.Id
                      join productType in productTypes on sampleReport.ProductType equals productType.Id
                      orderby sampleReport.Id
                      select new
                      {
                          ID = sampleReport.Id,
                          Name = sampleReport.Name,
                          ShortDescription = sampleReport.ShortDescription,
                          LongDescription = sampleReport.LongDescription,
                          FirmTypeId = sampleReport.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = sampleReport.ProductType,
                          ProductName = productType.ProductName,
                          PreRegisterPreView = sampleReport.PreRegisterPreView
                      });

                RecordCount = objects.Count();

                if (search == true)
                {
                    // filter by ID
                    if (searchField == "Id")
                    {
                        int Id = 0;
                        bool valid = Int32.TryParse(searchString, out Id);

                        if (!valid)
                        {
                            Id = 0;
                        }

                        objects = objects.Where(e => e.ID == Id);
                    }

                    RecordCount = objects.Count();

                    // filter by Name
                    if (searchField == "Name")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.Name.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.Name.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    /*
                    // filter by ShortDescription
                    if (searchField == "ShortDescription")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.ShortDescription.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.ShortDescription.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by LongDescription
                    if (searchField == "LongDescription")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.LongDescription.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.LongDescription.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();
                    */

                    // filter by FirmType
                    if (searchField == "FirmType")
                    {
                        int FirmTypeId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.FirmTypeId == FirmTypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            objects = objects.Where(e => e.FirmTypeId != FirmTypeId);
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by ProductType
                    if (searchField == "ProductType")
                    {
                        int ProductTypeId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.ProductTypeId == ProductTypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            objects = objects.Where(e => e.ProductTypeId != ProductTypeId);
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by PreRegisterPreView
                    if (searchField == "PreRegisterPreView")
                    {
                        bool PreRegisterPreView = bool.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.PreRegisterPreView == PreRegisterPreView);
                        }

                        if (searchOperator == "ne")
                        {
                            objects = objects.Where(e => e.PreRegisterPreView != PreRegisterPreView);
                        }
                    }

                    RecordCount = objects.Count();
                }

                RecordCount = objects.Count();

                // sorting by Id
                if (sord == "desc")
                {
                    objects = objects.OrderByDescending(x => x.ID);
                }

                RecordCount = objects.Count();

                // paging functionality
                objects = objects.Skip(startRow).Take(initialRecordCount);

                int pagingRecordCount = objects.Count();

                // serializing
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(objects);

                // converting json string to datatable
                rtnValue = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                rtnValue = null; // error

                Log.Error("GetSampleReportData()", ex);
            }

            // assign record count for paging - out parameter
            recordCount = RecordCount;

            return rtnValue;
        }

        public static int GetSampleReportCount()
        {
            int rtnValue = 0;

            message = string.Format("GetSampleReportCount()");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReports> sampleReports = entity.SampleReports;

                var objects =
                    (from sampleReport in sampleReports
                     orderby sampleReport.UploadedDate
                     select new
                     {
                         Id = sampleReport.Id
                     });

                rtnValue = objects.Count();
            }
            catch (Exception ex)
            {
                rtnValue = -1; // error

                Log.Error("GetSampleReportCount()", ex);
            }

            return rtnValue;
        }

        public static string UpdateSampleReport(int Id, string name, string shortDescription, string longDescription, int firmId, int productId, string preRegisterPreView)
        {
            var rtnValue = string.Empty;

            message = string.Format("UpdateSampleReport() Parameters = Id: {0}, name: {1}, shortDescription: {2}, longDescription: {3}, firmId: {4}, productId: {5}, preRegisterPreView: {6}", Id.ToString(), name, shortDescription, longDescription, firmId.ToString(), productId.ToString(), preRegisterPreView);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var sample = entity.SampleReports.FirstOrDefault(m => m.Id == Id);

                sample.Name = name.Trim();
                sample.ShortDescription = shortDescription.Trim();
                sample.LongDescription = longDescription.Trim();
                sample.FirmType = firmId;
                sample.ProductType = productId;

                if (preRegisterPreView == "True")
                {
                    sample.PreRegisterPreView = true;
                }
                else
                {
                    sample.PreRegisterPreView = false;
                }

                rtnValue = entity.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("UpdateSampleReport()", ex);
            }

            return rtnValue;
        }

        public static string DeleteSampleReport(int Id)
        {
            var rtnValue = string.Empty;

            message = string.Format("DeleteSampleReport() Parameters = Id: {0}", Id.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReportPages> sampleReportPages = entity.SampleReportPages;

                // delete sample report pages
                var object_pages =
                    (from sampleReportPage in sampleReportPages
                     where sampleReportPage.SampleReportId == Id
                     select sampleReportPage);

                foreach (SampleReportPages object_page in object_pages)
                {
                    entity.SampleReportPages.DeleteObject(object_page);
                }

                // delete sample report
                ObjectSet<SampleReports> sampleReports = entity.SampleReports;

                var object_report =
                    (from sampleReport in sampleReports
                     where sampleReport.Id == Id
                     select sampleReport).First();

                entity.SampleReports.DeleteObject(object_report);

                // save entity
                entity.SaveChanges();

                rtnValue = "1"; // success
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("DeleteSampleReport()", ex);
            }

            return rtnValue;
        }

        public static string EditSampleReportUploadedFile(int Id, byte[] PowerPointFile, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight)
        {
            var rtnValue = string.Empty;

            message = string.Format("EditSampleReportUploadedFile() Parameters = FileId: {0}, PowerPointFile: {1}, PageImageWidth: {2}, PageImageHeight: {3}, ThumbImageWidth: {4}, ThumbImageHeight: {5}", Id.ToString(), PowerPointFile.Length.ToString(), PageImageWidth.ToString(), PageImageHeight.ToString(), ThumbImageWidth.ToString(), ThumbImageHeight.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                // sample report update
                var sampleReport = entity.SampleReports.FirstOrDefault(m => m.Id == Id);

                sampleReport.PresentationFile = PowerPointFile;
                sampleReport.UploadedDate = DateTime.Now;

                entity.SaveChanges();

                // delete sample report pages
                ObjectSet<SampleReportPages> sampleReportPages = entity.SampleReportPages;

                var object_pages =
                    (from sampleReportPage in sampleReportPages
                     where sampleReportPage.SampleReportId == Id
                     select sampleReportPage);

                foreach (SampleReportPages object_page in object_pages)
                {
                    entity.SampleReportPages.DeleteObject(object_page);
                }

                entity.SaveChanges();

                // sample report page update
                ArrayList pageImagesList = new ArrayList();
                ArrayList thumbImagesList = new ArrayList();

                Stream FileStream = new MemoryStream(PowerPointFile);
                Presentation pres = new Presentation(FileStream);

                foreach (Slide slide in pres.Slides)
                {
                    pageImagesList.Add(GetSlideImage(slide, PageImageWidth, PageImageHeight));
                    thumbImagesList.Add(GetSlideImage(slide, ThumbImageWidth, ThumbImageHeight));
                }

                SampleReportPages pages;

                int i = 0;

                foreach (System.Drawing.Image image in pageImagesList)
                {
                    pages = new SampleReportPages();

                    pages.PageImage = (Byte[])new ImageConverter().ConvertTo(image, typeof(Byte[]));
                    pages.Thumbnail = (Byte[])new ImageConverter().ConvertTo(thumbImagesList[i], typeof(Byte[]));
                    pages.PageNo = Convert.ToInt16(i + 1);
                    pages.ViewCount = 0;
                    pages.SampleReportId = Id;

                    i = i + 1;

                    entity.SampleReportPages.AddObject(pages);
                }

                rtnValue = entity.SaveChanges().ToString();

                rtnValue = "1";
            }
            catch (Exception ex)
            {
                Log.Error("EditSampleReportUploadedFile()", ex);

                rtnValue = "-1";
            }

            return rtnValue;
        }

        public static string GetSampleReportsForPreview()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetSampleReportsForPreview() Parameters = no parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<SampleReports> sampleReports = entity.SampleReports;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from sampleReport in sampleReports
                      join firmType in firmTypes on sampleReport.FirmType equals firmType.Id
                      join productType in productTypes on sampleReport.ProductType equals productType.Id
                      where sampleReport.PreRegisterPreView == true
                      orderby sampleReport.Id
                      select new
                      {
                          ID = sampleReport.Id,
                          Name = sampleReport.Name,
                          ShortDescription = sampleReport.ShortDescription,
                          LongDescription = sampleReport.LongDescription,
                          FirmTypeId = sampleReport.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = sampleReport.ProductType,
                          ProductName = productType.ProductName,
                          PreRegisterPreView = sampleReport.PreRegisterPreView
                      });

                int objectCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetSampleReportsForPreview()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetAllSampleReportsForPreview()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetAllSampleReportsForPreview() Parameters = no parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var sampleReports = entity.SampleReports.Where(sam => sam.PreRegisterPreView == true).OrderBy(sam => sam.Id);
                var sampleReportPages = entity.SampleReportPages;

                var objects =
                     (from sampleReport in sampleReports
                      select new
                      {
                          ID = sampleReport.Id,
                          Name = sampleReport.Name,
                          ShortDescription = sampleReport.ShortDescription,
                          LongDescription = sampleReport.LongDescription,
                          FirmTypeId = sampleReport.FirmType,
                          FirmName = sampleReport.GLRY_FirmTypes.FirmName,
                          ProductTypeId = sampleReport.ProductType,
                          ProductName = sampleReport.GLRY_ProductTypes.ProductName,
                          PreRegisterPreView = sampleReport.PreRegisterPreView,
                          PageId = sampleReportPages.OrderBy(p => p.Id).FirstOrDefault(i => i.SampleReportId == sampleReport.Id).Id,
                          NoOfPages = sampleReportPages.Count(i => i.SampleReportId == sampleReport.Id),
                          UploadedDate = sampleReport.UploadedDate                    
                      });

                int objectCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllSampleReportsForPreview()", ex);
            }

            return rtnValue;
        }

        #endregion


        #region TEMPLATE DESIGNS

        public static string GetTemplateDesigns(int pageIndex, int firmId, int productId, string searchText, int recordCount)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetTemplateDesigns() Parameters = pageIndex: {0}, firmId: {1}, productId: {2}, searchText: {3}", pageIndex, firmId, productId, searchText);
            Log.Info(message);

            int initialRecordCount = recordCount;
            int startRow = (pageIndex - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesigns> templateDesigns = entity.TemplateDesigns;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from templateDesign in templateDesigns
                      join firmType in firmTypes on templateDesign.FirmType equals firmType.Id
                      join productType in productTypes on templateDesign.ProductType equals productType.Id
                      orderby templateDesign.Id
                      select new
                      {
                          ID = templateDesign.Id,
                          Name = templateDesign.Name,
                          ShortDescription = templateDesign.ShortDescription,
                          LongDescription = templateDesign.LongDescription,
                          FirmTypeId = templateDesign.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = templateDesign.ProductType,
                          ProductName = productType.ProductName
                      });

                int RecordCount = objects.Count();

                // filter by firm
                if (firmId != 0)
                {
                    objects = objects.Where(e => e.FirmTypeId == firmId);
                }

                RecordCount = objects.Count();

                // filter by product
                if (productId != 0)
                {
                    objects = objects.Where(e => e.ProductTypeId == productId);
                }

                RecordCount = objects.Count();

                // filter by search text
                if (searchText != "")
                {
                    objects = objects.Where(e => e.Name.ToUpper().Contains(searchText.ToUpper()));
                }

                RecordCount = objects.Count();

                // scroll functionality
                objects = objects.Skip(startRow).Take(initialRecordCount);

                RecordCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetTemplateDesigns()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetAllTemplateDesigns()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetAllTemplateDesigns() Parameters = no parameters passed");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var templateDesigns = entity.TemplateDesigns.OrderBy(tmp => tmp.Id);
                var templateDesignPages = entity.TemplateDesignPages;

                var objects =
                     (from templateDesign in templateDesigns
                      select new
                      {
                          ID = templateDesign.Id,
                          Name = templateDesign.Name,
                          ShortDescription = templateDesign.ShortDescription,
                          LongDescription = templateDesign.LongDescription,
                          FirmTypeId = templateDesign.FirmType,
                          FirmName = templateDesign.GLRY_FirmTypes.FirmName,
                          ProductTypeId = templateDesign.ProductType,
                          ProductName = templateDesign.GLRY_ProductTypes.ProductName,
                          PageId = templateDesignPages.OrderBy(p => p.PageNo).FirstOrDefault(i => i.TemplateDesignId == templateDesign.Id).Id,
                          NoOfPages = templateDesignPages.Count(i => i.TemplateDesignId == templateDesign.Id),
                          UploadedDate = templateDesign.UploadedDate
                      });

                int RecordCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllTemplateDesigns()", ex);
            }

            return rtnValue;
        }

        public static string GetTemplateDesignRecordIds(int firmId, int productId, string searchText)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetTemplateDesignRecordIds() Parameters = firmId: {0}, productId: {1}, searchText: {2}", firmId, productId, searchText);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesigns> templateDesigns = entity.TemplateDesigns;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from templateDesign in templateDesigns
                      join firmType in firmTypes on templateDesign.FirmType equals firmType.Id
                      join productType in productTypes on templateDesign.ProductType equals productType.Id
                      orderby templateDesign.Id
                      select new
                      {
                          ID = templateDesign.Id,
                          Name = templateDesign.Name,
                          ShortDescription = templateDesign.ShortDescription,
                          LongDescription = templateDesign.LongDescription,
                          FirmTypeId = templateDesign.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = templateDesign.ProductType,
                          ProductName = productType.ProductName
                      });

                // filter by firm
                if (firmId != 0)
                {
                    objects = objects.Where(e => e.FirmTypeId == firmId);
                }

                // filter by product
                if (productId != 0)
                {
                    objects = objects.Where(e => e.ProductTypeId == productId);
                }

                // filter by search text
                if (searchText != "")
                {
                    objects = objects.Where(e => e.Name.ToUpper().Contains(searchText.ToUpper()));
                }

                // get only the ids
                var Obj =
                     (from Obje in objects
                      orderby Obje.ID
                      select new
                      {
                          ID = Obje.ID
                      });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(Obj);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetTemplateDesignRecordIds()", ex);
            }

            return rtnValue;
        }

        public static string GetTemplateDesignDetails(int id)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetTemplateDesignDetails() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesigns> templateDesigns = entity.TemplateDesigns;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                    (from templateDesign in templateDesigns
                     join firmType in firmTypes on templateDesign.FirmType equals firmType.Id
                     join productType in productTypes on templateDesign.ProductType equals productType.Id
                     where templateDesign.Id == id
                     select new
                     {
                         ID = templateDesign.Id,
                         Name = templateDesign.Name,
                         ShortDescription = templateDesign.ShortDescription,
                         LongDescription = templateDesign.LongDescription,
                         FirmTypeId = templateDesign.FirmType,
                         FirmName = firmType.FirmName,
                         ProductTypeId = templateDesign.ProductType,
                         ProductName = productType.ProductName
                     });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetTemplateDesignDetails()", ex);
            }

            return rtnValue;
        }

        public static string GetTemplateDesignPages(int id)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetTemplateDesignPages() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesignPages> templateDesignPages = entity.TemplateDesignPages;

                var objects =
                    (from templateDesignPage in templateDesignPages
                     orderby templateDesignPage.PageNo
                     where templateDesignPage.TemplateDesignId == id
                     select new
                     {
                         ID = templateDesignPage.Id,
                         PageNo = templateDesignPage.PageNo,
                         TemplateDesignId = templateDesignPage.TemplateDesignId
                     });

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetTemplateDesignPages()", ex);
            }

            return rtnValue;
        }

        public static bool SaveTemplateDesigns(byte[] PowerPointFile, string Name, string ShortDescription, string LongDescription, Guid UploadedBy, string ProductType, string FirmType, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight, string Preview)
        {
            message = string.Format("SaveTemplateDesigns() Parameters = PowerPointFile: {0}, Name: {1}, ShortDescription: {2}, LongDescription: {3}, UploadedBy: {4}, ProductType: {5}, FirmType: {6}, PageImageWidth: {7}, PageImageHeight: {8}, ThumbImageWidth: {9}, ThumbImageHeight: {10}, Preview: {11}", PowerPointFile.Length.ToString(), Name, ShortDescription, LongDescription, UploadedBy, ProductType, FirmType, PageImageWidth.ToString(), PageImageHeight.ToString(), ThumbImageWidth.ToString(), ThumbImageHeight.ToString(), Preview);
            Log.Info(message);

            try
            {
                List<PowerpointImage> slideList = new List<PowerpointImage>();

                Stream FileStream = new MemoryStream(PowerPointFile);
                Presentation pres = new Presentation(FileStream);

                foreach (Slide slide in pres.Slides)
                {
                    PowerpointImage powerpointImage = new PowerpointImage();

                    powerpointImage.slideImage = GetSlideImage(slide, PageImageWidth, PageImageHeight);
                    powerpointImage.thumbnailImage = GetSlideImage(slide, ThumbImageWidth, ThumbImageHeight);
                    powerpointImage.pageNo = slide.SlidePosition;

                    //string path = @"G:\images\" + idx + "_" + slide.SlidePosition.ToString() + ".bmp";
                    //Image bitmap = GetSlideImage(slide, PageImageWidth, PageImageHeight);
                    //bitmap.Save(path);  

                    slideList.Add(powerpointImage);
                }

                var entity = GetEntity();

                TemplateDesigns report = new TemplateDesigns();

                report.LongDescription = LongDescription.Trim();
                report.Name = Name.Trim();
                report.PresentationFile = PowerPointFile;
                report.ShortDescription = ShortDescription.Trim();
                report.UploadedBy = UploadedBy;
                report.UploadedDate = DateTime.Now;
                report.ViewCount = 0;
                report.ProductType = int.Parse(ProductType);
                report.FirmType = int.Parse(FirmType);

                if (Preview == "1")
                {
                    report.PreRegisterPreView = true;
                }
                else
                {
                    report.PreRegisterPreView = false;
                }

                TemplateDesignPages pages;

                foreach (PowerpointImage powerpointImage in slideList)
                {
                    pages = new TemplateDesignPages();

                    pages.PageImage = (Byte[])new ImageConverter().ConvertTo(powerpointImage.slideImage, typeof(Byte[]));
                    pages.Thumbnail = (Byte[])new ImageConverter().ConvertTo(powerpointImage.thumbnailImage, typeof(Byte[]));
                    pages.PageNo = short.Parse(powerpointImage.pageNo.ToString());
                    pages.ViewCount = 0;

                    report.GLRY_TemplateDesignPages.Add(pages);
                }

                entity.TemplateDesigns.AddObject(report);
                entity.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("SaveTemplateDesigns()", ex);

                return false;
            }
        }

        public static string GetTemplates(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            Helper hlpr = new Helper();

            int recordCount = 0;

            DataTable tbl = GetTemplateDesignData(sidx, sord, page, rows, search, searchField, searchString, searchOperator, out recordCount);

            return hlpr.JsonForJqgrid(tbl, rows, recordCount, page);
        }

        public static DataTable GetTemplateDesignData(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator, out int recordCount)
        {
            DataTable rtnValue = new DataTable();

            message = string.Format("GetTemplateDesignData() Parameters = sidx: {0}, sord: {1}, page: {2}, rows: {3}", sidx, sord, page.ToString(), rows.ToString());
            Log.Info(message);

            int RecordCount = 0;

            int initialRecordCount = rows;
            int startRow = (page - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesigns> templateDesigns = entity.TemplateDesigns;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from templateDesign in templateDesigns
                      join firmType in firmTypes on templateDesign.FirmType equals firmType.Id
                      join productType in productTypes on templateDesign.ProductType equals productType.Id
                      orderby templateDesign.Id
                      select new
                      {
                          ID = templateDesign.Id,
                          Name = templateDesign.Name,
                          ShortDescription = templateDesign.ShortDescription,
                          LongDescription = templateDesign.LongDescription,
                          FirmTypeId = templateDesign.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = templateDesign.ProductType,
                          ProductName = productType.ProductName,
                          PreRegisterPreView = templateDesign.PreRegisterPreView
                      });

                RecordCount = objects.Count();

                if (search == true)
                {
                    // filter by ID
                    if (searchField == "Id")
                    {
                        int Id = 0;
                        bool valid = Int32.TryParse(searchString, out Id);

                        if (!valid)
                        {
                            Id = 0;
                        }

                        objects = objects.Where(e => e.ID == Id);
                    }

                    RecordCount = objects.Count();

                    // filter by Name
                    if (searchField == "Name")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.Name.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.Name.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by ShortDescription
                    if (searchField == "ShortDescription")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.ShortDescription.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.ShortDescription.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by LongDescription
                    if (searchField == "LongDescription")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.LongDescription.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.LongDescription.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by FirmType
                    if (searchField == "FirmType")
                    {
                        int FirmTypeId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.FirmTypeId == FirmTypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            objects = objects.Where(e => e.FirmTypeId != FirmTypeId);
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by ProductType
                    if (searchField == "ProductType")
                    {
                        int ProductTypeId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.ProductTypeId == ProductTypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            objects = objects.Where(e => e.ProductTypeId != ProductTypeId);
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by PreRegisterPreView
                    if (searchField == "PreRegisterPreView")
                    {
                        bool PreRegisterPreView = bool.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.PreRegisterPreView == PreRegisterPreView);
                        }

                        if (searchOperator == "ne")
                        {
                            objects = objects.Where(e => e.PreRegisterPreView != PreRegisterPreView);
                        }
                    }

                    RecordCount = objects.Count();
                }

                RecordCount = objects.Count();

                // sorting by Id
                if (sord == "desc")
                {
                    objects = objects.OrderByDescending(x => x.ID);
                }

                RecordCount = objects.Count();

                // paging functionality
                objects = objects.Skip(startRow).Take(initialRecordCount);

                int pagingRecordCount = objects.Count();

                // serializing
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(objects);

                // converting json string to datatable
                rtnValue = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                rtnValue = null; // error

                Log.Error("GetTemplateDesignData()", ex);
            }

            // assign record count for paging - out parameter
            recordCount = RecordCount;

            return rtnValue;
        }

        public static int GetTemplateDesignCount()
        {
            int rtnValue = 0;

            message = string.Format("GetTemplateDesignCount()");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesigns> templateDesigns = entity.TemplateDesigns;

                var objects =
                    (from templateDesign in templateDesigns
                     orderby templateDesign.UploadedDate
                     select new
                     {
                         Id = templateDesign.Id
                     });

                rtnValue = objects.Count();
            }
            catch (Exception ex)
            {
                rtnValue = -1; // error

                Log.Error("GetTemplateDesignCount()", ex);
            }

            return rtnValue;
        }

        public static string UpdateTemplateDesigns(int Id, string name, string shortDescription, string longDescription, int firmId, int productId, string preRegisterPreView)
        {
            var rtnValue = string.Empty;

            message = string.Format("UpdateTemplateDesigns() Parameters = Id: {0}, name: {1}, shortDescription: {2}, longDescription: {3}, firmId: {4}, productId: {5}, preRegisterPreView: {6}", Id.ToString(), name, shortDescription, longDescription, firmId.ToString(), productId.ToString(), preRegisterPreView);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var template = entity.TemplateDesigns.FirstOrDefault(m => m.Id == Id);

                template.Name = name.Trim();
                template.ShortDescription = shortDescription.Trim();
                template.LongDescription = longDescription.Trim();
                template.FirmType = firmId;
                template.ProductType = productId;

                if (preRegisterPreView == "True")
                {
                    template.PreRegisterPreView = true;
                }
                else
                {
                    template.PreRegisterPreView = false;
                }

                rtnValue = entity.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("UpdateTemplateDesigns()", ex);
            }

            return rtnValue;
        }

        public static string DeleteTemplateDesign(int Id)
        {
            var rtnValue = string.Empty;

            message = string.Format("DeleteTemplateDesign() Parameters = Id: {0}", Id.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesignPages> templateDesignPages = entity.TemplateDesignPages;

                // delete template design pages
                var object_pages =
                    (from templateDesignPage in templateDesignPages
                     where templateDesignPage.TemplateDesignId == Id
                     select templateDesignPage);

                foreach (TemplateDesignPages object_page in object_pages)
                {
                    entity.TemplateDesignPages.DeleteObject(object_page);
                }

                // delete template design
                ObjectSet<TemplateDesigns> templateDesigns = entity.TemplateDesigns;

                var object_report =
                    (from templateDesign in templateDesigns
                     where templateDesign.Id == Id
                     select templateDesign).First();

                entity.TemplateDesigns.DeleteObject(object_report);

                // save entity
                entity.SaveChanges();

                rtnValue = "1"; // success
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("DeleteTemplateDesign()", ex);
            }

            return rtnValue;
        }

        public static string EditTemplateDesignUploadedFile(int Id, byte[] PowerPointFile, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight)
        {
            var rtnValue = string.Empty;

            message = string.Format("EditTemplateDesignUploadedFile() Parameters = FileId: {0}, PowerPointFile: {1}, PageImageWidth: {2}, PageImageHeight: {3}, ThumbImageWidth: {4}, ThumbImageHeight: {5}", Id.ToString(), PowerPointFile.Length.ToString(), PageImageWidth.ToString(), PageImageHeight.ToString(), ThumbImageWidth.ToString(), ThumbImageHeight.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                // template design update
                var templateDesign = entity.TemplateDesigns.FirstOrDefault(m => m.Id == Id);

                templateDesign.PresentationFile = PowerPointFile;
                templateDesign.UploadedDate = DateTime.Now;

                entity.SaveChanges();

                // delete template design pages
                ObjectSet<TemplateDesignPages> templateDesignPages = entity.TemplateDesignPages;

                var object_pages =
                    (from templateDesignPage in templateDesignPages
                     where templateDesignPage.TemplateDesignId == Id
                     select templateDesignPage);

                foreach (TemplateDesignPages object_page in object_pages)
                {
                    entity.TemplateDesignPages.DeleteObject(object_page);
                }

                entity.SaveChanges();

                // template design page update
                ArrayList pageImagesList = new ArrayList();
                ArrayList thumbImagesList = new ArrayList();

                Stream FileStream = new MemoryStream(PowerPointFile);
                Presentation pres = new Presentation(FileStream);

                foreach (Slide slide in pres.Slides)
                {
                    pageImagesList.Add(GetSlideImage(slide, PageImageWidth, PageImageHeight));
                    thumbImagesList.Add(GetSlideImage(slide, ThumbImageWidth, ThumbImageHeight));
                }

                TemplateDesignPages pages;

                int i = 0;

                foreach (System.Drawing.Image image in pageImagesList)
                {
                    pages = new TemplateDesignPages();

                    pages.PageImage = (Byte[])new ImageConverter().ConvertTo(image, typeof(Byte[]));
                    pages.Thumbnail = (Byte[])new ImageConverter().ConvertTo(thumbImagesList[i], typeof(Byte[]));
                    pages.PageNo = Convert.ToInt16(i + 1);
                    pages.ViewCount = 0;
                    pages.TemplateDesignId = Id;

                    i = i + 1;

                    entity.TemplateDesignPages.AddObject(pages);
                }

                rtnValue = entity.SaveChanges().ToString();

                rtnValue = "1";
            }
            catch (Exception ex)
            {
                Log.Error("EditTemplateDesignUploadedFile()", ex);

                rtnValue = "-1";
            }

            return rtnValue;
        }

        public static string GetTemplateDesignsForPreView()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetTemplateDesignsForPreView() Parameters = no parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<TemplateDesigns> templateDesigns = entity.TemplateDesigns;
                ObjectSet<FirmTypes> firmTypes = entity.FirmTypes;
                ObjectSet<ProductTypes> productTypes = entity.ProductTypes;

                var objects =
                     (from templateDesign in templateDesigns
                      join firmType in firmTypes on templateDesign.FirmType equals firmType.Id
                      join productType in productTypes on templateDesign.ProductType equals productType.Id
                      where templateDesign.PreRegisterPreView == true
                      orderby templateDesign.Id
                      select new
                      {
                          ID = templateDesign.Id,
                          Name = templateDesign.Name,
                          ShortDescription = templateDesign.ShortDescription,
                          LongDescription = templateDesign.LongDescription,
                          FirmTypeId = templateDesign.FirmType,
                          FirmName = firmType.FirmName,
                          ProductTypeId = templateDesign.ProductType,
                          ProductName = productType.ProductName,
                          PreRegisterPreView = templateDesign.PreRegisterPreView
                      });

                int RecordCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetTemplateDesignsForPreView()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetAllTemplateDesignsForPreView()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetAllTemplateDesignsForPreView() Parameters = no parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var templateDesigns = entity.TemplateDesigns.Where(tem => tem.PreRegisterPreView == true).OrderBy( tem => tem.Id);
                var templateDesignPages = entity.TemplateDesignPages;

                var objects =
                     (from templateDesign in templateDesigns
                      select new
                      {
                          ID = templateDesign.Id,
                          Name = templateDesign.Name,
                          ShortDescription = templateDesign.ShortDescription,
                          LongDescription = templateDesign.LongDescription,
                          FirmTypeId = templateDesign.FirmType,
                          FirmName = templateDesign.GLRY_FirmTypes.FirmName,
                          ProductTypeId = templateDesign.ProductType,
                          ProductName = templateDesign.GLRY_ProductTypes.ProductName,
                          PreRegisterPreView = templateDesign.PreRegisterPreView,
                          PageId = templateDesignPages.OrderBy(p => p.Id).FirstOrDefault(i => i.TemplateDesignId == templateDesign.Id).Id,
                          NoOfPages = templateDesignPages.Count(i => i.TemplateDesignId == templateDesign.Id),
                          UploadedDate = templateDesign.UploadedDate
                      });

                int RecordCount = objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllTemplateDesignsForPreView()", ex);
            }

            return rtnValue;
        }

        #endregion


        #region MY GALLERY

        public static string GetMyGalleryPAObjects(int pageIndex, int categoryId, string clientType, string searchText, int type, Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetMyGalleryPAObjects() Parameters = pageIndex: {0}, categoryId: {1}, clientType: {2}, searchText: {3}, type: {4}, userId: {5}", pageIndex, categoryId, clientType, searchText, type, userId);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;
                ObjectSet<PAObjectMockup> pAObjectMockups = entity.PAObjectMockups1;
                ObjectSet<GalleryNotes> galleryNotes = entity.GalleryNotes;

                int RecordCount = 0;

                // get pa-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsPA =
                     (from pAObject in pAObjects
                      join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                      join prospectsGalleryContent in prospectsGalleryContents on pAObject.ID equals prospectsGalleryContent.ObjectId
                      where prospectsGalleryContent.PropsectId == userId && prospectsGalleryContent.TableType == "p"
                      orderby prospectsGalleryContent.Id
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObjectType.Type,
                          GalleryContentId = prospectsGalleryContent.Id,
                          UserId = prospectsGalleryContent.PropsectId,
                          NoteCount = galleryNotes.Count(i => i.GalleryContentId == prospectsGalleryContent.Id),
                          ModifiedDate = pAObject.ModifiedDate,
                          ObjectTableType = "p"
                      });

                RecordCount = ObjectsPA.Count();

                // get pa-objects-mockups ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsPAM =
                     (from pAObjectMockup in pAObjectMockups
                      join pAObjectCategory in pAObjectCategories on pAObjectMockup.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObjectMockup.ObjectTypeID equals pAObjectType.ID
                      join prospectsGalleryContent in prospectsGalleryContents on pAObjectMockup.ID equals prospectsGalleryContent.ObjectId
                      where prospectsGalleryContent.PropsectId == userId && prospectsGalleryContent.TableType == "m"
                      orderby prospectsGalleryContent.Id
                      select new
                      {
                          ID = pAObjectMockup.ID,
                          Name = pAObjectMockup.Name,
                          Description = pAObjectMockup.Description,
                          ClientType = pAObjectMockup.ClientType,
                          CategoryId = pAObjectMockup.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObjectMockup.ObjectTypeID,
                          Type = pAObjectType.Type,
                          GalleryContentId = prospectsGalleryContent.Id,
                          UserId = prospectsGalleryContent.PropsectId,
                          NoteCount = galleryNotes.Count(i => i.GalleryContentId == prospectsGalleryContent.Id),
                          ModifiedDate = pAObjectMockup.UploadedDate,
                          ObjectTableType = "m"
                      });

                RecordCount = ObjectsPAM.Count();

                // join 2 results
                var Objects = ObjectsPA.Union(ObjectsPAM);

                RecordCount = Objects.Count();

                // sorting: 
                // 1 - category
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                Objects = Objects.OrderBy(obj => obj.CategoryId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                // filter by category
                if (categoryId != 0)
                {
                    Objects = Objects.Where(e => e.CategoryId == categoryId);
                }

                // filter by client type
                if (clientType != "0")
                {
                    Objects = Objects.Where(e => e.ClientType == clientType);
                }

                // filter by search text
                if (searchText != "")
                {
                    Objects = Objects.Where(e => e.Name.ToUpper().Contains(searchText.ToUpper()));
                }

                // filter by type
                if (type != 0)
                {
                    Objects = Objects.Where(e => e.TypeId == type);
                }

                // sorting: 
                // 1 - category
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                Objects = Objects.OrderBy(obj => obj.CategoryId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(Objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetMyGalleryPAObjects()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetAllMyGalleryPAObjects(Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetAllMyGalleryPAObjects() Parameters = userId: {0}", userId.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;
                ObjectSet<GalleryNotes> galleryNotes = entity.GalleryNotes;
                ObjectSet<PAObjectMockup> pAObjectMockups = entity.PAObjectMockups1;

                int RecordCount = 0;

                // get pa-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsPA =
                     (from pAObject in pAObjects
                      join prospectsGalleryContent in prospectsGalleryContents on pAObject.ID equals prospectsGalleryContent.ObjectId
                      where prospectsGalleryContent.PropsectId == userId && prospectsGalleryContent.TableType == "p"
                      orderby prospectsGalleryContent.Id
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          ClientType = pAObject.ClientType,
                          CategoryId = pAObject.ObjectCategoryID,
                          Category = pAObject.paObjectCategory.Category,
                          TypeId = pAObject.ObjectTypeID,
                          Type = pAObject.paObjectType.Type,
                          GalleryContentId = prospectsGalleryContent.Id,
                          UserId = prospectsGalleryContent.PropsectId,
                          NoteCount = galleryNotes.Count(i => i.GalleryContentId == prospectsGalleryContent.Id),
                          ModifiedDate = pAObject.ModifiedDate,
                          ObjectTableType = "p"
                      });

                RecordCount = ObjectsPA.Count();

                // get pa-objects-mockups ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var ObjectsPAM =
                     (from pAObjectMockup in pAObjectMockups
                      join prospectsGalleryContent in prospectsGalleryContents on pAObjectMockup.ID equals prospectsGalleryContent.ObjectId
                      where prospectsGalleryContent.PropsectId == userId && prospectsGalleryContent.TableType == "m"
                      orderby prospectsGalleryContent.Id
                      select new
                      {
                          ID = pAObjectMockup.ID,
                          Name = pAObjectMockup.Name,
                          Description = pAObjectMockup.Description,
                          ClientType = pAObjectMockup.ClientType,
                          CategoryId = pAObjectMockup.ObjectCategoryID,
                          Category = pAObjectMockup.paObjectCategory.Category,
                          TypeId = pAObjectMockup.ObjectTypeID,
                          Type = pAObjectMockup.paObjectType.Type,
                          GalleryContentId = prospectsGalleryContent.Id,
                          UserId = prospectsGalleryContent.PropsectId,
                          NoteCount = galleryNotes.Count(i => i.GalleryContentId == prospectsGalleryContent.Id),
                          ModifiedDate = pAObjectMockup.UploadedDate,
                          ObjectTableType = "m"
                      });

                RecordCount = ObjectsPAM.Count();

                // join 2 results
                var Objects = ObjectsPA.Union(ObjectsPAM);

                RecordCount = Objects.Count();

                // sorting: 
                // 1 - category
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                Objects = Objects.OrderBy(obj => obj.CategoryId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(Objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetAllMyGalleryPAObjects()", ex);
            }

            return rtnValue;
        }

        public static string GetMyGalleryPAObjectDetails(int id, string type)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetMyGalleryPAObjectDetails() Parameters = id: {0}, type: {1}", id, type);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObject> pAObjects = entity.PAObjects1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;
                ObjectSet<PAObjectMockup> pAObjectMockups = entity.PAObjectMockups1;

                if (type == "p")
                {
                    var objects =
                         (from pAObject in pAObjects
                          join pAObjectCategory in pAObjectCategories on pAObject.ObjectCategoryID equals pAObjectCategory.ID
                          join pAObjectType in pAObjectTypes on pAObject.ObjectTypeID equals pAObjectType.ID
                          join prospectsGalleryContent in prospectsGalleryContents on pAObject.ID equals prospectsGalleryContent.ObjectId
                          where prospectsGalleryContent.Id == id && prospectsGalleryContent.TableType == type
                          select new
                          {
                              ID = pAObject.ID,
                              Name = pAObject.Name,
                              Description = pAObject.Description,
                              ClientType = pAObject.ClientType,
                              CategoryId = pAObject.ObjectCategoryID,
                              Category = pAObjectCategory.Category,
                              TypeId = pAObject.ObjectTypeID,
                              Type = pAObjectType.Type,
                              GalleryContentId = prospectsGalleryContent.Id,
                              UserId = prospectsGalleryContent.PropsectId,
                              ModifiedDate = pAObject.ModifiedDate
                          });

                    var serializer = new JavaScriptSerializer();
                    rtnValue = serializer.Serialize(objects);
                }
                else
                {
                    var objects =
                         (from pAObjectMockup in pAObjectMockups
                          join pAObjectCategory in pAObjectCategories on pAObjectMockup.ObjectCategoryID equals pAObjectCategory.ID
                          join pAObjectType in pAObjectTypes on pAObjectMockup.ObjectTypeID equals pAObjectType.ID
                          join prospectsGalleryContent in prospectsGalleryContents on pAObjectMockup.ID equals prospectsGalleryContent.ObjectId
                          where prospectsGalleryContent.Id == id && prospectsGalleryContent.TableType == type
                          select new
                          {
                              ID = pAObjectMockup.ID,
                              Name = pAObjectMockup.Name,
                              Description = pAObjectMockup.Description,
                              ClientType = pAObjectMockup.ClientType,
                              CategoryId = pAObjectMockup.ObjectCategoryID,
                              Category = pAObjectCategory.Category,
                              TypeId = pAObjectMockup.ObjectTypeID,
                              Type = pAObjectType.Type,
                              GalleryContentId = prospectsGalleryContent.Id,
                              UserId = prospectsGalleryContent.PropsectId,
                              ModifiedDate = pAObjectMockup.UploadedDate
                          });

                    var serializer = new JavaScriptSerializer();
                    rtnValue = serializer.Serialize(objects);
                }
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetMyGalleryPAObjectDetails()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string AddToMyGallery(int id, string type, Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("AddToMyGallery() Parameters = id: {0}, type: {1}, userId: {1}", id, type, userId);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var record = entity.ProspectsGalleryContents.Any(m => m.ObjectId == id && m.PropsectId == userId && m.TableType == type);

                if (record == true)
                {
                    rtnValue = "-2"; // record exists
                }
                else
                {
                    //rtnValue = "0"; // no record found

                    var content = new ProspectsGalleryContents();

                    content.PropsectId = userId;
                    content.ObjectId = id;
                    content.TableType = type;
                    content.AddedDate = DateTime.Now;

                    entity.AddToProspectsGalleryContents(content);
                    rtnValue = entity.SaveChanges().ToString();
                }
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("AddToMyGallery()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string RemoveFromMyGallery(int id, string type)
        {
            var rtnValue = string.Empty;

            message = string.Format("RemoveFromMyGallery() Parameters = id: {0}, type: {1}", id, type);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<GalleryNotes> galleryNotes = entity.GalleryNotes;

                // delete notes
                var object_notes =
                    (from galleryNote in galleryNotes
                     where galleryNote.GalleryContentId == id
                     select galleryNote);

                foreach (GalleryNotes object_note in object_notes)
                {
                    entity.GalleryNotes.DeleteObject(object_note);
                }

                // delete contents
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;

                var object_contents =
                    (from prospectsGalleryContent in prospectsGalleryContents
                     where prospectsGalleryContent.Id == id && prospectsGalleryContent.TableType == type
                     select prospectsGalleryContent).First();

                entity.ProspectsGalleryContents.DeleteObject(object_contents);

                // save entity
                entity.SaveChanges();

                rtnValue = "1"; // success
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("RemoveFromMyGallery()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string RemoveFromMyGallery(int objectId, string type, Guid userId)
        {
            var rtnValue = string.Empty;

            message = string.Format("RemoveFromMyGallery() Parameters = objectId: {0}, type: {1}, userId: {2}", objectId, type, userId.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                // get contentgalleryid
                ObjectSet<ProspectsGalleryContents> prospectsGalleryContents = entity.ProspectsGalleryContents;

                var content =
                    (from prospectsGalleryContent in prospectsGalleryContents
                     where prospectsGalleryContent.ObjectId == objectId && prospectsGalleryContent.PropsectId == userId && prospectsGalleryContent.TableType == type
                     select prospectsGalleryContent).FirstOrDefault();

                // delete notes
                ObjectSet<GalleryNotes> galleryNotes = entity.GalleryNotes;

                var object_notes =
                    (from galleryNote in galleryNotes
                     where galleryNote.GalleryContentId == content.Id
                     select galleryNote);

                foreach (GalleryNotes object_note in object_notes)
                {
                    entity.GalleryNotes.DeleteObject(object_note);
                }

                // delete contents
                var object_contents =
                    (from prospectsGalleryContent in prospectsGalleryContents
                     where prospectsGalleryContent.Id == content.Id
                     select prospectsGalleryContent).First();

                entity.ProspectsGalleryContents.DeleteObject(object_contents);

                // save entity
                entity.SaveChanges();

                rtnValue = "1"; // success
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("RemoveFromMyGallery()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string AddNotes(int id, string note)
        {
            var rtnValue = string.Empty;

            message = string.Format("AddNotes() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var galleryNotes = new GalleryNotes();

                galleryNotes.Note = note.Trim();
                galleryNotes.GalleryContentId = id;
                galleryNotes.AddedDate = DateTime.Now;

                var entity = GetEntity();

                entity.AddToGalleryNotes(galleryNotes);
                entity.SaveChanges().ToString();

                // get newly created row
                rtnValue = galleryNotes.Id.ToString();
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("AddNotes()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string RemoveNotes(int id)
        {
            var rtnValue = string.Empty;

            message = string.Format("RemoveNotes() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<GalleryNotes> galleryNotes = entity.GalleryNotes;

                var objects =
                    (from galleryNote in galleryNotes
                     where galleryNote.Id == id
                     select galleryNote).First();

                entity.GalleryNotes.DeleteObject(objects);
                entity.SaveChanges();

                rtnValue = "1"; // sucess
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("RemoveNotes()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetNotes(int id)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetNotes() Parameters = Id: {0}", id);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<GalleryNotes> galleryNotes = entity.GalleryNotes;

                var objects =
                    (from galleryNote in galleryNotes
                     where galleryNote.GalleryContentId == id
                     orderby galleryNote.Id
                     select new
                     {
                         ID = galleryNote.Id,
                         Note = galleryNote.Note,
                         GalleryContentId = galleryNote.GalleryContentId
                     });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetNotes()", ex);
            }

            return rtnValue;
        }

        #endregion


        #region OTHER

        class PowerpointImage
        {
            public System.Drawing.Image slideImage { get; set; }
            public System.Drawing.Image thumbnailImage { get; set; }
            public int pageNo { get; set; }
        }

        // working one
        public static string GetFirmTypes()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                var firmTypes = entity.FirmTypes.OrderBy(typ => typ.Id);

                var objects =
                    (from firmType in firmTypes
                     select new
                     {
                         ID = firmType.Id,
                         Name = firmType.FirmName
                     });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);

                // testing purpose
                //throw new System.ArgumentException("Parameter cannot be null", "original");
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetFirmTypes()", ex);
            }

            return rtnValue;
        }

        // working one
        public static string GetProductTypes()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                var productTypes = entity.ProductTypes.OrderBy(typ => typ.Id);

                var objects =
                    (from productType in productTypes
                     select new
                     {
                         ID = productType.Id,
                         Name = productType.ProductName
                     });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetProductTypes()", ex);
            }

            return rtnValue;
        }

        public static byte[] GetImage(string Id, string Type, string PageNo)
        {
            byte[] rtnValue = new byte[0];

            message = string.Format("GetImage() Parameters = Id: {0}, Type: {1}, PageNo: {2}", Id, Type, PageNo);
            Log.Info(message);

            Int64 ID = Int64.Parse(Id);

            try
            {
                var entity = GetEntity();

                if (Type == "r_i") // report object - image
                {
                    var objects = (from paObject in entity.PAObjects1
                                   where paObject.ID == ID
                                   select paObject).Single();

                    if (objects.ObjectImage == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.ObjectImage.ToArray();
                    }
                }
                else if (Type == "r_t") // report object - thumbnail
                {
                    var objects = (from paObject in entity.PAObjects1
                                   where paObject.ID == ID
                                   select paObject).Single();

                    if (objects.ObjectThumbnail == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.ObjectThumbnail.ToArray();
                    }
                }
                else if (Type == "s_i") // samples - image
                {
                    short shortPageNo = short.Parse(PageNo);

                    var objects = (from sampleReportPage in entity.SampleReportPages
                                   where sampleReportPage.SampleReportId == ID && sampleReportPage.PageNo == shortPageNo
                                   select sampleReportPage).Single();

                    if (objects.PageImage == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.PageImage.ToArray();
                    }
                }
                else if (Type == "s_t") // samples - thumbnail
                {
                    var objects = (from sampleReportPage in entity.SampleReportPages
                                   where sampleReportPage.SampleReportId == ID
                                   && sampleReportPage.PageNo == 1
                                   select sampleReportPage).Single();

                    if (objects.Thumbnail == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.Thumbnail.ToArray();
                    }
                }
                else if (Type == "t_i") // template - image
                {
                    short shortPageNo = short.Parse(PageNo);

                    var objects = (from templateDesignPage in entity.TemplateDesignPages
                                   where templateDesignPage.TemplateDesignId == ID && templateDesignPage.PageNo == shortPageNo
                                   select templateDesignPage).Single();

                    if (objects.PageImage == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.PageImage.ToArray();
                    }
                }
                else if (Type == "t_t") // template - thumbnail
                {
                    var objects = (from templateDesignPage in entity.TemplateDesignPages
                                   where templateDesignPage.TemplateDesignId == ID
                                   && templateDesignPage.PageNo == 1
                                   select templateDesignPage).Single();

                    if (objects.Thumbnail == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.Thumbnail.ToArray();
                    }
                }
                else if (Type == "m_i") // mockup - image
                {
                    var objects = (from pAObjectMockup in entity.PAObjectMockups1
                                   where pAObjectMockup.ID == ID
                                   select pAObjectMockup).Single();

                    if (objects.ObjectImage == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.ObjectImage.ToArray();
                    }
                }
                else if (Type == "m_t") // mockup - thumbnail
                {
                    var objects = (from pAObjectMockup in entity.PAObjectMockups1
                                   where pAObjectMockup.ID == ID
                                   select pAObjectMockup).Single();

                    if (objects.ObjectThumbnail == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.ObjectThumbnail.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                rtnValue = new byte[0];  // error

                Log.Error("GetImage()", ex);
            }

            return rtnValue;
        }

        public static byte[] GetSlideImage(string Id, string PageNo)
        {
            byte[] rtnValue = new byte[0];


            message = string.Format("GetSlideImage() Parameters = Id: {0}, PageNo: {1}", Id, PageNo);
            Log.Info(message);

            Int64 ID = Int64.Parse(Id);
            Int64 PageNumber = Int64.Parse(PageNo);

            try
            {
                var entity = GetEntity();

                var objects = (from sampleReportPage in entity.SampleReportPages
                               where sampleReportPage.Id == ID
                               select sampleReportPage).Single();

                if (objects.Thumbnail == null)
                {
                    rtnValue = new byte[0];
                }
                else
                {
                    rtnValue = objects.PageImage.ToArray();
                }
            }
            catch (Exception ex)
            {
                rtnValue = new byte[0];  // error

                Log.Error("GetSlideImage()", ex);
            }

            return rtnValue;
        }

        public static System.Drawing.Image GetSlideImage(Slide slide, int imageW, int imageH)
        {
            //Image image = slide.GetThumbnail(new Size(imageW, imageH));

            System.Drawing.Image image = slide.GetThumbnail(2.0, 2.0);

            image = FixedSize(image, imageW, imageH);

            return image;
        }

        static System.Drawing.Image FixedSize(System.Drawing.Image imgPhoto, int width, int height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent) - 0;
            int destHeight = (int)(sourceHeight * nPercent) - 0;

            //MyTest
            destX = ((width - destWidth) / 2); //(int)(50 * nPercent);
            destY = ((height - destHeight) / 2);//(int)(50 * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto, new System.Drawing.Rectangle(destX, destY, destWidth, destHeight), new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }

        public static string SendMail(string emailTo, string emailFrom, string emailMessage, string smtpServer)
        {
            string rtnValue = string.Empty;

            MailMessage mail = new MailMessage();

            // from
            mail.From = new MailAddress(emailFrom);

            // to
            string[] emails = emailTo.Split(',');

            foreach (string email in emails)
            {
                mail.To.Add(email.Trim());
            }

            // subject
            mail.Subject = "Assette Gallery";

            // body
            mail.Body = emailMessage;

            // smtp server
            SmtpClient SmtpServer = new SmtpClient(smtpServer);

            try
            {
                SmtpServer.Send(mail);

                rtnValue = "1";
            }
            catch (Exception ex)
            {
                Log.Error(ex);

                rtnValue = "0";
            }

            return rtnValue;
        }

        public static string GetPAObjectCategoryOrdering(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            Helper hlpr = new Helper();

            int recordCount = 0;

            DataTable tbl = GetPAObjectCategoryOrdering(sidx, sord, page, rows, search, searchField, searchString, searchOperator, out recordCount);

            return hlpr.JsonForJqgrid(tbl, rows, recordCount, page);
        }

        public static DataTable GetPAObjectCategoryOrdering(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator, out int recordCount)
        {
            DataTable rtnValue = new DataTable();

            message = string.Format("GetPAObjectCategoryOrdering() Parameters = sidx: {0}, sord: {1}, page: {2}, rows: {3}", sidx, sord, page.ToString(), rows.ToString());
            Log.Info(message);

            int RecordCount = 0;

            int initialRecordCount = rows;
            int startRow = (page - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObjectCategoryOrder> pAObjectCategoryOrders = entity.PAObjectCategoryOrders;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;

                var pAObjectCategoryOrdersCount = pAObjectCategoryOrders.Count();
                var pAObjectCategoriesCount = pAObjectCategories.Count();

                // add categories that doesn't exists
                if (pAObjectCategoryOrdersCount != pAObjectCategoriesCount)
                {
                    var pAObjectCategoriesNonExists = (from pAObjectCategorie in pAObjectCategories
                                                       where (!pAObjectCategoryOrders.Any(paoco => paoco.PaObjectCategoryId == pAObjectCategorie.ID))
                                                       select pAObjectCategorie);

                    RecordCount = pAObjectCategoriesNonExists.Count();

                    int order = pAObjectCategoryOrdersCount + 1;

                    foreach (PAObjectCategory category in pAObjectCategoriesNonExists)
                    {
                        var ent = GetEntity();

                        PAObjectCategoryOrder newPAObjectCategoryOrder = new PAObjectCategoryOrder();

                        newPAObjectCategoryOrder.PaObjectCategoryId = category.ID;
                        newPAObjectCategoryOrder.SortOrder = order;
                        newPAObjectCategoryOrder.IsVisible = false;

                        ent.AddToPAObjectCategoryOrders(newPAObjectCategoryOrder);
                        ent.SaveChanges();

                        order++;
                    }
                }

                // get categories
                var objects = (from pAObjectCategoryOrder in pAObjectCategoryOrders
                               select new
                               {
                                   Id = pAObjectCategoryOrder.Id,
                                   CategoryId = pAObjectCategoryOrder.paObjectCategory.ID,
                                   Category = pAObjectCategoryOrder.paObjectCategory.Category,
                                   SortOrder = pAObjectCategoryOrder.SortOrder,
                                   IsVisible = pAObjectCategoryOrder.IsVisible
                               });

                // order by sort order / category name
                objects = objects.OrderBy(obj => obj.SortOrder).ThenBy(obj => obj.Category);

                RecordCount = objects.Count();

                if (search == true)
                {
                    // filter by ID
                    if (searchField == "Id")
                    {
                        int Id = 0;
                        bool valid = Int32.TryParse(searchString, out Id);

                        if (!valid)
                        {
                            Id = 0;
                        }

                        objects = objects.Where(e => e.Id == Id);
                    }

                    RecordCount = objects.Count();

                    // filter by CategoryId
                    if (searchField == "CategoryId")
                    {
                        int CategoryId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.CategoryId == CategoryId);
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by Category
                    if (searchField == "Category")
                    {
                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.Category.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            objects = objects.Where(e => e.Category.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by SortOrder
                    if (searchField == "SortOrder")
                    {
                        int SortOrder = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.SortOrder == SortOrder);
                        }
                    }

                    RecordCount = objects.Count();

                    // filter by IsVisible
                    if (searchField == "IsVisible")
                    {
                        bool IsVisible = bool.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            objects = objects.Where(e => e.IsVisible == IsVisible);
                        }

                        if (searchOperator == "ne")
                        {
                            objects = objects.Where(e => e.IsVisible != IsVisible);
                        }
                    }

                    RecordCount = objects.Count();
                }

                RecordCount = objects.Count();

                // paging functionality
                objects = objects.Skip(startRow).Take(initialRecordCount);

                // serializing
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(objects);

                // converting json string to datatable
                rtnValue = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                rtnValue = null; // error

                Log.Error("GetPAObjectCategoryOrdering()", ex);
            }

            // assign record count for paging - out parameter
            recordCount = RecordCount;

            return rtnValue;
        }

        public static string UpdateCategoryOrdering(int Id, int sortOrder, string isVisible)
        {
            var rtnValue = string.Empty;

            message = string.Format("UpdateCategoryOrdering() Parameters = Id: {0}, sortOrder: {1}, isVisible: {2}", Id, sortOrder, isVisible);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                // getting the record
                var pAObjectCategoryOrder = entity.PAObjectCategoryOrders.FirstOrDefault(m => m.Id == Id);

                var oldPAObjectCategoryOrder = pAObjectCategoryOrder.SortOrder;

                if (sortOrder == oldPAObjectCategoryOrder)
                {
                    // setting the correct sort order
                    if (pAObjectCategoryOrder != null)
                    {
                        pAObjectCategoryOrder.SortOrder = sortOrder;

                        if (isVisible == "True")
                        {
                            pAObjectCategoryOrder.IsVisible = true;
                        }
                        else
                        {
                            pAObjectCategoryOrder.IsVisible = false;
                        }

                        rtnValue = entity.SaveChanges().ToString();
                    }
                }
                else
                {                
                    // setting sort order = 0 temporarily
                    if (pAObjectCategoryOrder != null)
                    {
                        pAObjectCategoryOrder.SortOrder = 0;

                        rtnValue = entity.SaveChanges().ToString();
                    }

                    // get all
                    ObjectSet<PAObjectCategoryOrder> pAObjectCategoryOrders = entity.PAObjectCategoryOrders;

                    var pAObjectCategoryOrderAll = (from pAObjectCategoryOrd in pAObjectCategoryOrders
                                                    orderby pAObjectCategoryOrd.PaObjectCategoryId
                                                    select pAObjectCategoryOrd);

                    if (sortOrder > oldPAObjectCategoryOrder)
                    {
                        foreach (PAObjectCategoryOrder cat in pAObjectCategoryOrderAll)
                        {
                            var ent = GetEntity();

                            if (cat.SortOrder <= sortOrder  && cat.SortOrder >= oldPAObjectCategoryOrder)
                            {
                                // increment by 1
                                cat.SortOrder = cat.SortOrder - 1;

                                ent.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        foreach (PAObjectCategoryOrder cat in pAObjectCategoryOrderAll)
                        {
                            var ent = GetEntity();

                            if (cat.SortOrder >= sortOrder && cat.SortOrder <= oldPAObjectCategoryOrder)
                            {
                                // increment by 1
                                cat.SortOrder = cat.SortOrder + 1;

                                ent.SaveChanges();
                            }
                        }
                    }

                    // setting the correct sort order
                    if (pAObjectCategoryOrder != null)
                    {
                        int objCount = pAObjectCategoryOrderAll.Count();

                        if (sortOrder == 0) 
                        {
                            pAObjectCategoryOrder.SortOrder = 1;
                        }
                        else if (sortOrder < objCount)
                        {
                            pAObjectCategoryOrder.SortOrder = sortOrder;
                        }
                        else
                        {
                            pAObjectCategoryOrder.SortOrder = objCount;
                        }

                        if (isVisible == "True")
                        {
                            pAObjectCategoryOrder.IsVisible = true;
                        }
                        else
                        {
                            pAObjectCategoryOrder.IsVisible = false;
                        }

                        rtnValue = entity.SaveChanges().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("UpdateCategoryOrdering()", ex);
            }

            return rtnValue;
        }

        public static byte[] DownLoadFile(string Id, string Type)
        {
            byte[] rtnValue = new byte[0];

            message = string.Format("DownLoadFile() Parameters = Id: {0}, Type: {1}", Id, Type);
            Log.Info(message);

            Int64 ID = Int64.Parse(Id);

            try
            {
                var entity = GetEntity();

                if (Type == "s") // sample reports
                {
                    var objects = (from sampleReports in entity.SampleReports
                                   where sampleReports.Id == ID
                                   select sampleReports).Single();

                    if (objects.PresentationFile == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.PresentationFile.ToArray();
                    }
                }
                else if (Type == "t") // template designs
                {
                    var objects = (from templateDesigns in entity.TemplateDesigns
                                   where templateDesigns.Id == ID
                                   select templateDesigns).Single();

                    if (objects.PresentationFile == null)
                    {
                        rtnValue = new byte[0];
                    }
                    else
                    {
                        rtnValue = objects.PresentationFile.ToArray();
                    }
                }
                else 
                {
                    // do nothing
                }             
            }
            catch (Exception ex)
            {
                rtnValue = new byte[0];  // error

                Log.Error("DownLoadFile()", ex);
            }

            return rtnValue;
        }

        public static string SendMailOnClientRegistration(string clientEmail, string smtpServer, string emailFrom, string isBodyHTML, string subject, string templateName, string emailImageEmbed, string adminEmail, string type) 
        {
            string rtnValue = string.Empty;

            message = string.Format("SendMailOnClientRegistration() Parameters = clientEmail: {0}, smtpServer: {1}, emailFrom: {2}, isBodyHTML: {3}, subject: {4}, templateName: {5}, emailImageEmbed: {6}, adminEmail: {7}, type: {8}", clientEmail, smtpServer, emailFrom, isBodyHTML, subject, templateName, emailImageEmbed, adminEmail, type);
            Log.Info(message);

            string parameterName = "GalleryDocsRootFolder";

            try
            {
                var entity = GetEntity();

                // get configuration
                var configuration = entity.ClientConfigurations.Where(m => m.ParamName.ToUpper() == parameterName.ToUpper()).FirstOrDefault();

                // get root path
                string rootFolderPath = configuration.ParamValue;

                // get client detail
                ObjectSet<Prospects> prospects = entity.Prospects;

                // get client object
                var clientObject =
                    (from prospect in prospects
                     where prospect.Email.ToUpper() == clientEmail.ToUpper()
                     orderby prospect.Id
                     select new
                     {
                         Id = prospect.Id,
                         FirstName = prospect.FirstName,
                         LastName = prospect.LastName,
                         JobTitle = prospect.JobTitle,
                         Email = prospect.Email,
                         Company = prospect.Company,
                         RegisteredIP = prospect.RegisteredIP,
                         RegisteredDate = prospect.RegisteredDate
                     });

                // set template path
                string templatePath = rootFolderPath + @"\EmailTemplates\" + templateName;

                // mail definition
                MailDefinition mailDefinition = new MailDefinition();
                mailDefinition.BodyFileName = templatePath;
                mailDefinition.From = emailFrom;
                mailDefinition.Subject = subject;

                if (isBodyHTML == "1")
                {
                    mailDefinition.IsBodyHtml = true;
                }
                else
                {
                    mailDefinition.IsBodyHtml = false;
                }

                // replace values
                ListDictionary ldReplacements = new ListDictionary();

                ldReplacements.Add("<%FirstName%>", clientObject.FirstOrDefault().FirstName);
                ldReplacements.Add("<%LastName%>", clientObject.FirstOrDefault().LastName);
                ldReplacements.Add("<%JobTitle%>", clientObject.FirstOrDefault().JobTitle);
                ldReplacements.Add("<%Email%>", clientObject.FirstOrDefault().Email);
                ldReplacements.Add("<%Company%>", clientObject.FirstOrDefault().Company);
                ldReplacements.Add("<%RegisteredIP%>", clientObject.FirstOrDefault().RegisteredIP);
                ldReplacements.Add("<%RegisteredDate%>", clientObject.FirstOrDefault().RegisteredDate.ToShortDateString());
                ldReplacements.Add("<%DirectoryPath%>", clientObject.FirstOrDefault().RegisteredDate.ToShortDateString());

                // setting email address
                string receiverEmailAddress = string.Empty;
                if (type == "client")
                {
                    receiverEmailAddress = clientObject.FirstOrDefault().Email;
                }
                else
                {
                    receiverEmailAddress = adminEmail;

                }
                string email = string.Format("{0} {1} <{2}>", clientObject.FirstOrDefault().FirstName, clientObject.FirstOrDefault().LastName, receiverEmailAddress);

                // mail message
                MailMessage mail = mailDefinition.CreateMailMessage(email, ldReplacements, new System.Web.UI.Control());

                // setting alternate view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");

                // adding image
                string[] imageList = emailImageEmbed.Split(',');

                foreach (string image in imageList)
                {
                    string imageName = image.Split('.')[0].Trim();
                    string imageExtinsion = image.Split('.')[1].Trim();

                    string imagePath = rootFolderPath + @"\Images\" + image;

                    if (File.Exists(imagePath))
                    {
                        LinkedResource logo = new LinkedResource(imagePath, new System.Net.Mime.ContentType("image/jpeg"));
                        logo.ContentId = imageName;

                        // adding image to view
                        htmlView.LinkedResources.Add(logo);
                    }
                }

                // add html view
                mail.AlternateViews.Add(htmlView);

                // smtp server
                SmtpClient SmtpServer = new SmtpClient(smtpServer);

                SmtpServer.Send(mail);

                rtnValue = "1";
            }
            catch (Exception ex)
            {
                Log.Error(ex);

                rtnValue = "0";
            }

            return rtnValue;
        }

        public static string AddTagContent(string tagContent)
        {
            var rtnValue = string.Empty;

            message = string.Format("AddTagContent() Parameters = tagContent: {0}", tagContent);
            Log.Info(message);

            string parameterName = "HeaderContent";

            try
            {
                var entity = GetEntity();

                var configuration = entity.ClientConfigurations.Where(m => m.ParamName.ToUpper() == parameterName.ToUpper()).FirstOrDefault();

                if (configuration != null)
                {
                    configuration.ParamValue = tagContent.Trim();

                    rtnValue = entity.SaveChanges().ToString();
                }
                else
                {
                    var newConfiguration = new ClientConfiguration();

                    newConfiguration.ParamName = parameterName;
                    newConfiguration.ParamValue = tagContent.Trim();
                    newConfiguration.IsApplicationVariable = 1;
                    newConfiguration.Description = "Injects contents into header tag";

                    entity.AddToClientConfigurations(newConfiguration);
                    rtnValue = entity.SaveChanges().ToString();
                }
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("AddTagContent()", ex);
            }

            return rtnValue;
        }

        public static string GetTagContent()
        {
            var rtnValue = string.Empty;

            message = string.Format("GetTagContent() No parameters passed.");
            Log.Info(message);

            string parameterName = "HeaderContent";

            try
            {
                var entity = GetEntity();

                var configuration = entity.ClientConfigurations.Where(m => m.ParamName.ToUpper() == parameterName.ToUpper());

                var objects =
                    (from config in configuration
                     select new
                     {
                         ParamValue = config.ParamValue
                     });


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetTagContent()", ex);
            }

            return rtnValue;
        }

        /*
        public static byte[] GetPAObjectDetailsForPdfByList(int[] paObjectIdList, int[] mockupObjectIdList)
        {
            byte[] rtnValue = new byte[0];

            int RecordCount = 0;

            message = string.Format("GetPAObjectDetailsForPdfByList() No parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                IQueryable<PAObjectCategoryOrder> pAObjectCategoryOrders = entity.PAObjectCategoryOrders.Where(pa => pa.IsVisible == true);

                // get pa-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var PAObjects = (from pAObject in entity.PAObjects1
                                 join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObject.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                                 where paObjectIdList.Contains(pAObject.ID)
                                 select new
                                 {
                                     ID = pAObject.ID,
                                     Name = pAObject.Name,
                                     Description = pAObject.Description,
                                     Type = pAObject.paObjectType.Type,
                                     TypeId = pAObject.ObjectTypeID,
                                     Category = pAObject.paObjectCategory.Category,
                                     CategoryId = pAObject.ObjectCategoryID,
                                     ObjectImage = pAObject.ObjectImage,
                                     ObjectTableType = "p",
                                     CategoryOrderId = pAObjectCategoryOrder.SortOrder
                                 });

                RecordCount = PAObjects.Count();

                // get mockup-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var PAObjectMokupsDoesntExists = (from pAObject in entity.PAObjectMockups1
                                                  join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObject.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                                                  where mockupObjectIdList.Contains(pAObject.ID)
                                                  select new
                                                  {
                                                      ID = pAObject.ID,
                                                      Name = pAObject.Name,
                                                      Description = pAObject.Description,
                                                      Type = pAObject.paObjectType.Type,
                                                      TypeId = pAObject.ObjectTypeID,
                                                      Category = pAObject.paObjectCategory.Category,
                                                      CategoryId = pAObject.ObjectCategoryID,
                                                      ObjectImage = pAObject.ObjectImage,
                                                      ObjectTableType = "m",
                                                      CategoryOrderId = pAObjectCategoryOrder.SortOrder
                                                  });

                RecordCount = PAObjectMokupsDoesntExists.Count();

                /// join 2 results
                var ObjectsAll = PAObjects.Concat(PAObjectMokupsDoesntExists);

                RecordCount = ObjectsAll.Count();

                // sorting: 
                // 1 - category order - PAObjectCategoryOrder
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                var Objects = ObjectsAll.OrderBy(obj => obj.CategoryOrderId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                // iTextSharp doc //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Document doc = new Document();

                // doc properties
                doc.SetPageSize(PageSize.LETTER.Rotate()); // letter size
                doc.AddTitle("ASSETTE Object Details");
                doc.AddAuthor("ASSETTE");

                // fonts
                BaseFont baseFontCalibri = BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font fontCalibri12 = FontFactory.GetFont("Calibri", 12);
                iTextSharp.text.Font fontCalibri18 = FontFactory.GetFont("Calibri", 18);
                iTextSharp.text.Font fontCalibriBody = FontFactory.GetFont("Calibri (Body)", 12, iTextSharp.text.Font.ITALIC);

                string parameterName = "GalleryDocsRootFolder";

                // get configuration
                var configuration = entity.ClientConfigurations.Where(m => m.ParamName.ToUpper() == parameterName.ToUpper()).FirstOrDefault();

                // get root path
                string rootFolderPath = configuration.ParamValue;

                // set image path
                string imagePath = rootFolderPath + @"\Images\";

                using (var memoryStream = new MemoryStream())
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream))
                    {
                        // open doc
                        doc.Open();

                        // a4 842/595
                        // note 720/540
                        // letter 792/612

                        // set page size
                        iTextSharp.text.Rectangle pageSize = iTextSharp.text.PageSize.LETTER;

                        float width = pageSize.Rotate().Width; // 792
                        float height = pageSize.Rotate().Height; // 612

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - title page ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        doc.SetMargins(0f, 0f, 0f, 0f);
                        doc.NewPage();

                        // banner image
                        iTextSharp.text.Image imgPresentationBanner = iTextSharp.text.Image.GetInstance(imagePath + @"\PresentationBanner.jpg");
                        imgPresentationBanner.ScaleAbsolute(792f, 267f); // 720/243 = 0.3375
                        imgPresentationBanner.SetAbsolutePosition(0, pageSize.Rotate().Height - 267);
                        doc.Add(imgPresentationBanner);

                        // assette large logo
                        iTextSharp.text.Image imgAssetteLogoLarge = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoLarge.jpg");
                        imgAssetteLogoLarge.ScaleAbsolute(210f, 30f); // 408/59 percentage: 0.1446
                        imgAssetteLogoLarge.SetAbsolutePosition(50, 250);
                        doc.Add(imgAssetteLogoLarge);

                        // line title
                        PdfContentByte cbTitle = writer.DirectContent;
                        cbTitle.BeginText();
                        cbTitle.SetColorFill(new BaseColor(System.Drawing.Color.DarkSeaGreen));
                        //cbTitle.SetColorFill(new CMYKColor(0.11f, 0.00f, 0.78f, 0.15f)); //#C1D82F                      
                        cbTitle.SetFontAndSize(baseFontCalibri, 32f);
                        cbTitle.SetTextMatrix(50, 185);
                        cbTitle.ShowText("Client Reporting Object Library");
                        cbTitle.EndText();

                        // line date
                        PdfContentByte cbDate = writer.DirectContent;
                        cbDate.BeginText();
                        cbDate.SetColorFill(BaseColor.GRAY);
                        cbDate.SetFontAndSize(baseFontCalibri, 18f);
                        cbDate.SetTextMatrix(50, 60);
                        cbDate.ShowText("(Updated: " + DateTime.Today.ToShortDateString() + ")");
                        cbDate.EndText();

                        // line confidential
                        PdfContentByte cbConfidentialTitle = writer.DirectContent;
                        cbConfidentialTitle.BeginText();
                        cbConfidentialTitle.SetColorFill(BaseColor.GRAY);
                        cbConfidentialTitle.SetFontAndSize(baseFontCalibri, 16f);
                        cbConfidentialTitle.SetTextMatrix(50, 40);
                        cbConfidentialTitle.ShowText("Confidential");
                        cbConfidentialTitle.EndText();

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - table of contents page ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        doc.SetMargins(0f, 0f, 0f, 0f);
                        doc.NewPage();

                        // banner image
                        iTextSharp.text.Image imgTableOfContentsBanner = iTextSharp.text.Image.GetInstance(imagePath + @"\HeaderContentsBanner.jpg");
                        imgTableOfContentsBanner.ScaleToFit(pageSize.Rotate().Width, pageSize.Rotate().Height);
                        doc.Add(imgTableOfContentsBanner);

                        // line table of contents - table of contents
                        PdfContentByte cbTableOfContents = writer.DirectContent;
                        cbTableOfContents.BeginText();
                        cbTableOfContents.SetColorFill(BaseColor.BLACK);
                        cbTableOfContents.SetFontAndSize(baseFontCalibri, 32f);
                        cbTableOfContents.SetTextMatrix(50, 460);
                        cbTableOfContents.ShowText("Table of Contents");
                        cbTableOfContents.EndText();

                        // add important notes to table of contents
                        PdfContentByte cbImportantNotesTableOfContent = writer.DirectContent;
                        cbImportantNotesTableOfContent.BeginText();
                        cbImportantNotesTableOfContent.SetColorFill(BaseColor.BLACK);
                        cbImportantNotesTableOfContent.SetFontAndSize(baseFontCalibri, 18f);
                        cbImportantNotesTableOfContent.SetTextMatrix(50, 400);
                        cbImportantNotesTableOfContent.ShowText("Important Notes");
                        cbImportantNotesTableOfContent.EndText();

                        PdfContentByte cbImportantNotesTableOfContentPageNumber = writer.DirectContent;
                        cbImportantNotesTableOfContentPageNumber.BeginText();
                        cbImportantNotesTableOfContentPageNumber.SetColorFill(BaseColor.BLACK);
                        cbImportantNotesTableOfContentPageNumber.SetFontAndSize(baseFontCalibri, 18f);
                        cbImportantNotesTableOfContentPageNumber.SetTextMatrix(650, 400);
                        cbImportantNotesTableOfContentPageNumber.ShowText("3");
                        cbImportantNotesTableOfContentPageNumber.EndText();

                        // add categories to table of contents ///////////////////////////////////////////////////////////////////////////////////////////
                        float lineHeightDifference = 30f;
                        float newLinePosition = 400f;

                        int initialPage = 4;
                        int pageNumber = 0;

                        // get distinct categories
                        var categories = (from obj in ObjectsAll
                                          select new
                                          {
                                              Category = obj.Category,
                                              CategoryOrderId = obj.CategoryOrderId,
                                              ObjectCount = Objects.Count(i => i.Category == obj.Category),
                                          }).Distinct();

                        // order the categories
                        categories = categories.OrderBy(obj => obj.CategoryOrderId);

                        //// group by
                        //var categories = (from obj in Objects
                        //                  group obj by obj.Category into distinctCategories
                        //                  select new
                        //                  {
                        //                      Category = distinctCategories.Key,
                        //                      CategoryCount = distinctCategories.Count()
                        //                  }).Distinct().OrderBy(obj => obj.Category);

                        foreach (var category in categories)
                        {
                            newLinePosition = newLinePosition - lineHeightDifference;

                            if (pageNumber == 0)
                            {
                                pageNumber = initialPage;
                            }
                            else
                            {
                                pageNumber = pageNumber + 1;
                            }

                            PdfContentByte cbCategory = writer.DirectContent;
                            cbCategory.BeginText();
                            cbCategory.SetColorFill(BaseColor.BLACK);
                            cbCategory.SetFontAndSize(baseFontCalibri, 18f);
                            cbCategory.SetTextMatrix(50, newLinePosition);
                            cbCategory.ShowText(category.Category);
                            cbCategory.EndText();

                            PdfContentByte cbCategoryNumber = writer.DirectContent;
                            cbCategoryNumber.BeginText();
                            cbCategoryNumber.SetColorFill(BaseColor.BLACK);
                            cbCategoryNumber.SetFontAndSize(baseFontCalibri, 18f);
                            cbCategoryNumber.SetTextMatrix(650, newLinePosition);
                            cbCategoryNumber.ShowText(pageNumber.ToString());
                            cbCategoryNumber.EndText();

                            // set page number
                            pageNumber = pageNumber + category.ObjectCount;
                        }

                        // footer content
                        PdfContentByte cbConfidentialTableOfContents = writer.DirectContent;
                        cbConfidentialTableOfContents.BeginText();
                        cbConfidentialTableOfContents.SetFontAndSize(baseFontCalibri, 12f);
                        cbConfidentialTableOfContents.SetTextMatrix(50, 20);
                        cbConfidentialTableOfContents.ShowText("Confidential");
                        cbConfidentialTableOfContents.EndText();

                        PdfContentByte cbPageNumberTableOfContents = writer.DirectContent;
                        cbPageNumberTableOfContents.BeginText();
                        cbPageNumberTableOfContents.SetFontAndSize(baseFontCalibri, 10f);
                        cbPageNumberTableOfContents.SetTextMatrix(400, 20);
                        cbPageNumberTableOfContents.ShowText("2");
                        cbPageNumberTableOfContents.EndText();

                        iTextSharp.text.Image imgFooterTableOfContents = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                        imgFooterTableOfContents.ScaleAbsolute(69f, 10f); // 6.88
                        imgFooterTableOfContents.SetAbsolutePosition(700, 20);
                        imgFooterTableOfContents.SpacingAfter = 10f;
                        doc.Add(imgFooterTableOfContents);

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - important notes page /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        doc.SetMargins(0f, 0f, 0f, 0f);
                        doc.NewPage();

                        // banner image
                        iTextSharp.text.Image imgImportantNotesBanner = iTextSharp.text.Image.GetInstance(imagePath + @"\HeaderContentsBanner.jpg");
                        imgImportantNotesBanner.ScaleToFit(pageSize.Rotate().Width, pageSize.Rotate().Height);
                        doc.Add(imgImportantNotesBanner);

                        // line table of contents
                        PdfContentByte cbImportantNotes = writer.DirectContent;
                        cbImportantNotes.BeginText();
                        cbImportantNotes.SetColorFill(BaseColor.BLACK);
                        cbImportantNotes.SetFontAndSize(baseFontCalibri, 32f);
                        cbImportantNotes.SetTextMatrix(50, 460);
                        cbImportantNotes.ShowText("Important Notes");
                        cbImportantNotes.EndText();

                        // note 1
                        string note1 = "1). In Assette software – Assette Reports, Assette Presentations and Assette EasyReports – the objects and templates are \nin PowerPoint format. You may select objects depicted in this library and place in templates. Objects can be placed to fit \nyour exact requirements";
                        iTextSharp.text.Paragraph para1 = new iTextSharp.text.Paragraph(note1, fontCalibri12);
                        para1.SpacingBefore = 20f;
                        para1.IndentationLeft = 50f;
                        doc.Add(para1);

                        // note 2
                        string note2 = "2). Objects are placeholders for actual data. In Assette software, you first create templates by using objects from this \nlibrary. Next, you generate reports using templates – you select a template and then specify the accounts you want to \ngenerate reports for. At generation time, actual account data is merged into the objects.";
                        iTextSharp.text.Paragraph para2 = new iTextSharp.text.Paragraph(note2, fontCalibri12);
                        para2.SpacingBefore = 10f;
                        para2.IndentationLeft = 50f;
                        doc.Add(para2);

                        // note 3
                        string note3 = "3) Certain objects in this library are available only in Assette Reports and Assette Presentations – not in EasyReports. \nObjects that are not included in Assette EasyReports are clearly marked as such on the top-right corner of the page.";
                        iTextSharp.text.Paragraph para3 = new iTextSharp.text.Paragraph(note3, fontCalibri12);
                        para3.SpacingBefore = 10f;
                        para3.IndentationLeft = 50f;
                        doc.Add(para3);

                        // note 4
                        string note4 = "4). Composite return data is available only in Assette Reports and Assette Presentations.";
                        iTextSharp.text.Paragraph para4 = new iTextSharp.text.Paragraph(note4, fontCalibri12);
                        para4.SpacingBefore = 10f;
                        para4.IndentationLeft = 50f;
                        doc.Add(para4);

                        // note 5
                        string note5 = "5). The objects depicted in the “Advanced Analytics” section are optional. The Advanced Analytics Pack is Not available in Easy Editions.";
                        iTextSharp.text.Paragraph para5 = new iTextSharp.text.Paragraph(note5, fontCalibri12);
                        para5.SpacingBefore = 10f;
                        para5.IndentationLeft = 50f;
                        doc.Add(para5);

                        // note 6
                        string note6 = "CONFIDENTIALITY NOTICE";
                        iTextSharp.text.Paragraph para6 = new iTextSharp.text.Paragraph(note6, fontCalibri12);
                        para6.SpacingBefore = 20f;
                        para6.IndentationLeft = 320f;
                        doc.Add(para6);

                        // note 7
                        string note7 = "The information contained in this document is proprietary to Assette. This document is intended solely for the \ndesignated recipient(s) and other employees of the recipient's firm. The contents of this document must not be \nshared with any other party without the written consent of Assette.";
                        iTextSharp.text.Paragraph para7 = new iTextSharp.text.Paragraph(note7, fontCalibri12);
                        para7.SpacingBefore = 15f;
                        para7.IndentationLeft = 110f;
                        doc.Add(para7);

                        // footer content - important notes
                        PdfContentByte cbConfidentialImportantNotes = writer.DirectContent;
                        cbConfidentialImportantNotes.BeginText();
                        cbConfidentialImportantNotes.SetFontAndSize(baseFontCalibri, 12f);
                        cbConfidentialImportantNotes.SetTextMatrix(50, 20);
                        cbConfidentialImportantNotes.ShowText("Confidential");
                        cbConfidentialImportantNotes.EndText();

                        PdfContentByte cbPageNumberImportantNotes = writer.DirectContent;
                        cbPageNumberImportantNotes.BeginText();
                        cbPageNumberImportantNotes.SetFontAndSize(baseFontCalibri, 10f);
                        cbPageNumberImportantNotes.SetTextMatrix(400, 20);
                        cbPageNumberImportantNotes.ShowText("3");
                        cbPageNumberImportantNotes.EndText();

                        iTextSharp.text.Image imgFooterImportantNotes = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                        imgFooterImportantNotes.ScaleAbsolute(69f, 10f); // 6.88
                        imgFooterImportantNotes.SetAbsolutePosition(700, 20);
                        imgFooterImportantNotes.SpacingAfter = 10f;
                        doc.Add(imgFooterImportantNotes);

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - asset allocation page ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        // set category
                        string CategoryName = string.Empty;
                        bool writeCategoryName = false;

                        // page number
                        int loop = 3;

                        foreach (var obj in Objects)
                        {
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            // new page - setting category name ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            if (CategoryName == string.Empty)
                            {
                                // setting category
                                CategoryName = obj.Category;

                                // increment
                                loop++;

                                writeCategoryName = true;
                            }
                            else if (CategoryName == obj.Category)
                            {
                                // don't write
                                writeCategoryName = false;
                            }
                            else
                            {
                                // setting new category
                                CategoryName = obj.Category;

                                // increment
                                loop++;

                                writeCategoryName = true;
                            }

                            if (writeCategoryName == true)
                            {
                                doc.SetMargins(0f, 0f, 0f, 0f);
                                doc.NewPage();

                                // banner image
                                iTextSharp.text.Image imgAsseteAllocation = iTextSharp.text.Image.GetInstance(imagePath + @"\HeaderContentsBanner.jpg");
                                imgAsseteAllocation.ScaleToFit(pageSize.Rotate().Width, pageSize.Rotate().Height);
                                imgAsseteAllocation.SetAbsolutePosition(0, 300);
                                doc.Add(imgAsseteAllocation);

                                // category name
                                PdfContentByte cbCategoryName = writer.DirectContent;
                                cbCategoryName.BeginText();
                                cbCategoryName.SetColorFill(BaseColor.BLACK);
                                //cb3.SetColorFill(new CMYKColor(0.11f, 0.00f, 0.78f, 0.15f)); //#C1D82F                      
                                cbCategoryName.SetFontAndSize(baseFontCalibri, 40f);
                                cbCategoryName.SetTextMatrix(40, 320);
                                cbCategoryName.ShowText(CategoryName);
                                cbCategoryName.EndText();

                                // footer content - category name
                                PdfContentByte cbConfidentialCategoryName = writer.DirectContent;
                                cbConfidentialCategoryName.BeginText();
                                cbConfidentialCategoryName.SetFontAndSize(baseFontCalibri, 12f);
                                cbConfidentialCategoryName.SetTextMatrix(50, 20);
                                cbConfidentialCategoryName.ShowText("Confidential");
                                cbConfidentialCategoryName.EndText();

                                PdfContentByte cbPageNumberCategoryName = writer.DirectContent;
                                cbPageNumberCategoryName.BeginText();
                                cbPageNumberCategoryName.SetFontAndSize(baseFontCalibri, 10f);
                                cbPageNumberCategoryName.SetTextMatrix(400, 20);
                                cbPageNumberCategoryName.ShowText(loop.ToString());
                                cbPageNumberCategoryName.EndText();

                                iTextSharp.text.Image imgFooterCategoryName = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                                imgFooterCategoryName.ScaleAbsolute(69f, 10f); // 6.88
                                imgFooterCategoryName.SetAbsolutePosition(700, 20);
                                imgFooterCategoryName.SpacingAfter = 10f;
                                doc.Add(imgFooterCategoryName);
                            }

                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            // new page - adding object to page ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            doc.SetMargins(50f, 50f, 30f, 0f);
                            doc.NewPage();

                            // increment
                            loop++;

                            // line 01                            
                            string firstLine = obj.Category + " - " + obj.Name + " (" + obj.Type + ")";
                            doc.Add(new iTextSharp.text.Paragraph(firstLine, fontCalibri18));

                            // new line
                            //doc.Add(new iTextSharp.text.Paragraph(Environment.NewLine));

                            // line 02

                            string secondLine = "Description: " + obj.Description;
                            doc.Add(new iTextSharp.text.Paragraph(secondLine, fontCalibriBody));

                            // new line
                            //doc.Add(new iTextSharp.text.Paragraph(Environment.NewLine));

                            // footer content
                            PdfContentByte cbConfidential = writer.DirectContent;
                            cbConfidential.BeginText();
                            cbConfidential.SetFontAndSize(baseFontCalibri, 12f);
                            cbConfidential.SetTextMatrix(50, 20);
                            cbConfidential.ShowText("Confidential");
                            cbConfidential.EndText();

                            PdfContentByte cbPageNumber = writer.DirectContent;
                            cbPageNumber.BeginText();
                            cbPageNumber.SetFontAndSize(baseFontCalibri, 10f);
                            cbPageNumber.SetTextMatrix(400, 20);
                            cbPageNumber.ShowText(loop.ToString());
                            cbPageNumber.EndText();

                            // footer image
                            iTextSharp.text.Image imgFooter = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                            imgFooter.ScaleAbsolute(69f, 10f); // 6.88
                            imgFooter.SetAbsolutePosition(700, 20);
                            imgFooter.SpacingAfter = 10f;
                            doc.Add(imgFooter);

                            // object image
                            // pdf max image width: 700 | height: 350
                            iTextSharp.text.Image imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                            if (imgObject.Width > 700 || imgObject.Height > 350)
                            {
                                float percentage = 0f;
                                float imgWidth = 0f;
                                float imgHeight = 0f;

                                if (imgObject.Width >= imgObject.Height)
                                {
                                    percentage = imgObject.Height / imgObject.Width;

                                    imgWidth = 518f;
                                    imgHeight = 518f * percentage;
                                }
                                else
                                {
                                    percentage = imgObject.Width / imgObject.Height;

                                    imgWidth = 518f * percentage;
                                    imgHeight = 349f;
                                }

                                imgObject.ScaleToFit(imgWidth, imgHeight);
                            }
                            else
                            {
                                // no need to scale
                                //imgObject.ScaleAbsolute(imgObject.Width, imgObject.Height);
                            }

                            // set image properties
                            imgObject.SetDpi(300, 300);

                            // add image to doc
                            doc.Add(imgObject);
                        }

                        doc.Close();
                    }

                    rtnValue = memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                rtnValue = new byte[0];  // error


                Log.Error("GetPAObjectDetailsForPdfByList()", ex);
            }

            return rtnValue;
        }
        */

        public static byte[] GetPAObjectDetailsForPdfByList(int[] paObjectIdList, int[] mockupObjectIdList)
        {
            byte[] rtnValue = new byte[0];

            return rtnValue;
        }

        public static byte[] GetAllPAObjectDetailsForPdf_itextsharp()
        {
            byte[] rtnValue = new byte[0];

            message = string.Format("GetAllPAObjectDetailsForPdf_itextsharp() No parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                IQueryable<PAObject> pAObjects = entity.PAObjects1.Where(pao => (pao.IsDeleted == false) && (pao.ObjectTypeID == 1 || pao.ObjectTypeID == 2)).OrderBy(pao => pao.ID);
                IQueryable<PAObjectMockup> pAObjectMockups = entity.PAObjectMockups1.OrderBy(pam => pam.ID);
                IQueryable<PAObjectCategoryOrder> pAObjectCategoryOrders = entity.PAObjectCategoryOrders.Where(pa => pa.IsVisible == true);

                int RecordCount = 0;

                // get pa-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var PAObjects =
                     (from pAObject in pAObjects
                      join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObject.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          Type = pAObject.paObjectType.Type,
                          TypeId = pAObject.ObjectTypeID,
                          Category = pAObject.paObjectCategory.Category,
                          CategoryId = pAObject.ObjectCategoryID,
                          ObjectImage = pAObject.ObjectImage,
                          ClientType = pAObject.ClientType,
                          ObjectTableType = "p",
                          CategoryOrderId = pAObjectCategoryOrder.SortOrder,
                          Width = pAObject.Width,
                          Height = pAObject.Height
                      });

                RecordCount = PAObjects.Count();

                // get mockup objects that doesn't exists on pa-object table
                var PAObjectMokupsDoesntExists =
                    (from pAObjectMockup in pAObjectMockups
                     join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObjectMockup.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                     where
                    (!pAObjects.Any(pao => pao.Name.Replace(" ", "").Trim().ToLower() == pAObjectMockup.Name.Replace(" ", "").Trim().ToLower() && pAObjectMockup.ObjectCategoryID == pao.ObjectCategoryID && pAObjectMockup.ObjectTypeID == pao.ObjectTypeID))
                     select new
                     {
                         ID = pAObjectMockup.ID,
                         Name = pAObjectMockup.Name,
                         Description = pAObjectMockup.Description,
                         Type = pAObjectMockup.paObjectType.Type,
                         TypeId = pAObjectMockup.ObjectTypeID,
                         Category = pAObjectMockup.paObjectCategory.Category,
                         CategoryId = pAObjectMockup.ObjectCategoryID,
                         ObjectImage = pAObjectMockup.ObjectImage,
                         ClientType = pAObjectMockup.ClientType,
                         ObjectTableType = "m",
                         CategoryOrderId = pAObjectCategoryOrder.SortOrder,
                         Width = (int?)null,
                         Height = (int?)null
                     });

                RecordCount = PAObjectMokupsDoesntExists.Count();

                // join 2 results
                var ObjectsAll = PAObjects.Concat(PAObjectMokupsDoesntExists);
                //var ObjectsAll = PAObjects.Concat(PAObjectMokupsDoesntExists).Where(obj => obj.Name.ToUpper() == "Current and Target Allocation");
                //var ObjectsAll = PAObjects.Concat(PAObjectMokupsDoesntExists).Where(obj => obj.Category.ToUpper() == "Holdings");

                RecordCount = ObjectsAll.Count();

                // sorting: 
                // 1 - category order - PAObjectCategoryOrder
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                var Objects = ObjectsAll.OrderBy(obj => obj.CategoryOrderId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // iTextSharp doc //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                iTextSharp.text.Document doc = new iTextSharp.text.Document();

                // doc properties
                doc.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate()); // letter size
                doc.AddTitle("ASSETTE Object Details");
                doc.AddAuthor("ASSETTE");

                // fonts
                BaseFont baseFontCalibri = BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font fontCalibri12 = FontFactory.GetFont("Calibri", 12);
                iTextSharp.text.Font fontCalibri14 = FontFactory.GetFont("Calibri", 12);
                iTextSharp.text.Font fontCalibri18 = FontFactory.GetFont("Calibri", 18);
                iTextSharp.text.Font fontCalibri17 = FontFactory.GetFont("Calibri", 17);
                iTextSharp.text.Font fontCalibri16 = FontFactory.GetFont("Calibri", 16);
                iTextSharp.text.Font fontCalibriBody = FontFactory.GetFont("Calibri (Body)", 11, iTextSharp.text.Font.ITALIC);
                iTextSharp.text.Font fontCalibriBody11Bold = FontFactory.GetFont("Calibri (Body)", 11, iTextSharp.text.Font.BOLD);

                string parameterName = "GalleryDocsRootFolder";

                // get configuration
                var configuration = entity.ClientConfigurations.Where(m => m.ParamName.ToUpper() == parameterName.ToUpper()).FirstOrDefault();

                // get root path
                string rootFolderPath = configuration.ParamValue;

                // set image path
                string imagePath = rootFolderPath + @"\Images\";

                using (var memoryStream = new MemoryStream())
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream))
                    {
                        // open doc
                        doc.Open();

                        // a4 842/595
                        // note 720/540
                        // letter 792/612 pixels - 8.5|11 inches

                        // set page size
                        iTextSharp.text.Rectangle pageSize = iTextSharp.text.PageSize.LETTER;

                        float width = pageSize.Rotate().Width; // 792
                        float height = pageSize.Rotate().Height; // 612

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - title page ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        doc.SetMargins(0f, 0f, 0f, 0f);
                        doc.NewPage();

                        // banner image
                        iTextSharp.text.Image imgPresentationBanner = iTextSharp.text.Image.GetInstance(imagePath + @"\PresentationBanner.jpg");
                        imgPresentationBanner.ScaleAbsolute(792f, 267f); // 720/243 = 0.3375
                        imgPresentationBanner.SetAbsolutePosition(0, pageSize.Rotate().Height - 267);
                        doc.Add(imgPresentationBanner);

                        // assette large logo
                        iTextSharp.text.Image imgAssetteLogoLarge = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoLarge.jpg");
                        imgAssetteLogoLarge.ScalePercent(130f);
                        //imgAssetteLogoLarge.ScaleAbsolute(PixelsToPoints(408f, 110), PixelsToPoints(59f, 110)); // 408/59 percentage: 0.1446
                        imgAssetteLogoLarge.SetAbsolutePosition(50, 250);
                        doc.Add(imgAssetteLogoLarge);

                        // line title
                        PdfContentByte cbTitle = writer.DirectContent;
                        cbTitle.BeginText();
                        cbTitle.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C1D82F")));
                        //cbTitle.SetColorFill(new BaseColor(System.Drawing.Color.DarkSeaGreen));
                        //cbTitle.SetColorFill(new CMYKColor(0.11f, 0.00f, 0.78f, 0.15f)); //#C1D82F  
                        //cbTitle.SetCMYKColorFillF(10.65f, 0f, 78.24f, 15.29f); //#C1D82F http://easycalculation.com/colorconverter/rgb-color-converter.php
                        cbTitle.SetFontAndSize(baseFontCalibri, 32f);
                        cbTitle.SetTextMatrix(50, 185);
                        cbTitle.ShowText("Client Reporting Object Library");
                        cbTitle.EndText();

                        // line date
                        PdfContentByte cbDate = writer.DirectContent;
                        cbDate.BeginText();
                        cbDate.SetColorFill(BaseColor.GRAY);
                        cbDate.SetFontAndSize(baseFontCalibri, 10f);
                        cbDate.SetTextMatrix(700, 40);
                        cbDate.ShowText(DateTime.Today.ToShortDateString());
                        cbDate.EndText();

                        // line confidential
                        PdfContentByte cbConfidentialTitle = writer.DirectContent;
                        cbConfidentialTitle.BeginText();
                        cbConfidentialTitle.SetColorFill(BaseColor.GRAY);
                        cbConfidentialTitle.SetFontAndSize(baseFontCalibri, 14f);
                        cbConfidentialTitle.SetTextMatrix(50, 40);
                        cbConfidentialTitle.ShowText("Confidential");
                        cbConfidentialTitle.EndText();

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - table of contents page ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        doc.SetMargins(0f, 0f, 0f, 0f);
                        doc.NewPage();

                        // banner image
                        iTextSharp.text.Image imgTableOfContentsBanner = iTextSharp.text.Image.GetInstance(imagePath + @"\HeaderContentsBanner.jpg");
                        imgTableOfContentsBanner.ScaleToFit(pageSize.Rotate().Width, pageSize.Rotate().Height);
                        doc.Add(imgTableOfContentsBanner);

                        // line table of contents - table of contents
                        PdfContentByte cbTableOfContents = writer.DirectContent;
                        cbTableOfContents.BeginText();
                        cbTableOfContents.SetColorFill(BaseColor.BLACK);                   
                        cbTableOfContents.SetFontAndSize(baseFontCalibri, 32f);
                        cbTableOfContents.SetTextMatrix(50, 460);
                        cbTableOfContents.ShowText("Table of Contents");
                        cbTableOfContents.EndText();

                        // add section/page to table of contents
                        PdfContentByte cbSectionTableOfContent = writer.DirectContent;
                        cbSectionTableOfContent.BeginText();
                        //cbSectionTableOfContent.SetColorFill(BaseColor.BLACK);
                        cbSectionTableOfContent.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C1D82F")));
                        cbSectionTableOfContent.SetFontAndSize(baseFontCalibri, 20f);
                        cbSectionTableOfContent.SetTextMatrix(50, 395);
                        cbSectionTableOfContent.ShowText("SECTION");
                        cbSectionTableOfContent.EndText();

                        PdfContentByte cbPageTableOfContentPageNumber = writer.DirectContent;
                        cbPageTableOfContentPageNumber.BeginText();
                        //cbPageTableOfContentPageNumber.SetColorFill(BaseColor.BLACK);
                        cbSectionTableOfContent.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C1D82F")));
                        cbPageTableOfContentPageNumber.SetFontAndSize(baseFontCalibri, 20f);
                        cbPageTableOfContentPageNumber.SetTextMatrix(650, 395);
                        cbPageTableOfContentPageNumber.ShowText("PAGE");
                        cbPageTableOfContentPageNumber.EndText();

                        PdfContentByte cbHorizontalLine = writer.DirectContent;
                        cbHorizontalLine.SetLineWidth(1);
                        cbHorizontalLine.SetColorFill(BaseColor.BLACK);   
                        cbHorizontalLine.MoveTo(50, 387);
                        cbHorizontalLine.LineTo(695, 387);
                        cbHorizontalLine.Stroke();

                        // add important notes to table of contents
                        PdfContentByte cbImportantNotesTableOfContent = writer.DirectContent;
                        cbImportantNotesTableOfContent.BeginText();
                        cbImportantNotesTableOfContent.SetColorFill(BaseColor.GRAY);                
                        cbImportantNotesTableOfContent.SetFontAndSize(baseFontCalibri, 18f);
                        cbImportantNotesTableOfContent.SetTextMatrix(50, 360);
                        cbImportantNotesTableOfContent.ShowText("Important Notes");
                        cbImportantNotesTableOfContent.EndText();

                        PdfContentByte cbImportantNotesTableOfContentPageNumber = writer.DirectContent;
                        cbImportantNotesTableOfContentPageNumber.BeginText();
                        cbImportantNotesTableOfContentPageNumber.SetColorFill(BaseColor.GRAY);                 
                        cbImportantNotesTableOfContentPageNumber.SetFontAndSize(baseFontCalibri, 18f);
                        cbImportantNotesTableOfContentPageNumber.SetTextMatrix(650, 360);
                        cbImportantNotesTableOfContentPageNumber.ShowText("3");
                        cbImportantNotesTableOfContentPageNumber.EndText();

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // add categories to table of contents /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        float lineHeightDifference = 30f;
                        float newLinePosition = 360f;

                        int initialPage = 4;
                        int pageNumber = 0;

                        // get distinct categories
                        var categories = (from obj in ObjectsAll
                                          select new
                                          {
                                              Category = obj.Category,
                                              CategoryOrderId = obj.CategoryOrderId,
                                              ObjectCount = Objects.Count(i => i.Category == obj.Category),
                                          }).Distinct();

                        // order the categories
                        categories = categories.OrderBy(obj => obj.CategoryOrderId);

                        //// group by
                        //var categories = (from obj in Objects
                        //                  group obj by obj.Category into distinctCategories
                        //                  select new
                        //                  {
                        //                      Category = distinctCategories.Key,
                        //                      CategoryCount = distinctCategories.Count()
                        //                  }).Distinct().OrderBy(obj => obj.Category);

                        foreach (var category in categories)
                        {
                            newLinePosition = newLinePosition - lineHeightDifference;

                            if (pageNumber == 0)
                            {
                                pageNumber = initialPage;
                            }
                            else
                            {
                                pageNumber = pageNumber + 1;
                            }

                            PdfContentByte cbCategory = writer.DirectContent;
                            cbCategory.BeginText();
                            cbCategory.SetColorFill(BaseColor.GRAY);                    
                            cbCategory.SetFontAndSize(baseFontCalibri, 18f);
                            cbCategory.SetTextMatrix(50, newLinePosition);
                            cbCategory.ShowText(category.Category);
                            cbCategory.EndText();

                            PdfContentByte cbCategoryNumber = writer.DirectContent;
                            cbCategoryNumber.BeginText();
                            cbCategoryNumber.SetColorFill(BaseColor.GRAY);                    
                            cbCategoryNumber.SetFontAndSize(baseFontCalibri, 18f);
                            cbCategoryNumber.SetTextMatrix(650, newLinePosition);
                            cbCategoryNumber.ShowText(pageNumber.ToString());
                            cbCategoryNumber.EndText();

                            // set page number
                            pageNumber = pageNumber + category.ObjectCount;
                        }

                        // footer content
                        PdfContentByte cbConfidentialTableOfContents = writer.DirectContent;
                        cbConfidentialTableOfContents.BeginText();
                        cbConfidentialTableOfContents.SetFontAndSize(baseFontCalibri, 12f);
                        cbConfidentialTableOfContents.SetColorFill(BaseColor.BLACK);  
                        cbConfidentialTableOfContents.SetTextMatrix(50, 20);
                        cbConfidentialTableOfContents.ShowText("Confidential");
                        cbConfidentialTableOfContents.EndText();

                        PdfContentByte cbPageNumberTableOfContents = writer.DirectContent;
                        cbPageNumberTableOfContents.BeginText();
                        cbPageNumberTableOfContents.SetFontAndSize(baseFontCalibri, 10f);
                        cbPageNumberTableOfContents.SetTextMatrix(400, 20);
                        cbPageNumberTableOfContents.ShowText("2");
                        cbPageNumberTableOfContents.EndText();

                        iTextSharp.text.Image imgFooterTableOfContents = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                        //imgFooterTableOfContents.ScaleAbsolute(69f, 10f); 
                        //imgFooterTableOfContents.ScaleAbsolute(PixelsToPoints(134.25f, 110), PixelsToPoints(19.5f, 110)); // 179/26
                        imgFooterTableOfContents.ScalePercent(45f);
                        imgFooterTableOfContents.SetAbsolutePosition(680, 20);
                        imgFooterTableOfContents.SpacingAfter = 10f;
                        doc.Add(imgFooterTableOfContents);

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - important notes page /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        doc.SetMargins(0f, 0f, 0f, 0f);
                        doc.NewPage();

                        // banner image
                        iTextSharp.text.Image imgImportantNotesBanner = iTextSharp.text.Image.GetInstance(imagePath + @"\HeaderContentsBanner.jpg");
                        imgImportantNotesBanner.ScaleToFit(pageSize.Rotate().Width, pageSize.Rotate().Height);
                        doc.Add(imgImportantNotesBanner);

                        // line table of contents
                        PdfContentByte cbImportantNotes = writer.DirectContent;
                        cbImportantNotes.BeginText();
                        cbImportantNotes.SetColorFill(BaseColor.BLACK);                    
                        cbImportantNotes.SetFontAndSize(baseFontCalibri, 32f);
                        cbImportantNotes.SetTextMatrix(50, 460);
                        cbImportantNotes.ShowText("Important Notes");
                        cbImportantNotes.EndText();

                        // note 1
                        string note1 = "1). In Assette software – Assette Reports, Assette Presentations and Assette EasyReports – the objects and templates are \nin PowerPoint format. You may select objects depicted in this library and place in templates. Objects can be placed to fit \nyour exact requirements.";
                        iTextSharp.text.Paragraph para1 = new iTextSharp.text.Paragraph(note1, fontCalibri12);
                        para1.SpacingBefore = 20f;
                        para1.IndentationLeft = 50f;
                        doc.Add(para1);

                        // note 2
                        string note2 = "2). Objects are placeholders for actual data. In Assette software, you first create templates by using objects from this \nlibrary. Next, you generate reports using templates – you select a template and then specify the accounts you want to \ngenerate reports for. At generation time, actual account data is merged into the objects.";
                        iTextSharp.text.Paragraph para2 = new iTextSharp.text.Paragraph(note2, fontCalibri12);
                        para2.SpacingBefore = 10f;
                        para2.IndentationLeft = 50f;
                        doc.Add(para2);

                        // note 3
                        string note3 = "3) Certain objects in this library are available only in Assette Reports and Assette Presentations – not in EasyReports. \nObjects that are not included in Assette EasyReports are clearly marked as such on the top-right corner of the page.";
                        iTextSharp.text.Paragraph para3 = new iTextSharp.text.Paragraph(note3, fontCalibri12);
                        para3.SpacingBefore = 10f;
                        para3.IndentationLeft = 50f;
                        doc.Add(para3);

                        // note 4
                        string note4 = "4). Composite return data is available only in Assette Reports and Assette Presentations.";
                        iTextSharp.text.Paragraph para4 = new iTextSharp.text.Paragraph(note4, fontCalibri12);
                        para4.SpacingBefore = 10f;
                        para4.IndentationLeft = 50f;
                        doc.Add(para4);

                        // note 5
                        string note5 = "5). The objects depicted in the “Advanced Analytics” section are optional. The Advanced Analytics Pack is not \navailable in Assette EasyReports.";
                        iTextSharp.text.Paragraph para5 = new iTextSharp.text.Paragraph(note5, fontCalibri12);
                        para5.SpacingBefore = 10f;
                        para5.IndentationLeft = 50f;
                        doc.Add(para5);

                        // note 6
                        string note6 = "CONFIDENTIALITY NOTICE";
                        iTextSharp.text.Paragraph para6 = new iTextSharp.text.Paragraph(note6, fontCalibri12);
                        para6.SpacingBefore = 20f;
                        para6.IndentationLeft = 320f;
                        doc.Add(para6);

                        // note 7
                        string note7 = "The information contained in this document is proprietary to Assette. This document is intended solely for the \ndesignated recipient(s) and other employees of the recipient's firm. The contents of this document must not be \nshared with any other party without the written consent of Assette.";
                        iTextSharp.text.Paragraph para7 = new iTextSharp.text.Paragraph(note7, fontCalibri12);
                        para7.SpacingBefore = 15f;
                        para7.IndentationLeft = 110f;
                        doc.Add(para7);

                        // footer content - important notes
                        PdfContentByte cbConfidentialImportantNotes = writer.DirectContent;
                        cbConfidentialImportantNotes.BeginText();
                        cbConfidentialImportantNotes.SetFontAndSize(baseFontCalibri, 12f);
                        cbConfidentialImportantNotes.SetTextMatrix(50, 20);
                        cbConfidentialImportantNotes.ShowText("Confidential");
                        cbConfidentialImportantNotes.EndText();

                        PdfContentByte cbPageNumberImportantNotes = writer.DirectContent;
                        cbPageNumberImportantNotes.BeginText();
                        cbPageNumberImportantNotes.SetFontAndSize(baseFontCalibri, 10f);
                        cbPageNumberImportantNotes.SetTextMatrix(400, 20);
                        cbPageNumberImportantNotes.ShowText("3");
                        cbPageNumberImportantNotes.EndText();

                        iTextSharp.text.Image imgFooterImportantNotes = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                        //imgFooterImportantNotes.ScaleAbsolute(69f, 10f); // 6.88
                        //imgFooterImportantNotes.ScaleAbsolute(PixelsToPoints(134.25f, 110), PixelsToPoints(19.5f, 110)); // 179/26
                        imgFooterImportantNotes.ScalePercent(45f);
                        imgFooterImportantNotes.SetAbsolutePosition(680, 20);
                        imgFooterImportantNotes.SpacingAfter = 10f;
                        doc.Add(imgFooterImportantNotes);

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - asset allocation page ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        // set category
                        string CategoryName = string.Empty;
                        bool writeCategoryName = false;

                        // page number
                        int loop = 3;

                        foreach (var obj in Objects)
                        {
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            // new page - setting category name ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            if (CategoryName == string.Empty)
                            {
                                // setting category
                                CategoryName = obj.Category;

                                // increment
                                loop++;

                                writeCategoryName = true;
                            }
                            else if (CategoryName == obj.Category)
                            {
                                // don't write
                                writeCategoryName = false;
                            }
                            else
                            {
                                // setting new category
                                CategoryName = obj.Category;

                                // increment
                                loop++;

                                writeCategoryName = true;
                            }

                            if (writeCategoryName == true)
                            {
                                doc.SetMargins(0f, 0f, 0f, 0f);
                                doc.NewPage();

                                // banner image
                                iTextSharp.text.Image imgAsseteAllocation = iTextSharp.text.Image.GetInstance(imagePath + @"\HeaderContentsBanner.jpg");
                                imgAsseteAllocation.ScaleToFit(pageSize.Rotate().Width, pageSize.Rotate().Height);
                                imgAsseteAllocation.SetAbsolutePosition(0, 300);
                                doc.Add(imgAsseteAllocation);

                                // category name
                                PdfContentByte cbCategoryName = writer.DirectContent;
                                cbCategoryName.BeginText();
                                cbCategoryName.SetColorFill(BaseColor.BLACK);                   
                                cbCategoryName.SetFontAndSize(baseFontCalibri, 40f);
                                cbCategoryName.SetTextMatrix(40, 320);
                                cbCategoryName.ShowText(CategoryName);
                                cbCategoryName.EndText();

                                // category - easy reports
                                PdfContentByte cbCategoryEasy = writer.DirectContent;

                                if (CategoryName.ToLower() == "performance")
                                {                                  
                                    cbCategoryName.BeginText();
                                    cbCategoryName.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                    cbCategoryName.SetFontAndSize(baseFontCalibri, 20f);
                                    cbCategoryName.SetTextMatrix(40, 200);
                                    cbCategoryName.ShowText("Reporting on composite performance is not available in");
                                    cbCategoryName.EndText();

                                    cbCategoryName.BeginText();
                                    cbCategoryName.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                    cbCategoryName.SetFontAndSize(baseFontCalibri, 20f);
                                    cbCategoryName.SetTextMatrix(40, 175);
                                    cbCategoryName.ShowText("Assette EasyReports.");
                                    cbCategoryName.EndText();
                                }

                                if (CategoryName.ToLower() == "characteristics" || CategoryName.ToLower() == "attribution")
                                {
                                    cbCategoryName.BeginText();
                                    cbCategoryName.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                    cbCategoryName.SetFontAndSize(baseFontCalibri, 20f);
                                    cbCategoryName.SetTextMatrix(40, 200);
                                    cbCategoryName.ShowText("Not available in Easy Editions.");
                                    cbCategoryName.EndText();
                                }

                                if (CategoryName.ToLower() == "advanced analytics")
                                {
                                    cbCategoryName.BeginText();
                                    cbCategoryName.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                    cbCategoryName.SetFontAndSize(baseFontCalibri, 20f);
                                    cbCategoryName.SetTextMatrix(40, 200);
                                    cbCategoryName.ShowText("Optional in Standard Version. Not available in Assette");
                                    cbCategoryName.EndText();

                                    cbCategoryName.BeginText();
                                    cbCategoryName.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                    cbCategoryName.SetFontAndSize(baseFontCalibri, 20f);
                                    cbCategoryName.SetTextMatrix(40, 175);
                                    cbCategoryName.ShowText("EasyReports.");
                                    cbCategoryName.EndText();
                                }

                                // footer content - category name
                                PdfContentByte cbConfidentialCategoryName = writer.DirectContent;
                                cbConfidentialCategoryName.BeginText();
                                cbCategoryName.SetColorFill(BaseColor.BLACK);
                                cbConfidentialCategoryName.SetFontAndSize(baseFontCalibri, 12f);
                                cbConfidentialCategoryName.SetTextMatrix(50, 20);
                                cbConfidentialCategoryName.ShowText("Confidential");
                                cbConfidentialCategoryName.EndText();

                                PdfContentByte cbPageNumberCategoryName = writer.DirectContent;
                                cbPageNumberCategoryName.BeginText();
                                cbPageNumberCategoryName.SetFontAndSize(baseFontCalibri, 10f);
                                cbPageNumberCategoryName.SetTextMatrix(400, 20);
                                cbPageNumberCategoryName.ShowText(loop.ToString());
                                cbPageNumberCategoryName.EndText();

                                iTextSharp.text.Image imgFooterCategoryName = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                                //imgFooterCategoryName.ScaleAbsolute(69f, 10f); // 6.88
                                //imgFooterCategoryName.ScaleAbsolute(PixelsToPoints(134.25f, 110), PixelsToPoints(19.5f, 110)); // 179/26
                                imgFooterCategoryName.ScalePercent(45f);
                                imgFooterCategoryName.SetAbsolutePosition(680, 20);
                                imgFooterCategoryName.SpacingAfter = 10f;
                                doc.Add(imgFooterCategoryName);
                            }

                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            // new page - adding object to page ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            doc.SetMargins(50f, 50f, 30f, 0f);
                            doc.NewPage();

                            // increment
                            loop++;

                            // line 01                            
                            string firstLine = obj.Category + " - " + obj.Name + " (" + obj.Type + ")";
                            doc.Add(new iTextSharp.text.Paragraph(firstLine, fontCalibri18));

                            doc.Add(new iTextSharp.text.Paragraph("", fontCalibri12));

                            // new line
                            doc.Add(new iTextSharp.text.Paragraph(Environment.NewLine));

                            // line 02                          
                            string secondLine = "Description: " + obj.Description;
                            doc.Add(new iTextSharp.text.Paragraph(secondLine, fontCalibriBody));

                            // new line
                            doc.Add(new iTextSharp.text.Paragraph(Environment.NewLine));

                            // easy reports
                            PdfContentByte cbEasy = writer.DirectContent;

                            if (obj.ClientType.ToLower() != "easy" && (CategoryName.ToLower() == "performance" || CategoryName.ToLower() == "characteristics" || CategoryName.ToLower() == "attribution"))
                            {                              
                                cbEasy.BeginText();
                                cbEasy.SetFontAndSize(baseFontCalibri, 12f);
                                cbEasy.SetTextMatrix(600, 590);
                                cbEasy.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                cbEasy.ShowText("Not available in Easy Editions");                          
                                cbEasy.EndText();
                            }

                            // easy reports
                            if (obj.ClientType.ToLower() != "easy" && (CategoryName.ToLower() == "advanced Analytics"))
                            {
                                cbEasy.BeginText();
                                cbEasy.SetFontAndSize(baseFontCalibri, 12f);
                                cbEasy.SetTextMatrix(637, 590);
                                cbEasy.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                cbEasy.ShowText("Optional in Standard Version");                          
                                cbEasy.EndText();

                                cbEasy.BeginText();
                                cbEasy.SetFontAndSize(baseFontCalibri, 12f);
                                cbEasy.SetTextMatrix(600, 578);
                                cbEasy.SetColorFill(new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0397d6"))); // blue
                                cbEasy.ShowText("Not available in Easy Editions");
                                cbEasy.EndText();
                            }

                            // footer content
                            PdfContentByte cbConfidential = writer.DirectContent;
                            cbConfidential.BeginText();
                            cbConfidential.SetFontAndSize(baseFontCalibri, 12f);
                            cbEasy.SetColorFill(new BaseColor(System.Drawing.Color.Black));
                            cbConfidential.SetTextMatrix(50, 20);
                            cbConfidential.ShowText("Confidential");
                            cbConfidential.EndText();

                            PdfContentByte cbPageNumber = writer.DirectContent;
                            cbPageNumber.BeginText();
                            cbPageNumber.SetFontAndSize(baseFontCalibri, 10f);
                            cbPageNumber.SetTextMatrix(400, 20);
                            cbPageNumber.ShowText(loop.ToString());
                            cbPageNumber.EndText();

                            // footer image
                            iTextSharp.text.Image imgFooter = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                            //imgFooter.ScaleAbsolute(69f, 10f); 
                            //imgFooter.ScaleAbsolute(PixelsToPoints(134.25f, 110), PixelsToPoints(19.5f, 110)); // 179/26
                            imgFooter.ScalePercent(45f);
                            imgFooter.SetAbsolutePosition(680, 20);
                            imgFooter.SpacingAfter = 10f;
                            doc.Add(imgFooter);

                            float imgWidth = 0f;
                            float imgHeight = 0f;
                            iTextSharp.text.Image imgObject;
                         
                            /*
                            if (obj.ObjectTableType.ToLower() == "p" && obj.Type.ToLower() == "chart")
                            {                         
                                imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                                imgWidth = PixelsToPoints(imgObject.Width, imgObject.DpiX, pdfDpi);
                                imgHeight = PixelsToPoints(imgObject.Height, imgObject.DpiY, pdfDpi);

                                imgObject.ScaleAbsolute(imgWidth, imgHeight);

                                imgObject.BorderWidthBottom = 1f;
                                imgObject.BorderColorBottom = BaseColor.LIGHT_GRAY;

                                doc.Add(imgObject);
                            }
                            else
                            {
                                if (obj.ObjectTableType.ToLower() == "m" && obj.Type.ToLower() == "chart")
                                {
                                    imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                                    //imgObject = iTextSharp.text.Image.GetInstance(@"G:\Images\a.png");

                                    imgWidth = PixelsToPoints(imgObject.Width, imgObject.DpiX, pdfDpi);
                                    imgHeight = PixelsToPoints(imgObject.Height, imgObject.DpiY, pdfDpi);

                                    //imgWidth = imgObject.Width;
                                    //imgHeight = imgObject.Height;

                                    imgObject.ScaleAbsolute(imgWidth, imgHeight);

                                    doc.Add(imgObject);
                                }
                                else
                                {
                                    imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                                    imgWidth = PixelsToPoints(imgObject.Width, imgObject.DpiX, pdfDpi);
                                    imgHeight = PixelsToPoints(imgObject.Height, imgObject.DpiY, pdfDpi);

                                    if (imgWidth > 500 || imgHeight > 400)
                                    {
                                        imgObject.ScalePercent(63f);
                                        Log.Info("ScalePercent - " + imgObject.Width.ToString() + ":" + imgObject.Height.ToString());
                                    }
                                    else
                                    {
                                        imgObject.ScaleAbsolute(imgWidth, imgHeight);
                                        Log.Info("ScaleAbsolute - " + imgWidth.ToString() + ":" + imgHeight.ToString());
                                    }

                                    doc.Add(imgObject);    
                                }
                            }
                            */

                            /*
                            imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                            if (imgObject.Width > 500 || imgObject.Height > 400)
                            {
                                imgObject.ScalePercent(70f);
                                //imgObject.BorderWidth = 1f;
                                //imgObject.BorderColorBottom = BaseColor.LIGHT_GRAY;
                                Log.Info("ScalePercent - " + imgObject.Width.ToString() + ":" + imgObject.Height.ToString());
                            }
                            else
                            {
                                imgObject.ScaleAbsolute(imgObject.Width, imgObject.Height);
                                //imgObject.BorderWidth = 1f;
                                //imgObject.BorderColorBottom = BaseColor.LIGHT_GRAY;
                                Log.Info("ScaleAbsolute - " + imgWidth.ToString() + ":" + imgHeight.ToString());
                            }
                            */

                            // default pdf dpi = 110 (110 dots per inch)
                            // so that means, there are 110 pixels per inch
                            // 1 in = 2.54 cm = 72 points
                            int pdfDpi = 110; //

                            // get image
                            imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                            imgWidth = PixelsToPoints(imgObject.Width, pdfDpi);
                            imgHeight = PixelsToPoints(imgObject.Height, pdfDpi);

                            imgObject.ScaleToFit(imgWidth, imgHeight);
                            doc.Add(imgObject); 

                            // add image to doc
                            //doc.Add(imgObject);                 
                        }

                        doc.Close();                
                    }

                    rtnValue = memoryStream.ToArray();            
                }
            }
            catch (Exception ex)
            {
                rtnValue = new byte[0];  // error

                Log.Error("GetAllPAObjectDetailsForPdf_itextsharp()", ex);
            }

            return rtnValue;
        }


        public static byte[] GetAllPAObjectDetailsForPdf_pdfsharp()
        {
            byte[] rtnValue = new byte[0];

            message = string.Format("GetAllPAObjectDetailsForPdf_pdfsharp() No parameters passed.");
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                IQueryable<PAObject> pAObjects = entity.PAObjects1.Where(pao => (pao.IsDeleted == false) && (pao.ObjectTypeID == 1 || pao.ObjectTypeID == 2)).OrderBy(pao => pao.ID);
                IQueryable<PAObjectMockup> pAObjectMockups = entity.PAObjectMockups1.OrderBy(pam => pam.ID);
                IQueryable<PAObjectCategoryOrder> pAObjectCategoryOrders = entity.PAObjectCategoryOrders.Where(pa => pa.IsVisible == true);

                int RecordCount = 0;

                // get pa-objects ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var PAObjects =
                     (from pAObject in pAObjects
                      join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObject.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                      select new
                      {
                          ID = pAObject.ID,
                          Name = pAObject.Name,
                          Description = pAObject.Description,
                          Type = pAObject.paObjectType.Type,
                          TypeId = pAObject.ObjectTypeID,
                          Category = pAObject.paObjectCategory.Category,
                          CategoryId = pAObject.ObjectCategoryID,
                          ObjectImage = pAObject.ObjectImage,
                          ClientType = pAObject.ClientType,
                          ObjectTableType = "p",
                          CategoryOrderId = pAObjectCategoryOrder.SortOrder,
                          Width = pAObject.Width,
                          Height = pAObject.Height
                      });

                RecordCount = PAObjects.Count();

                // get mockup objects that doesn't exists on pa-object table
                var PAObjectMokupsDoesntExists =
                    (from pAObjectMockup in pAObjectMockups
                     join pAObjectCategoryOrder in pAObjectCategoryOrders on pAObjectMockup.ObjectCategoryID equals pAObjectCategoryOrder.PaObjectCategoryId
                     where
                    (!pAObjects.Any(pao => pao.Name.Replace(" ", "").Trim().ToLower() == pAObjectMockup.Name.Replace(" ", "").Trim().ToLower() && pAObjectMockup.ObjectCategoryID == pao.ObjectCategoryID && pAObjectMockup.ObjectTypeID == pao.ObjectTypeID))
                     select new
                     {
                         ID = pAObjectMockup.ID,
                         Name = pAObjectMockup.Name,
                         Description = pAObjectMockup.Description,
                         Type = pAObjectMockup.paObjectType.Type,
                         TypeId = pAObjectMockup.ObjectTypeID,
                         Category = pAObjectMockup.paObjectCategory.Category,
                         CategoryId = pAObjectMockup.ObjectCategoryID,
                         ObjectImage = pAObjectMockup.ObjectImage,
                         ClientType = pAObjectMockup.ClientType,
                         ObjectTableType = "m",
                         CategoryOrderId = pAObjectCategoryOrder.SortOrder,
                         Width = (int?)null,
                         Height = (int?)null
                     });

                RecordCount = PAObjectMokupsDoesntExists.Count();

                // join 2 results
                var ObjectsAll = PAObjects.Concat(PAObjectMokupsDoesntExists).Take(1000);
                //var ObjectsAll = PAObjects.Concat(PAObjectMokupsDoesntExists).Where(obj => obj.Name == "Portfolio Holdings Detail" || obj.Name == "One Asset Class by Country");
                //var ObjectsAll = PAObjects.Concat(PAObjectMokupsDoesntExists).Where(obj => obj.Name == "Purchases and Sales");

                RecordCount = ObjectsAll.Count();

                // sorting: 
                // 1 - category order - PAObjectCategoryOrder
                // 2 - type - charts first then table
                // 3 - name alpha asc              
                var Objects = ObjectsAll.OrderBy(obj => obj.CategoryOrderId).ThenByDescending(obj => obj.TypeId).ThenBy(obj => obj.Name);

                RecordCount = Objects.Count();

                string parameterName = "GalleryDocsRootFolder";

                // get configuration
                var configuration = entity.ClientConfigurations.Where(m => m.ParamName.ToUpper() == parameterName.ToUpper()).FirstOrDefault();

                // get root path
                string rootFolderPath = configuration.ParamValue;

                // set image path
                string imagePath = rootFolderPath + @"\Images\";

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // PDFSharp - http://www.pdfsharp.net/wiki/PDFsharpSamples.ashx ////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                // Create a new PDF document
                PdfSharp.Pdf.PdfDocument pdfDocument = new PdfSharp.Pdf.PdfDocument();

                // Document properties
                pdfDocument.Info.Title = "ASSETTE Client Reporting Object Library";
                pdfDocument.Info.CreationDate = DateTime.Now;
                pdfDocument.Info.Author = "ASSETTE";

                // Create fonts
                XFont fontCalibriBody10Regular = new XFont("Calibri (Body)", 10, XFontStyle.Regular);
                XFont fontCalibriBody10Bold = new XFont("Calibri (Body)", 10, XFontStyle.Bold);
                XFont fontCalibriBody12Regular = new XFont("Calibri (Body)", 12, XFontStyle.Regular);
                XFont fontCalibriBody14Regular = new XFont("Calibri (Body)", 14, XFontStyle.Regular);
                XFont fontCalibriBody16Regular = new XFont("Calibri (Body)", 16, XFontStyle.Regular);
                XFont fontCalibriBody18Regular = new XFont("Calibri (Body)", 18, XFontStyle.Regular);
                XFont fontCalibriBody28Bold = new XFont("Calibri (Body)", 28, XFontStyle.Bold);
                XFont fontCalibriBody32Regular = new XFont("Calibri (Body)", 32, XFontStyle.Regular);

                XFont fontCalibri115Regular = new XFont("Calibri", 11.5, XFontStyle.Regular);
                XFont fontCalibri14Bold = new XFont("Calibri", 14, XFontStyle.Bold);
                XFont fontCalibri16Regular = new XFont("Calibri", 16, XFontStyle.Regular);
                XFont fontCalibri20Regular = new XFont("Calibri", 20, XFontStyle.Regular);
                XFont fontCalibri28Bold = new XFont("Calibri", 28, XFontStyle.Bold);

                XGraphics gfx;
                XImage xImage;
                string text = string.Empty;

                //letter 792/612 points - 8.5|11 inches 
                //600/470

                using (var memoryStream = new MemoryStream())
                {
                    //XImage xImage = XImage.FromFile(@"G:\Images\original.png");
                    //text = "Hello, World!";
                    //WriteTextToPDF(gfx, text, fontCalibri14Bold, XBrushes.Black, 0, 0);
                    //DrawImageToPDF(gfx, xImage, 10, 10);
                    //DrawImageToPDFWithScalingResolution(gfx, xImage, 100, 10);
                    //DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 200, 10, 100, 100);
                    //DrawImageToPDFWithScalingValue(gfx, xImage, 300, 10, 0.7);


                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Page 1 - title page ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    AddNewPageToPDF(pdfDocument, out gfx);

                    xImage = XImage.FromFile(imagePath + @"\PresentationBanner.jpg"); // 720/243
                    DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 0, 0, 792, 267);

                    xImage = XImage.FromFile(imagePath + @"\AssetteLogoLarge.jpg"); // 154/25
                    DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 50, 310, 231, 37.5);

                    text = "Client Reporting Object Library";
                    WriteTextToPDF(gfx, text, fontCalibriBody32Regular, XBrushes.Black, 50, 410);

                    text = "Confidential";
                    WriteTextToPDF(gfx, text, fontCalibriBody16Regular, XBrushes.Black, 50, 570);

                    text = DateTime.Today.ToShortDateString();
                    WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 700, 577);


                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Page 2 - table of contents /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    AddNewPageToPDF(pdfDocument, out gfx);

                    xImage = XImage.FromFile(imagePath + @"\HeaderContentsBanner.jpg"); // 720/154
                    DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 0, 0, 792, 169);

                    text = "Table of Contents";
                    WriteTextToPDF(gfx, text, fontCalibri28Bold, XBrushes.Black, 50, 120);

                    text = "SECTION";
                    WriteTextToPDF(gfx, text, fontCalibri20Regular, XBrushes.Black, 50, 180);

                    text = "PAGE";
                    WriteTextToPDF(gfx, text, fontCalibri20Regular, XBrushes.Black, 700, 180);

                    text = "Important Notes";
                    WriteTextToPDF(gfx, text, fontCalibri16Regular, XBrushes.Black, 50, 220);

                    text = "3";
                    WriteTextToPDF(gfx, text, fontCalibri16Regular, XBrushes.Black, 700, 220);

                    float lineHeightDifference = 30f;
                    float newLinePosition = 220f;

                    int initialPage = 4;
                    int pageNumber = 0;

                    // get distinct categories
                    var categories = (from obj in ObjectsAll
                                      select new
                                      {
                                          Category = obj.Category,
                                          CategoryOrderId = obj.CategoryOrderId,
                                          ObjectCount = Objects.Count(i => i.Category == obj.Category),
                                      }).Distinct();

                    // order the categories
                    categories = categories.OrderBy(obj => obj.CategoryOrderId);

                    foreach (var category in categories)
                    {
                        newLinePosition = newLinePosition + lineHeightDifference;

                        if (pageNumber == 0)
                        {
                            pageNumber = initialPage;
                        }
                        else
                        {
                            pageNumber = pageNumber + 1;
                        }

                        text = category.Category;
                        WriteTextToPDF(gfx, text, fontCalibri16Regular, XBrushes.Black, 50, newLinePosition);

                        text = pageNumber.ToString();
                        WriteTextToPDF(gfx, text, fontCalibri16Regular, XBrushes.Black, 700, newLinePosition);

                        // set page number
                        pageNumber = pageNumber + category.ObjectCount;
                    }

                    text = "Confidential";
                    WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 50, 580);

                    text = "2";
                    WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 396, 578);

                    xImage = XImage.FromFile(imagePath + @"\AssetteLogoSmall.jpg"); // 169/32
                    DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 680, 575, 84.5, 16);


                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Page 3 - important notes ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    AddNewPageToPDF(pdfDocument, out gfx);

                    text = "Confidential";
                    WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 50, 580);

                    text = "3";
                    WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 396, 578);

                    xImage = XImage.FromFile(imagePath + @"\AssetteLogoSmall.jpg"); // 169/32
                    DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 680, 575, 84.5, 16);


                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Page - objects /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    string CategoryName = string.Empty;
                    bool writeCategoryName = false;

                    // page number
                    int loop = 3;

                    foreach (var obj in Objects)
                    {
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // Page - category name ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        if (CategoryName == string.Empty)
                        {
                            // setting category
                            CategoryName = obj.Category;

                            // increment
                            loop++;

                            writeCategoryName = true;
                        }
                        else if (CategoryName == obj.Category)
                        {
                            // don't write
                            writeCategoryName = false;
                        }
                        else
                        {
                            // setting new category
                            CategoryName = obj.Category;

                            // increment
                            loop++;

                            writeCategoryName = true;
                        }

                        if (writeCategoryName == true)
                        {
                            AddNewPageToPDF(pdfDocument, out gfx);

                            xImage = XImage.FromFile(imagePath + @"\HeaderContentsBanner.jpg"); // 720/154
                            DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 0, 0, 792, 169);

                            text = CategoryName;
                            WriteTextToPDF(gfx, text, fontCalibri28Bold, XBrushes.Black, 50, 120);

                            if (CategoryName.ToLower() == "performance")
                            {
                                text = "Reporting on composite performance is not available in";
                                WriteTextToPDF(gfx, text, fontCalibriBody18Regular, XBrushes.Black, 50, 300);

                                text = "Assette EasyReports.";
                                WriteTextToPDF(gfx, text, fontCalibriBody18Regular, XBrushes.Black, 50, 325);
                            }

                            if (CategoryName.ToLower() == "characteristics" || CategoryName.ToLower() == "attribution")
                            {
                                text = "Not available in Easy Editions.";
                                WriteTextToPDF(gfx, text, fontCalibriBody18Regular, XBrushes.Black, 50, 300);
                            }

                            if (CategoryName.ToLower() == "advanced analytics")
                            {
                                text = "Optional in Standard Version. Not available in Assette";
                                WriteTextToPDF(gfx, text, fontCalibriBody18Regular, XBrushes.Black, 50, 300);

                                text = "EasyReports.";
                                WriteTextToPDF(gfx, text, fontCalibriBody18Regular, XBrushes.Black, 50, 325);
                            }

                            text = "Confidential";
                            WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 50, 580);

                            text = loop.ToString();
                            WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 396, 578);

                            xImage = XImage.FromFile(imagePath + @"\AssetteLogoSmall.jpg"); // 169/32
                            DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 680, 575, 84.5, 16);
                        }

                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // Page - pa object ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        AddNewPageToPDF(pdfDocument, out gfx);

                        // migra-doc
                        MigraDoc.DocumentObjectModel.Document document = new MigraDoc.DocumentObjectModel.Document();

                        // add a section to the document
                        MigraDoc.DocumentObjectModel.Section section = document.AddSection();

                        // add new paragraph
                        MigraDoc.DocumentObjectModel.Paragraph paragraph = section.AddParagraph();
                        paragraph = document.LastSection.AddParagraph();
                        paragraph.Format.Borders.Width = 1;

                        // migra-doc render
                        MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(document);
                        docRenderer.PrepareDocument();

                        // increment
                        loop++;

                        int xStartPosition = 50;
                        int paragraphHeight = 0;
                        int xPosition = 0;

                        // line 01   
                        xPosition = xStartPosition;
                        text = obj.Category + " - " + obj.Name + " (" + obj.Type + ")";
                        WriteTextParagraphToPDF(document, section, gfx, text, 50, xPosition, 670, 0, 0, "Calibri (Body)", 18f, false, false);
                        paragraphHeight = GetPdfParagraphHeight(text, 670, "Calibri (Body)", 18f);

                        // line 02  
                        xPosition = xPosition + paragraphHeight + 10;
                        text = "Description: " + obj.Description;
                        WriteTextParagraphToPDF(document, section, gfx, text, 50, xPosition, 670, 0, 0, "Calibri (Body)", 11.5f, true, false);
                        paragraphHeight = GetPdfParagraphHeight(text, 670, "Calibri (Body)", 11.5f);

                        //value = value.Replace("\r\n", "\n"); //crlf

                        // image
                        xPosition = xPosition + paragraphHeight + 25;
                        MemoryStream msImg = new MemoryStream(obj.ObjectImage);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(msImg);
                        xImage = PdfSharp.Drawing.XImage.FromGdiPlusImage(image);

                        text = "Dimensions: " + xImage.PixelWidth.ToString() + ":" + xImage.PixelHeight.ToString();
                        WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 50, 50);

                        //DrawImageToPDF(gfx, xImage, 50, xPosition);

                        DrawImageToPDFToScaleFit(gfx, xImage, 50, xPosition, 0.8);

                        //DrawImageToPDFWithScalingValue(gfx, xImage, 50, xPosition, 0.9);
                        //gfx.ScaleTransform(1);

                        if (obj.ClientType.ToLower() != "easy" && (CategoryName.ToLower() == "performance" || CategoryName.ToLower() == "characteristics" || CategoryName.ToLower() == "attribution"))
                        {
                            text = "Not available in Easy Editions";
                            WriteTextToPDF(gfx, text, fontCalibri115Regular, XBrushes.Black, 580, 30);
                        }

                        // easy reports
                        if (obj.ClientType.ToLower() != "easy" && (CategoryName.ToLower() == "advanced analytics"))
                        {
                            text = "Optional in Standard Version";
                            WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 580, 20);

                            text = "Not available in Easy Editions";
                            WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 580, 35);
                        }

                        text = "Confidential";
                        WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 50, 580);

                        text = loop.ToString();
                        WriteTextToPDF(gfx, text, fontCalibriBody10Regular, XBrushes.Black, 396, 578);

                        xImage = XImage.FromFile(imagePath + @"\AssetteLogoSmall.jpg"); // 169/32
                        DrawImageToPDFWithScalingWidthHeight(gfx, xImage, 680, 575, 84.5, 16);



                    }

                    pdfDocument.Save(memoryStream, true);




                    /*


                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // new page - important notes page /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        doc.SetMargins(0f, 0f, 0f, 0f);
                        doc.NewPage();

                        // banner image
                        iTextSharp.text.Image imgImportantNotesBanner = iTextSharp.text.Image.GetInstance(imagePath + @"\HeaderContentsBanner.jpg");
                        imgImportantNotesBanner.ScaleToFit(pageSize.Rotate().Width, pageSize.Rotate().Height);
                        doc.Add(imgImportantNotesBanner);

                        // line table of contents
                        PdfContentByte cbImportantNotes = writer.DirectContent;
                        cbImportantNotes.BeginText();
                        cbImportantNotes.SetColorFill(BaseColor.BLACK);
                        cbImportantNotes.SetFontAndSize(baseFontCalibri, 32f);
                        cbImportantNotes.SetTextMatrix(50, 460);
                        cbImportantNotes.ShowText("Important Notes");
                        cbImportantNotes.EndText();

                        // note 1
                        string note1 = "1). In Assette software – Assette Reports, Assette Presentations and Assette EasyReports – the objects and templates are \nin PowerPoint format. You may select objects depicted in this library and place in templates. Objects can be placed to fit \nyour exact requirements.";
                        iTextSharp.text.Paragraph para1 = new iTextSharp.text.Paragraph(note1, fontCalibri12);
                        para1.SpacingBefore = 20f;
                        para1.IndentationLeft = 50f;
                        doc.Add(para1);

                        // note 2
                        string note2 = "2). Objects are placeholders for actual data. In Assette software, you first create templates by using objects from this \nlibrary. Next, you generate reports using templates – you select a template and then specify the accounts you want to \ngenerate reports for. At generation time, actual account data is merged into the objects.";
                        iTextSharp.text.Paragraph para2 = new iTextSharp.text.Paragraph(note2, fontCalibri12);
                        para2.SpacingBefore = 10f;
                        para2.IndentationLeft = 50f;
                        doc.Add(para2);

                        // note 3
                        string note3 = "3) Certain objects in this library are available only in Assette Reports and Assette Presentations – not in EasyReports. \nObjects that are not included in Assette EasyReports are clearly marked as such on the top-right corner of the page.";
                        iTextSharp.text.Paragraph para3 = new iTextSharp.text.Paragraph(note3, fontCalibri12);
                        para3.SpacingBefore = 10f;
                        para3.IndentationLeft = 50f;
                        doc.Add(para3);

                        // note 4
                        string note4 = "4). Composite return data is available only in Assette Reports and Assette Presentations.";
                        iTextSharp.text.Paragraph para4 = new iTextSharp.text.Paragraph(note4, fontCalibri12);
                        para4.SpacingBefore = 10f;
                        para4.IndentationLeft = 50f;
                        doc.Add(para4);

                        // note 5
                        string note5 = "5). The objects depicted in the “Advanced Analytics” section are optional. The Advanced Analytics Pack is not \navailable in Assette EasyReports.";
                        iTextSharp.text.Paragraph para5 = new iTextSharp.text.Paragraph(note5, fontCalibri12);
                        para5.SpacingBefore = 10f;
                        para5.IndentationLeft = 50f;
                        doc.Add(para5);

                        // note 6
                        string note6 = "CONFIDENTIALITY NOTICE";
                        iTextSharp.text.Paragraph para6 = new iTextSharp.text.Paragraph(note6, fontCalibri12);
                        para6.SpacingBefore = 20f;
                        para6.IndentationLeft = 320f;
                        doc.Add(para6);

                        // note 7
                        string note7 = "The information contained in this document is proprietary to Assette. This document is intended solely for the \ndesignated recipient(s) and other employees of the recipient's firm. The contents of this document must not be \nshared with any other party without the written consent of Assette.";
                        iTextSharp.text.Paragraph para7 = new iTextSharp.text.Paragraph(note7, fontCalibri12);
                        para7.SpacingBefore = 15f;
                        para7.IndentationLeft = 110f;
                        doc.Add(para7);

                        // footer content - important notes
                        PdfContentByte cbConfidentialImportantNotes = writer.DirectContent;
                        cbConfidentialImportantNotes.BeginText();
                        cbConfidentialImportantNotes.SetFontAndSize(baseFontCalibri, 12f);
                        cbConfidentialImportantNotes.SetTextMatrix(50, 20);
                        cbConfidentialImportantNotes.ShowText("Confidential");
                        cbConfidentialImportantNotes.EndText();

                        PdfContentByte cbPageNumberImportantNotes = writer.DirectContent;
                        cbPageNumberImportantNotes.BeginText();
                        cbPageNumberImportantNotes.SetFontAndSize(baseFontCalibri, 10f);
                        cbPageNumberImportantNotes.SetTextMatrix(400, 20);
                        cbPageNumberImportantNotes.ShowText("3");
                        cbPageNumberImportantNotes.EndText();

                        iTextSharp.text.Image imgFooterImportantNotes = iTextSharp.text.Image.GetInstance(imagePath + @"\AssetteLogoSmall.jpg");
                        //imgFooterImportantNotes.ScaleAbsolute(69f, 10f); // 6.88
                        //imgFooterImportantNotes.ScaleAbsolute(PixelsToPoints(134.25f, 110), PixelsToPoints(19.5f, 110)); // 179/26
                        imgFooterImportantNotes.ScalePercent(45f);
                        imgFooterImportantNotes.SetAbsolutePosition(680, 20);
                        imgFooterImportantNotes.SpacingAfter = 10f;
                        doc.Add(imgFooterImportantNotes);

                       

                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            // new page - adding object to page ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            

                            // 1 in = 2.54 cm = 72 points
                            int pdfDpi = 72;

                            float imgWidth = 0f;
                            float imgHeight = 0f;
                            iTextSharp.text.Image imgObject;

                            if (obj.ObjectTableType.ToLower() == "p" && obj.Type.ToLower() == "chart")
                            {
                                imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                                imgWidth = PixelsToPoints(imgObject.Width, imgObject.DpiX, pdfDpi);
                                imgHeight = PixelsToPoints(imgObject.Height, imgObject.DpiY, pdfDpi);

                                imgObject.ScaleAbsolute(imgWidth, imgHeight);

                                imgObject.BorderWidthBottom = 1f;
                                imgObject.BorderColorBottom = BaseColor.LIGHT_GRAY;

                                //doc.Add(imgObject);
                            }
                            else
                            {
                                if (obj.ObjectTableType.ToLower() == "m" && obj.Type.ToLower() == "chart")
                                {
                                    imgObject = iTextSharp.text.Image.GetInstance(obj.ObjectImage.ToArray());

                                    //imgObject = iTextSharp.text.Image.GetInstance(@"G:\Images\a.png");

                                    imgWidth = PixelsToPoints(imgObject.Width, imgObject.DpiX, pdfDpi);
                                    imgHeight = PixelsToPoints(imgObject.Height, imgObject.DpiY, pdfDpi);

                                    //imgWidth = imgObject.Width;
                                    //imgHeight = imgObject.Height;

                                    imgObject.ScaleAbsolute(imgWidth, imgHeight);

                                    doc.Add(imgObject);
                                }
                            }

                            // add image to doc
                            //doc.Add(imgObject);                 
                        }

                        doc.Close();
                    }
                    */

                    rtnValue = memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                rtnValue = new byte[0];  // error

                Log.Error("GetAllPAObjectDetailsForPdf_pdfsharp()", ex);
            }

            return rtnValue;
        }


        public static void AddNewPageToPDF(PdfSharp.Pdf.PdfDocument pdfDocument, out XGraphics gfx)
        {
            // Create an empty page
            PdfSharp.Pdf.PdfPage page = pdfDocument.AddPage();

            // Page properties
            page.Size = PdfSharp.PageSize.Letter;
            page.Orientation = PageOrientation.Landscape;

            // Get an XGraphics object for drawing
            gfx = XGraphics.FromPdfPage(page);

            // Set page margins
            //page.TrimMargins.Top = 50;
            //page.TrimMargins.Right = 50;
            //page.TrimMargins.Bottom = 50;
            //page.TrimMargins.Left = 50;
        }

        public static int GetPdfParagraphHeight(string text, int paragraphWidth, string fontName, float fontSize)
        {
            int height = 0;

            int count = text.Count(f => f == '\n');

            text = text.Replace("\n", "");

            var pdfDoc = new PdfSharp.Pdf.PdfDocument();
            var pdfPage = pdfDoc.AddPage();
            var pdfGfx = PdfSharp.Drawing.XGraphics.FromPdfPage(pdfPage);
            var pdfFont = new PdfSharp.Drawing.XFont(fontName, fontSize);

            double fontWidth = pdfGfx.MeasureString(text, pdfFont).Width;
            double fontHeight = pdfGfx.MeasureString(text, pdfFont).Height;

            int noOfLines = (int)(fontWidth / paragraphWidth) + 1 + count;

            height = (noOfLines * (int)Math.Round(fontHeight));

            return height;
        }

        public static void WriteTextToPDF(XGraphics gfx, string text, XFont font, XBrush brush, double positionX, double positionY)
        {
            gfx.DrawString(text, font, brush, new XRect(positionX, positionY, 0, 0), XStringFormats.TopLeft);
        }

        public static void DrawImageToPDFToScaleFit(XGraphics gfx, XImage xImage, int positionX, int positionY, double scale)
        {
            //letter 792/612 points - 8.5|11 inches 
            if (xImage.PixelWidth > 500 || xImage.PixelHeight > 400)
            {
                gfx.DrawImage(xImage, positionX, positionY);  
                //gfx.DrawImage(xImage, positionX, positionY, xImage.PointWidth * scale, xImage.PointHeight * scale);
            }
            else
            {
                gfx.DrawImage(xImage, positionX, positionY);  
            }
        }

        public static void DrawImageToPDF(XGraphics gfx, XImage xImage, int positionX, int positionY)
        {
            gfx.ScaleTransform(1);

            gfx.DrawImage(xImage, positionX, positionY);         
        }

        public static void DrawImageToPDFWithScalingResolution(XGraphics gfx, XImage xImage, int positionX, int positionY)
        {
            gfx.ScaleTransform(1);

            double scaledWwidth = xImage.PixelWidth * 72 / xImage.HorizontalResolution;
            double scaledHeight = xImage.PixelHeight * 72 / xImage.HorizontalResolution;

            gfx.DrawImage(xImage, positionX, positionY, scaledWwidth, scaledHeight);
        }

        public static void DrawImageToPDFWithScalingWidthHeight(XGraphics gfx, XImage xImage, int positionX, int positionY, double scaledWwidth, double scaledHeight)
        {
            gfx.ScaleTransform(1);

            gfx.DrawImage(xImage, positionX, positionY, scaledWwidth, scaledHeight);
        }

        public static void DrawImageToPDFWithScalingValue(XGraphics gfx, XImage xImage, int positionX, int positionY, double scale)
        {
            gfx.ScaleTransform(scale);

            gfx.DrawImage(xImage, positionX, positionY);
        }

        public static void DrawImageToPDFSection(MigraDoc.DocumentObjectModel.Document document, XGraphics gfx, MigraDoc.DocumentObjectModel.Shapes.Image image)
        {
            gfx.ScaleTransform(1);

            // Add a section to the document
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();

            //section.Add(image);

            MigraDoc.DocumentObjectModel.Paragraph paragraph = section.AddParagraph("");


            // Create a renderer and prepare (=layout) the document
            MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(document);
            docRenderer.PrepareDocument();

            docRenderer.RenderObject(gfx, XUnit.FromPoint(0), XUnit.FromPoint(0), XUnit.FromPoint(10), paragraph);

            section.LastParagraph.AddImage(@"G:\Images\original.png");
        }

        public static void WriteTextParagraphToPDF(MigraDoc.DocumentObjectModel.Document document, MigraDoc.DocumentObjectModel.Section section, XGraphics gfx, string text, int positionX, int positionY, int paragraphWidth, int spaceBefore, int spaceAfter, string font, float fontSize, bool italic, bool bold)
        {
            MigraDoc.DocumentObjectModel.Paragraph paragraph = section.AddParagraph(text);
            
            // Paragraph properties
            paragraph.Format.Font.Name = font;
            paragraph.Format.Font.Size = fontSize;
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Font.Italic = italic;
            paragraph.Format.Font.Bold = bold;
            paragraph.Format.SpaceBefore = MigraDoc.DocumentObjectModel.Unit.FromPoint(spaceBefore);
            paragraph.Format.SpaceAfter = MigraDoc.DocumentObjectModel.Unit.FromPoint(spaceAfter);
            paragraph.Format.Borders.Width = 1;

            // Create a renderer and prepare (=layout) the document
            MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(document);
            docRenderer.PrepareDocument();

            docRenderer.RenderObject(gfx, XUnit.FromPoint(positionX), XUnit.FromPoint(positionY), XUnit.FromPoint(paragraphWidth), paragraph);
        }

        //public static void WriteIamgeParagraphToPDF(Document document, XGraphics gfx, string text, int positionX, int positionY, int paragraphWidth, int spaceBefore, int spaceAfter, string font, float fontSize, bool italic, bool bold)
        //{
        //    // Add a section to the document
        //    Section section = document.AddSection();

        //    MigraDoc.DocumentObjectModel.Shapes.Image img = section.AddImage(@"G:\Images\original.png");

        //    // Create a renderer and prepare (=layout) the document
        //    MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(document);
        //    docRenderer.PrepareDocument();

        //    docRenderer.RenderObject(gfx, XUnit.FromPoint(positionX), XUnit.FromPoint(positionY), XUnit.FromPoint(paragraphWidth), img);
        //}

        public static float PixelsToPoints(float imageDiemension, float imageDpi, float pdfDpi)
        {
            return (imageDiemension * pdfDpi) / imageDpi;
        }

        public static float PixelsToPoints(float value, int dpi)
        {
            return value / dpi * 72;
        }

        public static float ConvertPixelsToPoints(float pixels)
        {
            float points = 0f;

            // default pdf dpi = 110 (110 dots per inch)
            // so that means, there are 110 pixels per inch
            // 1 in = 2.54 cm = 72 points

            points = (72 / 110) * pixels;

            return points;
        }

        public static void WriteImageToLoaclPath(byte[] imageByteArray, string fileName)
        {
            MemoryStream ms = new MemoryStream(imageByteArray);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            string path = @"G:\images\" + fileName + ".bmp";
            returnImage.Save(path);
        }

        static System.Drawing.Image FixedSizee(System.Drawing.Image imgPhoto, int width, int height, bool hasBorder)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((width -
                              (sourceWidth * nPercent)) / 2);
                //destY = destY+ 50;
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((height -
                              (sourceHeight * nPercent)) / 2);
                //destX = destX + 50;
            }

            int destWidth = (int)(sourceWidth * nPercent) - 0;
            int destHeight = (int)(sourceHeight * nPercent) - 0;


            //MyTest
            destX = ((width - destWidth) / 2); //(int)(50 * nPercent);
            destY = ((height - destHeight) / 2);//(int)(50 * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height,
                              PixelFormat.Format32bppArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);

            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;



            if (hasBorder)
            {
                imgPhoto = AddBorder(imgPhoto, destWidth, destHeight);


                grPhoto.DrawImage(imgPhoto, new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
                                  new System.Drawing.Rectangle(sourceX, sourceY, imgPhoto.Width, imgPhoto.Height),
                                  GraphicsUnit.Pixel);
            }
            else
            {
                grPhoto.DrawImage(imgPhoto, new System.Drawing.Rectangle(destX, destY, destWidth, destHeight), new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            }


            grPhoto.Dispose();
            return bmPhoto;
        }

        private static System.Drawing.Image AddBorder(System.Drawing.Image image, int width, int height)
        {
            Bitmap bmPhoto = new Bitmap(width + 1, height + 1,
                             PixelFormat.Format32bppArgb);
            bmPhoto.SetResolution(image.HorizontalResolution,
                             image.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.FromArgb(217, 217, 217));

            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;


            grPhoto.DrawImage(image, new System.Drawing.Rectangle(0, 0, width, height), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        #endregion


        #region ADMIN

        public static bool ValidateUser(string userName, string password)
        {
            var rtnValue = false;

            message = string.Format("ValidateUser() - Admin Login: Parameters = username: {0}, password: {1}", userName, "******");
            Log.Info(message);

            try
            {
                var entity = GetEntity();
                rtnValue = entity.Users.Any(m => m.UserName.ToUpper() == userName.Trim().ToUpper() && m.Password == password);
            }
            catch (Exception ex)
            {
                rtnValue = false;

                Log.Error("ValidateUser()", ex);
            }

            return rtnValue;
        }

        public static string GetUserNameByEmail(string email)
        {
            var rtnValue = string.Empty;

            message = string.Format("GetUserNameByEmail() Parameters = email: {0}", email);
            Log.Info(message);

            try
            {
                var entity = GetEntity();
                var user = entity.Users.Where(m => m.Email == email).FirstOrDefault();

                if (user != null)
                {
                    return user.UserName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetUserNameByEmail()", ex);

                return null;
            }
        }

        public static string[] GetAllRoles()
        {
            message = string.Format("GetAllRoles()");
            Log.Info(message);

            try
            {
                var entity = GetEntity();
                return entity.Roles.Select(r => r.RoleName).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("GetAllRoles()", ex));

                return null;
            }
        }

        public static bool DeleteUser(string userName, bool deleteAllRelatedData)
        {
            message = string.Format("DeleteUser() Parameters = userName: {0}, deleteAllRelatedData: {1}", userName, deleteAllRelatedData.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var user = entity.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    user.GLRY_Roles.Clear();
                    entity.Users.DeleteObject(user);

                    entity.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("DeleteUser()", ex));

                return false;
            }
        }

        public static bool UpdateUser(int userId, string userName, string email, string applicationName)
        {
            message = string.Format("UpdateUser() Parameters = userId: {0}, userName: {1}, email: {2}, applicationName: {3}", userId, userName, email, applicationName);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var user = entity.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(email) && user.Email != email)
                    {
                        user.Email = email;
                    }

                    if (string.IsNullOrEmpty(applicationName))
                    {
                        user.ApplicationName = applicationName;
                    }

                    entity.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("UpdateUser()", ex));

                return false;
            }
        }

        public static void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            message = string.Format("AddUsersToRoles() Parameters = userNames: {0}, roleNames: {1}", String.Join(",", userNames), String.Join(",", roleNames));
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var roles = entity.Roles.Where(r => roleNames.Contains(r.RoleName)).ToList();
                var users = entity.Users.Where(u => userNames.Contains(u.UserName)).ToList();

                users.ForEach(x => roles.ForEach(r => x.GLRY_Roles.Add(r)));

                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("AddUsersToRoles", ex));
            }
        }

        public static void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            message = string.Format("RemoveUsersFromRoles() Parameters = userNames: {0}, roleNames: {1}", String.Join(",", userNames), String.Join(",", roleNames));
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var roles = entity.Roles.Where(r => roleNames.Contains(r.RoleName)).ToList();
                var users = entity.Users.Where(u => userNames.Contains(u.UserName)).ToList();

                users.ForEach(x => roles.ForEach(r => x.GLRY_Roles.Remove(r)));

                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("RemoveUsersFromRoles()", ex));
            }
        }

        public static string[] GetRolesForUser(string userName)
        {
            message = string.Format("GetRolesForUser() Parameters = userNames: {0}", userName);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var user = entity.Users.Where(u => u.UserName == userName).FirstOrDefault();

                return user.GLRY_Roles.Select(r => r.RoleName).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("GetRolesForUser()", ex));

                throw;
            }
        }

        public static bool CreateAdminUser(string userName, string password, string email, string applicationName)
        {
            message = string.Format("CreateAdminUser() Parameters = userName: {0}, password: {1}, email: {2}, applicationName: {3}", userName, password, email, applicationName);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var user = new Users();
                user.UserName = userName.Trim();
                user.Password = password.Trim();
                user.Email = email.Trim();
                user.ApplicationName = applicationName.Trim();

                entity.AddToUsers(user);

                if (entity.SaveChanges() > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("CreateAdminUser()", ex));

                return false;
            }
        }

        public static MembershipUser GetUser(string userName, bool userIsOnline)
        {
            message = string.Format("GetUser() Parameters = userName: {0}, userIsOnline: {1}", userName, userIsOnline.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                var user = entity.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    CustomMemberShipUser customUser = new CustomMemberShipUser(
                                        "AssetteMembershipProvider",
                                        user.Email,
                                        user.UserId,
                                        user.UserName,
                                        "",
                                        "",
                                        true,
                                        false,
                                        user.RegisteredDate,
                                        DateTime.MinValue,
                                        DateTime.MinValue,
                                        DateTime.MinValue,
                                        DateTime.MinValue);

                    return customUser;
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("GetUser()", ex));

                throw;
            }
        }

        private static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            message = string.Format("LINQToDataTable()");
            Log.Info(message);

            try
            {
                DataTable dtReturn = new DataTable();

                PropertyInfo[] oProps = null;

                if (varlist == null)
                {
                    return dtReturn;
                }

                foreach (T rec in varlist)
                {
                    // Use reflection to get property names, to create table, Only first time, others will follow
                    if (oProps == null)
                    {
                        oProps = ((Type)rec.GetType()).GetProperties();

                        foreach (PropertyInfo pi in oProps)
                        {
                            Type colType = pi.PropertyType;

                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }

                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                    }

                    DataRow dr = dtReturn.NewRow();

                    foreach (PropertyInfo pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                    }

                    dtReturn.Rows.Add(dr);
                }

                return dtReturn;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("LINQToDataTable()", ex));

                throw;
            }
        }

        public static string CheckUserNameExistence(string userName)
        {
            var rtnValue = string.Empty;

            message = string.Format("CheckUserNameExistence() Parameters = userName: {0}", userName);
            Log.Info(message);

            try
            {
                var entity = GetEntity();
                var record = entity.Users.Any(m => m.UserName.ToUpper() == userName.Trim().ToUpper());

                if (record == true)
                {
                    rtnValue = "1"; // record exists
                }
                else
                {
                    rtnValue = "0"; // no record found
                }
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("CheckUserNameExistence()", ex);
            }

            return rtnValue;
        }

        public static string CreateNewUser(string userName, string applicationName, string email, string password, string[] roles)
        {
            var rtnValue = string.Empty;

            message = string.Format("CreateNewUser() Parameters = userName: {0}, applicationName: {1}, email: {2}, password: {3}", userName, applicationName, email, password);
            Log.Info(message);

            try
            {
                var user = new Users();

                user.UserName = userName.Trim();
                user.ApplicationName = applicationName.Trim();
                user.Email = email.Trim();
                user.Password = password.Trim();
                user.IsApproved = true;
                user.RegisteredDate = DateTime.Now;

                var entity = GetEntity();

                entity.AddToUsers(user);
                rtnValue = entity.SaveChanges().ToString();

                // get newly created record
                string userId = user.UserId.ToString();

                //foreach (string role in roles)
                //{
                //    var usersInRoles = new UsersInRoles();

                //    int UserId = Int32.Parse(userId);
                //    int RoleId = Int32.Parse(role);

                //    var record = entity.UsersInRoles.Any(m => m.UserId == UserId && m.RoleId == RoleId);

                //    if (record == true)
                //    {
                //        // record exists
                //    }
                //    else
                //    {
                //        usersInRoles.UserId = UserId;
                //        usersInRoles.RoleId = RoleId;
                //        usersInRoles.ApplicationName = applicationName;

                //        entity.AddToUsersInRoles(usersInRoles);
                //        entity.SaveChanges();
                //    }
                //}
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("CreateNewUser()", ex);
            }

            return rtnValue;
        }

        public static string GetUserRoles()
        {
            var rtnValue = string.Empty;

            try
            {
                var entity = GetEntity();

                ObjectSet<Assette.ReportGallery.Data.Models.Roles> roles = entity.Roles;

                var objects =
                    (from role in roles
                     orderby role.RoleId
                     select new
                     {
                         ID = role.RoleId,
                         RoleName = role.RoleName
                     }).Distinct();


                var serializer = new JavaScriptSerializer();
                rtnValue = serializer.Serialize(objects);
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("GetUserRoles()", ex);
            }

            return rtnValue;
        }

        #endregion


        #region PAOBJECTS MOCKUPS

        public static bool SavePAObjectMockups(byte[] byteArray, Guid UploadedBy, int PageImageWidth, int PageImageHeight, int ThumbImageWidth, int ThumbImageHeight)
        {
            message = string.Format("SavePAObjectMockups() Parameters = UploadedBy: {0}, PageImageWidth: {1}, PageImageHeight: {2}, ThumbImageWidth: {3}, ThumbImageHeight: {4}", UploadedBy, PageImageWidth, PageImageHeight, ThumbImageWidth, ThumbImageHeight);
            Log.Info(message);

            int index = 0;
            int indexInsert = 0;
            int indexUpdate = 0;

            List<Information> PAObjectMockupsList = new List<Information>();

            try
            {
                // convert byte array to memory stream
                var memoryStream = new MemoryStream(byteArray, 0, (byteArray.Length - 1), true);

                var entity = GetEntity();

                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;

                Stripper stripper = new Stripper();

                // get shell object details
                PAObjectMockupsList = stripper.SlideInformation(memoryStream);

                foreach (Information PAObjectMockup in PAObjectMockupsList)
                {
                    // get category id by category name
                    var category =
                    (from pAObjectCategory in pAObjectCategories
                     where pAObjectCategory.Category.Trim().ToLower() == PAObjectMockup.Category.Trim().ToLower()
                     select new
                     {
                         Id = pAObjectCategory.ID
                     }).FirstOrDefault();

                    // get type id by type name
                    var type =
                    (from pAObjectType in pAObjectTypes
                     where pAObjectType.Type.Trim().ToLower() == PAObjectMockup.ObjectType
                     select new
                     {
                         Id = pAObjectType.ID
                     }).FirstOrDefault();

                    // try to get record by name, category id, type id
                    var mockupExists = entity.PAObjectMockups1.FirstOrDefault(e => e.Name == PAObjectMockup.Name && e.ObjectCategoryID == category.Id && e.ObjectTypeID == type.Id);

                    if (mockupExists != null)
                    {
                        message = string.Format("SavePAObjectMockups() - Update - Parameters = Name: {0}, ObjectCategoryId: {1}, ObjectTypeId: {2}, Description: {3}, IsEasy: {4}", PAObjectMockup.Name.Trim(), category.Id, type.Id, PAObjectMockup.Description.Trim(), PAObjectMockup.IsEasy);
                        Log.Info(message);

                        mockupExists.Name = PAObjectMockup.Name.Trim();
                        mockupExists.ObjectCategoryID = category.Id;
                        mockupExists.ObjectTypeID = type.Id;
                        mockupExists.Description = PAObjectMockup.Description.Trim();
                        mockupExists.ObjectImage = PAObjectMockup.FileImage;
                        mockupExists.ObjectThumbnail = PAObjectMockup.ThumbnailImage;
                        mockupExists.UploadedBy = UploadedBy;
                        mockupExists.UploadedDate = DateTime.Now;
                        mockupExists.ViewCount = 0;
                        mockupExists.PreRegisterPreView = false;

                        if (PAObjectMockup.IsEasy)
                        {
                            mockupExists.ClientType = "Easy";
                        }
                        else
                        {
                            mockupExists.ClientType = "Standard";
                        }

                        entity.SaveChanges();

                        // increment
                        index++;
                        indexUpdate++;
                    }
                    else
                    {
                        message = string.Format("SavePAObjectMockups() - Insert - Parameters = Name: {0}, ObjectCategoryId: {1}, ObjectTypeId: {2}, Description: {3}, IsEasy: {4}", PAObjectMockup.Name.Trim(), category.Id, type.Id, PAObjectMockup.Description.Trim(), PAObjectMockup.IsEasy);
                        Log.Info(message);

                        PAObjectMockup mockup = new PAObjectMockup();

                        mockup.Name = PAObjectMockup.Name.Trim();
                        mockup.ObjectCategoryID = category.Id;
                        mockup.ObjectTypeID = type.Id;
                        mockup.Description = PAObjectMockup.Description.Trim();
                        mockup.ObjectImage = PAObjectMockup.FileImage;
                        mockup.ObjectThumbnail = PAObjectMockup.ThumbnailImage;
                        mockup.UploadedBy = UploadedBy;
                        mockup.UploadedDate = DateTime.Now;
                        mockup.ViewCount = 0;
                        mockup.PreRegisterPreView = false;

                        if (PAObjectMockup.IsEasy)
                        {
                            mockup.ClientType = "Easy";
                        }
                        else
                        {
                            mockup.ClientType = "Standard";
                        }

                        entity.PAObjectMockups1.AddObject(mockup);
                        entity.SaveChanges();

                        // increment
                        index++;
                        indexInsert++;
                    }
                }

                message = string.Format("SavePAObjectMockups() - Insert/Update Successful ListCount: {0}, IndexCount: {1}, InsertCount: {2}, UpdateCount: {3}", PAObjectMockupsList.Count, index, indexInsert, indexUpdate);
                Log.Info(message);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("SavePAObjectMockups()", ex);

                message = string.Format("SavePAObjectMockups() - Insert/Update Error ListCount: {0}, IndexCount: {1}, InsertCount: {2}, UpdateCount: {3}", PAObjectMockupsList.Count, index, indexInsert, indexUpdate);
                Log.Info(message);

                return false;
            }
        }

        public static string UpdateReportObjectMockup(string Id, string name, string description, string clientType, string category, string type, string preRegisterPreView)
        {
            var rtnValue = string.Empty;

            message = string.Format("UpdateReportObjectMockup() Parameters = Id: {0}, name: {1}, description: {2}, clientType: {3}, category: {4}, type: {5} , preRegisterPreView: {6}", Id, name, description, clientType, category, type, preRegisterPreView);
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                int recordId = int.Parse(Id);

                ObjectSet<PAObjectMockup> PAObjectMockups = entity.PAObjectMockups1;

                var reportObject = PAObjectMockups.FirstOrDefault(m => m.ID == recordId);

                reportObject.Name = name.Trim();
                reportObject.Description = description.Trim();
                reportObject.ClientType = clientType;
                reportObject.ObjectCategoryID = int.Parse(category);
                reportObject.ObjectTypeID = int.Parse(type);

                if (preRegisterPreView == "True")
                {
                    reportObject.PreRegisterPreView = true;
                }
                else
                {
                    reportObject.PreRegisterPreView = false;
                }

                rtnValue = entity.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("UpdateReportObject()", ex);
            }

            return rtnValue;
        }

        public static string GetReportObjectMockups(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator)
        {
            Helper hlpr = new Helper();

            int recordCount = 0;

            DataTable tbl = GetReportObjectMockupData(sidx, sord, page, rows, search, searchField, searchString, searchOperator, out recordCount);

            return hlpr.JsonForJqgrid(tbl, rows, recordCount, page);
        }

        public static DataTable GetReportObjectMockupData(string sidx, string sord, int page, int rows, bool search, string searchField, string searchString, string searchOperator, out int recordCount)
        {
            DataTable rtnValue = new DataTable();

            message = string.Format("GetReportObjectMockupData() Parameters = sidx: {0}, sord: {1}, page: {2}, rows: {3}", sidx, sord, page.ToString(), rows.ToString());
            Log.Info(message);

            int RecordCount = 0;

            int initialRecordCount = rows;
            int startRow = (page - 1) * initialRecordCount;

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObjectMockup> pAObjectMockups = entity.PAObjectMockups1;
                ObjectSet<PAObjectCategory> pAObjectCategories = entity.PAObjectCategories1;
                ObjectSet<PAObjectType> pAObjectTypes = entity.PAObjectTypes1;

                var Objects =
                     (from pAObjectMockup in pAObjectMockups
                      join pAObjectCategory in pAObjectCategories on pAObjectMockup.ObjectCategoryID equals pAObjectCategory.ID
                      join pAObjectType in pAObjectTypes on pAObjectMockup.ObjectTypeID equals pAObjectType.ID
                      orderby pAObjectMockup.ID
                      select new
                      {
                          ID = pAObjectMockup.ID,
                          Name = pAObjectMockup.Name,
                          Description = pAObjectMockup.Description,
                          ClientType = pAObjectMockup.ClientType,
                          CategoryId = pAObjectMockup.ObjectCategoryID,
                          Category = pAObjectCategory.Category,
                          TypeId = pAObjectMockup.ObjectTypeID,
                          Type = pAObjectType.Type,
                          PreRegisterPreView = pAObjectMockup.PreRegisterPreView
                      });

                if (search == true)
                {
                    // filter by ID
                    if (searchField == "ID")
                    {
                        int Id = 0;
                        bool valid = Int32.TryParse(searchString, out Id);

                        if (!valid)
                        {
                            Id = 0;
                        }

                        Objects = Objects.Where(e => e.ID == Id);
                    }

                    RecordCount = Objects.Count();

                    // filter by Name
                    if (searchField == "Name")
                    {
                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.Name.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            Objects = Objects.Where(e => e.Name.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by Description
                    if (searchField == "Description")
                    {
                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.Description.ToUpper() == searchString.ToUpper());
                        }

                        if (searchOperator == "cn")
                        {
                            Objects = Objects.Where(e => e.Description.ToUpper().Contains(searchString.ToUpper()));
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by ClientType
                    if (searchField == "ClientType")
                    {
                        string ClientTypeId = searchString;

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.ClientType == ClientTypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.ClientType != ClientTypeId);
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by Category
                    if (searchField == "Category")
                    {
                        int CategoryId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.CategoryId == CategoryId);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.CategoryId != CategoryId);
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by ProductType
                    if (searchField == "Type")
                    {
                        int TypeId = int.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.TypeId == TypeId);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.TypeId != TypeId);
                        }
                    }

                    RecordCount = Objects.Count();

                    // filter by PreRegisterPreView
                    if (searchField == "PreRegisterPreView")
                    {
                        bool PreRegisterPreView = bool.Parse(searchString);

                        if (searchOperator == "eq")
                        {
                            Objects = Objects.Where(e => e.PreRegisterPreView == PreRegisterPreView);
                        }

                        if (searchOperator == "ne")
                        {
                            Objects = Objects.Where(e => e.PreRegisterPreView != PreRegisterPreView);
                        }
                    }

                    RecordCount = Objects.Count();
                }

                RecordCount = Objects.Count();

                // sorting by Id
                if (sord == "desc")
                {
                    Objects = Objects.OrderByDescending(x => x.ID);
                }

                RecordCount = Objects.Count();

                // paging functionality
                Objects = Objects.Skip(startRow).Take(initialRecordCount);

                int pagingRecordCount = Objects.Count();

                // serializing
                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(Objects);

                // converting json string to datatable
                rtnValue = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                rtnValue = null; // error

                Log.Error("GetReportObjectMockupData()", ex);
            }

            // assign record count for paging - out parameter
            recordCount = RecordCount;

            return rtnValue;
        }

        public static string DeleteReportObjectMockup(int Id)
        {
            var rtnValue = string.Empty;

            message = string.Format("DeleteReportObjectMockup() Parameters = Id: {0}", Id.ToString());
            Log.Info(message);

            try
            {
                var entity = GetEntity();

                ObjectSet<PAObjectMockup> pAObjectMockups1 = entity.PAObjectMockups1;

                // delete paobject mockup
                var pAObjectMockup = entity.PAObjectMockups1.FirstOrDefault(pao => pao.ID == Id);

                entity.PAObjectMockups1.DeleteObject(pAObjectMockup);

                // save entity
                rtnValue = entity.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                rtnValue = "-1"; // error

                Log.Error("DeleteReportObjectMockup()", ex);
            }

            return rtnValue;
        }

        #endregion


        #region CUSTOM MEMBERSHIP

        public class CustomMemberShipUser : MembershipUser
        {
            public CustomMemberShipUser(
                string providerName,
                string email,
                object providerUserKey,
                string name,
                string passwordQuestion,
                string comment,
                bool isApproved,
                bool isLockedOut,
                DateTime creationDate,
                DateTime lastLoginDate,
                DateTime lastActivityDate,
                DateTime lastPasswordChangedDate,
                DateTime lastLockoutDate
                )
                : base(
                   providerName, email, providerUserKey, name, passwordQuestion,
                   comment, isApproved, isLockedOut, creationDate, lastLoginDate,
                   lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
            {
            }
        }

        #endregion
    }
}
