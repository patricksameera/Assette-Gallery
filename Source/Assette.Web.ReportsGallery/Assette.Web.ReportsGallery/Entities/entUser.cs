
namespace Assette.Web.ReportsGallery.Entities
{
    public class entUser : ent
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string RegisteredIP { get; set; }
        public string RegisteredDate { get; set; }       
    }
}