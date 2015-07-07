namespace SklepInternetowy.Models
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Manufacturer { get; set; }
        public int Quantity { get; set; }
        public bool IsBestSeller { get; set; }
        public string ShortDescription { get; set; }
    }
}