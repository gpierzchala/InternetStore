namespace SklepInternetowy.Areas.Admin.Models
{
    public class OrderDetailsModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}