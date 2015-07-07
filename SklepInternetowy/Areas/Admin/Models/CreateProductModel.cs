using System.Web;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public int Quantity { get; set; }
        public bool IsRecent { get; set; }
        public bool IsBestseller { get; set; }
        public bool IsFeatured { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}