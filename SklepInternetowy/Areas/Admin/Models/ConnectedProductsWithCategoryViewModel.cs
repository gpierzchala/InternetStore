using System.Collections.Generic;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class ConnectedProductsWithCategoryViewModel
    {
        public string CategoryName { get; set; }
        public List<ProductData> ProductData { get; set; }
    }

    public class ProductData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsBestseller { get; set; }
        public bool IsRecent { get; set; }
        public bool IsFeatured { get; set; }
    }
}