namespace SklepInternetowy.Areas.Admin.Models
{
    public class ProductDetailsViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public int AvailableItemCount { get; set; }
        public bool IsRecent { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsBestseller { get; set; }
        public string ShortDescription { get; set; }

    }
}